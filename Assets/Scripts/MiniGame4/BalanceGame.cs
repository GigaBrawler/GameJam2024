using Core;
using UnityEngine;

namespace MiniGame4
{
    public class BalanceGame : MiniGameCore
    {
        [Header("Balance Data")] 
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private GameObject guy;
        [SerializeField] private float forceMultiplier;

        void Start()
        {
            var randomNumber = Random.Range(-1, 1);
            if (randomNumber == 0) randomNumber = 1;
            rb.AddTorque(30 * Mathf.Deg2Rad * randomNumber * 50, ForceMode2D.Force);
        }
        
        void Update() {
            if (GameHasEnded) return;
            var direction = -Input.GetAxisRaw("Horizontal");
            var impulse = (30 * Mathf.Deg2Rad * direction * forceMultiplier) * rb.inertia;
            rb.AddTorque(impulse, ForceMode2D.Force);
            if (guy.transform.eulerAngles.z is > 210 or < 150)
                EndGame();
        }

        public override void EndGame() { //Este void, al ser override, toma la función base de MiniGameCore y la modifica.
            //Aquí se añaden las condiciones de victoria, muy importante poner esto de aquí abajo: (if (GameHasEnded) return;)
            if (GameHasEnded) return;
            if (guy.transform.eulerAngles.z is > 210 or < 150) GameManager.Instance.lives -= 1;
            Debug.Log(guy.transform.eulerAngles.z is < 210 and > 150 ? "You Won!" : "What a loser...");
            base.EndGame();
        }
    }
}
