using System;
using UnityEngine;

namespace Core
{
    public class MiniGameCore : MonoBehaviour
    {
        public bool gameHasEnded; //Esta variable se encarga de saber si el juego ha terminado.
        
        private void Start() { //Aquí se asigna este minijuego como minijuego actual en el manager.
            GameManager.Instance.currentMiniGame = this;
            GameManager.Instance.music.Play();
        }

        public virtual void EndGame() { //Esta función gestiona todos los finales de juego.
            GameManager.Instance.timeModifier += 0.1f;
            if (GameManager.Instance.lives > 0) 
                GameManager.Instance.EndOfGame();
            else 
                GameManager.Instance.BackToMenu();
            gameHasEnded = true;
        }
    }
}