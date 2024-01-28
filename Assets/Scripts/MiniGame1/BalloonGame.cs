using Core;
using UnityEngine;

namespace MiniGame1
{
    public class BalloonGame : MiniGameCore
    {
        [Header("Balloon Data")] 
        [SerializeField] private float air; //Esta variable es el aire.
        [SerializeField] private GameObject balloon;

        [Header("Audio")] 
        [SerializeField] private AudioSource pop;
        [SerializeField] private AudioSource blow;
        [SerializeField] private AudioSource inflate;
        
        void Update() {
            if (gameHasEnded) return;
            var size = 1 + (air / 80);
            balloon.transform.localScale = new Vector3(size, size, size);
            if (Input.GetKeyDown(KeyCode.Space)) {
                //Si le doy al espacio, suma aire.
                inflate.pitch = 1f - (air / 375);
                blow.Play();
                inflate.Play();
                air += 10;
            }
            if (air <= 0) return; //Si el aire es menor o igual a cero, no hagas nada.
            air -= Time.deltaTime * (air / 7.9f); //Si el aire es mayor a cero, ves restándolo con el tiempo.
            if (!(air >= 375)) return;
            //Si el aire es mayor a 500, has ganado!
            pop.Play();
            balloon.SetActive(false);
            EndGame();
        }

        public override void EndGame() { //Este void, al ser override, toma la función base de MiniGameCore y la modifica.
            //Aquí se añaden las condiciones de victoria, muy importante poner esto de aquí abajo: (if (GameHasEnded) return;)
            if (gameHasEnded) return;
            if (air < 375)
            {
                GameManager.Instance.boo.Play();
                GameManager.Instance.lives -= 1;
            }
            else
            {
                GameManager.Instance.yay.Play();
                GameManager.Instance.gamesWon++;
            }
            Debug.Log(air >= 375 ? "You Won!" : "What a loser...");
            base.EndGame();
        }
    }
}
