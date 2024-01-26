using Core;
using UnityEngine;

namespace MiniGame1
{
    public class BalloonGame : MiniGameCore
    {
        [Header("Balloon Data")] 
        [SerializeField] private float air; //Esta variable es el aire.
        
        void Update() {
            if (Input.GetKeyDown(KeyCode.Space)) //Si le doy al espacio, suma aire.
                air += 10;
            if (air <= 0) return; //Si el aire es menor o igual a cero, no hagas nada.
            air -= Time.deltaTime * (air / 7.5f); //Si el aire es mayor a cero, ves restándolo con el tiempo.
            if (air >= 500) //Si el aire es mayor a 500, has ganado!
                EndGame();
        }

        public override void EndGame() { //Este void, al ser override, toma la función base de MiniGameCore y la modifica.
            //Aquí se añaden las condiciones de victoria, muy importante poner esto de aquí abajo: (if (GameHasEnded) return;)
            if (GameHasEnded) return;
            if (air < 500) GameManager.Instance.lives -= 1; //Aquí se restan vidas si pierdes.
            Debug.Log(air >= 500 ? "You Won!" : "What a loser...");
            base.EndGame();
        }
    }
}
