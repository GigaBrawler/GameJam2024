using Core;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

namespace MiniGame7
{
    public class CatBoxGame : MiniGameCore
    {
        [Header("CatBox Data")]
        [SerializeField] private float air; //Esta variable es el aire.
        [SerializeField] private GameObject[] boxes;
        public bool Win;

        [Header("Audio")]
        public AudioSource open;
        [SerializeField] private AudioSource dwarf;

        void Awake() {
            var number = Random.Range(0, boxes.Length);
            boxes[number].GetComponent<BoxScript>().hasCat = true;
            boxes[number].GetComponent<SpriteRenderer>().sprite = 
                boxes[number].GetComponent<BoxScript>().hasCat ? 
                    boxes[number].GetComponent<BoxScript>().closedBoxCat : 
                    boxes[number].GetComponent<BoxScript>().closedBoxNoCat;
        }

        public override void EndGame() {
            if (gameHasEnded) return;
            if (!Win) {
                GameManager.Instance.boo.Play();
                GameManager.Instance.lives -= 1;
            } else
            {
                dwarf.Play();
                GameManager.Instance.yay.Play();
                GameManager.Instance.gamesWon++;
            }
            Debug.Log(Win ? "You Won!" : "What a loser...");
            base.EndGame();
        }
    }  
}
