using System;
using UnityEngine;

namespace Core
{
    public class MiniGameCore : MonoBehaviour
    {
        protected bool GameHasEnded; //Esta variable se encarga de saber si el juego ha terminado.
        
        private void Start() { //Aquí se asigna este minijuego como minijuego actual en el manager.
            GameManager.Instance.currentMiniGame = this;
        }

        public virtual void EndGame() { //Esta función gestiona todos los finales de juego.
            GameManager.Instance.EndOfGame();
            GameHasEnded = true;
        }
    }
}