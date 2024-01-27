using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using MiniGame7;

public class BoxScript : MonoBehaviour, IPointerClickHandler
{
    [Header("Game Data")]
    public CatBoxGame game;

    [Header("Cat Data")]
    public bool hasCat;

    public void OnPointerClick(PointerEventData eventData) {
            Debug.Log(hasCat ? "Meow!" : "Fuck you!");
            if(hasCat) {
                game.Win = true;
            }
            game.EndGame();
    }
}
