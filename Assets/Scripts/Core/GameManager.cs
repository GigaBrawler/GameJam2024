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
        [SerializeField] private bool loadingLevel;
        public float timeModifier;
        public float timeLeft;
        public int gamesWon;
        private int _nextLevel;

        [Header("Mini Game Management")] 
        public int lives;
        public bool startGame;
        public MiniGameCore currentMiniGame;

        [Header("UI")] 
        [SerializeField] private Image timeBar;
        [SerializeField] private Image livesImage;
        [SerializeField] private TextMeshProUGUI promptText;
        [SerializeField] private TextMeshProUGUI timeText;
        [SerializeField] private Animator telon;

        [Header("Sound")] 
        public AudioSource yay;
        public AudioSource boo;
        public AudioSource music;

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
            SceneManager.LoadSceneAsync(index);
        }

        public void EndOfGame() //Termina el juego y carga el siguiente nivel.
        {
            StopAllCoroutines();
            Debug.Log("The game has ended, loading next game...");
            StartCoroutine(nameof(LoadNextLevelCoroutine));
        }

        public void BackToMenu()
        {
            PlayerPrefs.SetInt("Games", gamesWon);
            PlayerPrefs.Save();
            StopAllCoroutines();
            StartCoroutine(nameof(BackToMenuCoroutine));
        }

        private void HandleUI()
        {
            livesImage.fillAmount = lives / 5f;
            if (!startGame) {
                if (timeBar.gameObject.activeSelf) timeBar.gameObject.SetActive(false);
                return;
            }
            if (!timeBar.gameObject.activeSelf) timeBar.gameObject.SetActive(true);
            timeBar.fillAmount = timeLeft / (10f - timeModifier);
            timeText.text = Mathf.RoundToInt(timeLeft).ToString();
        }

        private void Update()
        {
            HandleUI();
            if (!startGame) {
                lives = 5;
                return;
            }
            if (timeLeft > 0) {
                if (!loadingLevel) timeLeft -= Time.deltaTime;
            } else if (!_loadLevel) {
                if (currentMiniGame != null)
                    currentMiniGame.EndGame();
                _loadLevel = true;
                StartCoroutine(nameof(LoadNextLevelCoroutine));
            }
        }

        public void StartGame() {
            StartCoroutine(nameof(LoadNextLevelCoroutine));
        }

        private IEnumerator LoadNextLevelCoroutine() //Espera cinco segundos y carga un nivel (Aquí debemos mostrar la pantalla de fin de juego)
        {
            telon.SetTrigger("Close");
            _nextLevel = GetRandomLevel();
            promptText.text = GetPromptText(_nextLevel);
            loadingLevel = true;
            promptText.gameObject.SetActive(true);
            yield return new WaitForSeconds(3f);
            if (!startGame) {
                gamesWon = 0;
                startGame = true;
            }
            timeLeft = timeModifier < 10f ? 10f - timeModifier : 0f;
            _loadLevel = false;
            loadingLevel = false;
            telon.SetTrigger("Open");
            promptText.gameObject.SetActive(false);
            LoadRandomLevel(_nextLevel);
        }
        
        private IEnumerator BackToMenuCoroutine() //Espera cinco segundos y carga un nivel (Aquí debemos mostrar la pantalla de fin de juego)
        {
            telon.SetTrigger("Close");
            startGame = false;
            yield return new WaitForSeconds(3f);
            timeLeft = 0f;
            telon.SetTrigger("Open");
            Debug.Log("HAHA YOU LOST!");
            SceneManager.LoadSceneAsync(0);
            timeModifier = 0;
        }
    }
}
