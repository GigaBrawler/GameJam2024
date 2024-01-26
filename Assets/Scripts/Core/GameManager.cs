using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace Core
{
    public class GameManager : Singleton<GameManager>
    {
        [Header("Scene Management")] 
        [SerializeField] private List<int> availableGames = new List<int>();
        [SerializeField] private List<int> playedGames = new List<int>();
        
        private int GetRandomLevel()
        {
            var index = Random.Range(0, availableGames.Count);
            var game = availableGames[index];
            if (playedGames.Contains(game)) return -1;
            playedGames.Add(game);
            availableGames.Remove(game);
            Debug.Log($"Game Index: {game}");
            return game;
        }

        public void LoadRandomLevel()
        {
            if (availableGames.Count <= 0) {
                Debug.Log("No more games!");
                return;
            }
            var index = GetRandomLevel();
            SceneManager.LoadSceneAsync(index);
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Space))
                LoadRandomLevel();
        }
    }
}
