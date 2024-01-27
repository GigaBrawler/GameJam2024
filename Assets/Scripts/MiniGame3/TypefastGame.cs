using System.Collections.Generic;
using Core;
using TMPro;
using UnityEngine;

namespace MiniGame3
{
    public class TypefastGame : MiniGameCore
    {
        [Header("Typefast Data")]
        [SerializeField] private List<string> wordList = new List<string>();
        [SerializeField] private string word;
        [SerializeField] private TextMeshProUGUI wordText;
        [SerializeField] private TMP_InputField inputText;

        void Start() {
            word = wordList[Random.Range(0, wordList.Count)];
            wordText.text = word;
            inputText.ActivateInputField();
        }

        public void CheckForWord() {
            if (inputText.text == null) return;
            for (var i = 0; i < inputText.text.Length; i++) {
                if (inputText.text[i] != word[i])
                    EndGame();
            }
            if (inputText.text == word)
                EndGame();
        }

        public override void EndGame() {
            if (gameHasEnded) return;
            inputText.interactable = false;
            if (inputText.text != word) GameManager.Instance.lives -= 1;
            Debug.Log(inputText.text == word ? "You Won!" : "What a loser...");
            base.EndGame();
        }
    }
}
