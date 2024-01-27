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
                    Box1.GetComponent<SpriteRenderer>().sprite = 
                        Box1.GetComponent<BoxScript>().hasCat ? 
                            Box1.GetComponent<BoxScript>().closedBoxCat : 
                            Box1.GetComponent<BoxScript>().closedBoxNoCat;
                break;
                case -1:
                    Box2.GetComponent<BoxScript>().hasCat = true;
                    Box2.GetComponent<SpriteRenderer>().sprite = 
                        Box2.GetComponent<BoxScript>().hasCat ? 
                            Box2.GetComponent<BoxScript>().closedBoxCat : 
                            Box2.GetComponent<BoxScript>().closedBoxNoCat;
                break;
            }
        }

        public override void EndGame() {
            if (gameHasEnded) return;
            if (!Win) GameManager.Instance.lives -= 1;
            Debug.Log(Win ? "You Won!" : "What a loser...");
            base.EndGame();
        }
    }  
}
