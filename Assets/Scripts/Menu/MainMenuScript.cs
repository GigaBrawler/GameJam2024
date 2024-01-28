using System;
using Core;
using TMPro;
using UnityEngine;

namespace Menu
{
    public class MainMenuScript : MonoBehaviour
    {
        [Header("Games Data")] 
        [SerializeField] private TextMeshProUGUI highscore;
        [SerializeField] private TextMeshProUGUI score;
        [SerializeField] private GameObject credits;
        
        public void StartGame()
        {
            GameManager.Instance.StartGame();
        }

        public void ShowCredits()
        {
            credits.SetActive(true);
        }

        private void Start()
        {
            highscore.text = "Highest Score:\n" + PlayerPrefs.GetInt("Games").ToString();
            score.text = "Last Score:\n" + GameManager.Instance.gamesWon.ToString();
        }
    }
}
