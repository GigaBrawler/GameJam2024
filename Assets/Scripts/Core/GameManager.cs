using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Core
{
    public class GameManager : Singleton<GameManager>
    {
        [Header("Scene Management")] 
        private readonly List<int> _availableGames = new List<int>();
        private int _lastLevel;
        private bool _loadLevel;
        private bool _loadingLevel;
        public float timeModifier;
        public float timeLeft;
        public int gamesWon;
        private int _gamesPlayed;

        [Header("Mini Game Management")] 
        public int lives;
        public bool startGame;
        public MiniGameCore currentMiniGame;

        [Header("UI")] 
        [SerializeField] private Image timeBar;
        [SerializeField] private TextMeshProUGUI promptText;

        private void Start() { //Al principio, recoge todos los niveles posibles.
            RepopulateLevels();
        }
        
        private void RepopulateLevels() { //Encuentra los niveles cargados y añadelos a la lista.
            for (var i = 0; i < EditorBuildSettings.scenes.Length; i++) {
                if (i <= 0 || (i == _lastLevel)) continue;
                _availableGames.Add(i);
            }
        }

        private string GetPromptText(int nextLevel)
        {
            return nextLevel switch {
                1 => "QUICK! POP HIS HEAD!",
                2 => "QUICK! POSE!",
                3 => "QUICK! SPELL CORRECTLY!",
                4 => "HELP THE ANT KEEP BALANCE!",
                5 => "MAKE HIM BREAK HIS SILENCE VOTE!",
                6 => "Alright dude you can chill a bit... CHOOSE THE CORRECT DUCKTATOR!",
                7 => "QUICK! FIND THE HIDDEN DWARF!",
                _ => ""
            };
        }

        private int GetRandomLevel() { //Pilla un nivel random de la lista de niveles.
            var index = Random.Range(0, _availableGames.Count);
            var game = _availableGames[index];
            _availableGames.Remove(game);
            _lastLevel = game;
            return game;
        }

        private void LoadRandomLevel(int index) { //Carga un nivel random de la lista de niveles.
            if (_availableGames.Count <= 0)
                RepopulateLevels();
            SceneManager.LoadScene(index);
        }

        public void EndOfGame() //Termina el juego y carga el siguiente nivel.
        {
            StopAllCoroutines();
            Debug.Log("The game has ended, loading next game...");
            StartCoroutine(nameof(LoadNextLevelCoroutine));
        }

        private void HandleUI()
        {
            if (!startGame) {
                if (timeBar.gameObject.activeSelf) timeBar.gameObject.SetActive(false);
                if (promptText.gameObject.activeSelf) promptText.gameObject.SetActive(false);
                return;
            }
            if (!timeBar.gameObject.activeSelf) timeBar.gameObject.SetActive(true);
            promptText.gameObject.SetActive(_loadingLevel);
            timeBar.fillAmount = timeLeft / (10f - timeModifier);
        }

        private void Update()
        {
            HandleUI();
            if (!startGame) {
                lives = 5;
                return;
            }
            if (lives <= 0) { //Si pierde las vidas reinicia el juego;
                StopAllCoroutines();
                Debug.Log("HAHA YOU LOST!");
                SceneManager.LoadScene(0);
                timeModifier = 0;
                startGame = false;
                return;
            }
            if (timeLeft > 0) {
                if (!_loadingLevel) timeLeft -= Time.deltaTime;
            } else if (!_loadLevel) {
                if (currentMiniGame != null)
                    currentMiniGame.EndGame();
                _loadLevel = true;
                if (_gamesPlayed <= 0) {
                    timeLeft = timeModifier < 10f ? 10f - timeModifier : 0f;
                    _loadLevel = false;
                    _gamesPlayed++;
                    LoadRandomLevel(GetRandomLevel());
                } else
                    StartCoroutine(nameof(LoadNextLevelCoroutine));
            }
        }

        private IEnumerator LoadNextLevelCoroutine() //Espera cinco segundos y carga un nivel (Aquí debemos mostrar la pantalla de fin de juego)
        {
            _loadingLevel = true;
            var nextLevel = GetRandomLevel();
            promptText.text = GetPromptText(nextLevel);
            yield return new WaitForSeconds(3f);
            timeLeft = timeModifier < 10f ? 10f - timeModifier : 0f;
            _loadLevel = false;
            _gamesPlayed++;
            _loadingLevel = false;
            LoadRandomLevel(nextLevel);
        }
    }
}
