using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MiniGame7
{
    public class BoxScript : MonoBehaviour, IPointerClickHandler
    {
        [Header("Game Data")]
        public CatBoxGame game;

        [Header("Cat Data")]
        public bool hasCat;
        public Sprite closedBoxNoCat;
        public Sprite closedBoxCat;
        public Sprite openBoxNoCat;
        public Sprite openBoxCat;

        void Awake() {
            
        }

        public void OnPointerClick(PointerEventData eventData) {
            if (game.gameHasEnded) return;
            Debug.Log(hasCat ? "Meow!" : "Fuck you!");
            if(hasCat) {
                gameObject.GetComponent<SpriteRenderer>().sprite = openBoxCat;
                game.Win = true;
            } else gameObject.GetComponent<SpriteRenderer>().sprite = openBoxNoCat;
            game.EndGame();
        }
    }
}
