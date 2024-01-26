using System;
using System.Collections.Generic;
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

        private void Start() {
            RepopulateLevels();
        }

        private void RepopulateLevels() {
            for (var i = 0; i < EditorBuildSettings.scenes.Length; i++) {
                if (i <= 0) continue;
                _availableGames.Add(i);
            }
        }

        private int GetRandomLevel() {
            var index = Random.Range(0, _availableGames.Count);
            var game = _availableGames[index];
            _availableGames.Remove(game);
            return game;
        }

        public void LoadRandomLevel() {
            if (_availableGames.Count <= 0)
                RepopulateLevels();
            var index = GetRandomLevel();
            SceneManager.LoadSceneAsync(index);
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Space))
                LoadRandomLevel();
        }
    }
}
