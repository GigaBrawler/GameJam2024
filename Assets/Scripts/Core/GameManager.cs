using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace Core
{
    public class GameManager : Singleton<GameManager>
    {
        [Header("Scene Management")] 
        private readonly List<int> _availableGames = new List<int>();
        private int _lastLevel;
        private Coroutine _gameCoroutine;
        private float _timeModifier;

        [Header("Mini Game Management")] 
        public int lives;
        public bool startGame;
        public MiniGameCore currentMiniGame;

        private void Start() { //Al principio, recoge todos los niveles posibles.
            RepopulateLevels();
        }

        private void RepopulateLevels() { //Encuentra los niveles cargados y añadelos a la lista.
            for (var i = 0; i < EditorBuildSettings.scenes.Length; i++) {
                if (i <= 0 || (i == _lastLevel)) continue;
                _availableGames.Add(i);
            }
        }

        private int GetRandomLevel() { //Pilla un nivel random de la lista de niveles.
            var index = Random.Range(0, _availableGames.Count);
            var game = _availableGames[index];
            _availableGames.Remove(game);
            _lastLevel = game;
            return game;
        }

        private void LoadRandomLevel() { //Carga un nivel random de la lista de niveles.
            if (_availableGames.Count <= 0)
                RepopulateLevels();
            var index = GetRandomLevel();
            SceneManager.LoadSceneAsync(index);
        }

        public void EndOfGame() //Termina el juego y carga el siguiente nivel.
        {
            StopAllCoroutines();
            Debug.Log("The game has ended, loading next game...");
            StartCoroutine(nameof(LoadNextLevelCoroutine));
        }

        private void Update()
        {
            if (!startGame) {
                lives = 5;
                return;
            }
            if (lives > 0) {
                _gameCoroutine ??= StartCoroutine(nameof(GameLoopCoroutine));
            } else { //Si pierde las vidas reinicia el juego;
                StopAllCoroutines();
                Debug.Log("HAHA YOU LOST!");
                SceneManager.LoadSceneAsync(0);
                startGame = false;
            }
        }

        private IEnumerator GameLoopCoroutine() { //Carga un nivel, espera 10 segundos, termina el nivel, carga el siguiente.
            LoadRandomLevel();
            while (currentMiniGame == null)
                yield return null;
            yield return new WaitForSeconds(_timeModifier < 10f ? 10f - _timeModifier : 0f);
            _timeModifier += 0.1f;
            currentMiniGame.EndGame();
            StartCoroutine(nameof(LoadNextLevelCoroutine));
        }

        private IEnumerator LoadNextLevelCoroutine() //Espera cinco segundos y carga un nivel (Aquí debemos mostrar la pantalla de fin de juego)
        {
            yield return new WaitForSeconds(5f);
            _gameCoroutine = null;
        }
    }
}
