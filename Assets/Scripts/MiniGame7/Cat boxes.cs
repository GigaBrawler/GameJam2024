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
        [SerializeField] private GameObject Box1;
        [SerializeField] private GameObject Box2;
        public bool Win;

        void Start() {
            var number = Random.Range(-1, 1);
            switch (number) {
                case 0:
                    Box1.GetComponent<BoxScript>().hasCat = true;
                break;
                case -1:
                    Box2.GetComponent<BoxScript>().hasCat = true;
                break;
            }
        }

        public override void EndGame() {
            if (GameHasEnded) return;
            if (!Win) GameManager.Instance.lives -= 1;
            Debug.Log(Win ? "You Won!" : "What a loser...");
            base.EndGame();
        }
    }  
}
