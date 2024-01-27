using Core;
using Unity.VisualScripting;
using UnityEngine;

namespace MiniGame4
{
    public class BalanceGame : MiniGameCore
    {
        [Header("Balance Data")] 
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private GameObject guy;
        [SerializeField] private float forceMultiplier;

        [Header("Clown Management")] 
        [SerializeField] private GameObject clownObject;
        [SerializeField] private Sprite clown;
        [SerializeField] private Sprite clownBlow;

        void Start()
        {
            var randomNumber = Random.Range(-1, 1);
            if (randomNumber == 0) randomNumber = 1;
            rb.AddTorque(30 * Mathf.Deg2Rad * randomNumber * 3000 * Time.deltaTime, ForceMode2D.Force);
        }
        
        void Update() {
            var direction = -Input.GetAxisRaw("Horizontal");
            if (gameHasEnded) return;
            switch (direction) {
                case 1:
                    clownObject.GetComponent<SpriteRenderer>().flipX = false;
                    clownObject.GetComponent<SpriteRenderer>().sprite = clownBlow;
                    break;
                case -1:
                    clownObject.GetComponent<SpriteRenderer>().flipX = true;
                    clownObject.GetComponent<SpriteRenderer>().sprite = clownBlow;
                    break;
                case 0:
                    clownObject.GetComponent<SpriteRenderer>().sprite = clown;
                    break;
            }
            var impulse = (30 * Mathf.Deg2Rad * direction * forceMultiplier) * rb.inertia * Time.deltaTime;
            rb.AddTorque(impulse, ForceMode2D.Force);
            if (guy.transform.eulerAngles.z is <= 225 and >= 135) return;
            guy.GetComponentInChildren<PolygonCollider2D>().enabled = false;
            EndGame();
        }

        public override void EndGame() { //Este void, al ser override, toma la función base de MiniGameCore y la modifica.
            //Aquí se añaden las condiciones de victoria, muy importante poner esto de aquí abajo: (if (GameHasEnded) return;)
            if (gameHasEnded) return;
            clownObject.GetComponent<SpriteRenderer>().sprite = clown;
            if (guy.transform.eulerAngles.z is > 210 or < 150) GameManager.Instance.lives -= 1;
            else guy.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            Debug.Log(guy.transform.eulerAngles.z is < 210 and > 150 ? "You Won!" : "What a loser...");
            base.EndGame();
        }
    }
}
