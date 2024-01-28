using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MiniGame3
{
    public class TypefastGame : MiniGameCore
    {
        [Header("Typefast Data")]
        [SerializeField] private List<string> wordList = new List<string>();
        [SerializeField] private string word;
        [SerializeField] private TextMeshProUGUI wordText;
        [SerializeField] private TMP_InputField inputText;
        private bool _started;

        [Header("Woman Data")] 
        [SerializeField] private SpriteRenderer woman;
        [SerializeField] private Sprite mouthOpen;
        [SerializeField] private Sprite mouthClosed;

        void Awake() {
            word = wordList[Random.Range(0, wordList.Count)];
            wordText.text = word;
            inputText.ActivateInputField();
        }

        public void CheckForWord() {
            if (inputText.text == null) return;
            StopAllCoroutines();
            StartCoroutine(nameof(Talk));
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
            if (inputText.text != word)
            {
                GameManager.Instance.boo.Play();
                GameManager.Instance.lives -= 1;
            }
            else
            {
                GameManager.Instance.yay.Play();
                GameManager.Instance.gamesWon++;
            }
            Debug.Log(inputText.text == word ? "You Won!" : "What a loser...");
            base.EndGame();
        }

        public IEnumerator Talk()
        {
            woman.sprite = mouthOpen;
            yield return new WaitForSeconds(0.05f);
            woman.sprite = mouthClosed;
        }
    }
}
