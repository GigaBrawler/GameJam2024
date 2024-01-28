using Core;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MiniGame6
{
    public class DuckScript : MonoBehaviour, IPointerClickHandler
    {
        [Header("Mini Game Data")] 
        public DucksGame game;
        
        [Header("Duck Data")]
        public int id;
        [SerializeField] private Rigidbody2D rb;

        [Header("Audio")] 
        [SerializeField] private AudioSource quack;

        void Start() {
            rb = GetComponent<Rigidbody2D>();
        }

        void Update()
        {
            rb.velocity = new Vector2(-1, 0) * game.duckSpeed;
            if (transform.position.x < -10)
                Destroy(this.gameObject);
        }
        
        public void OnPointerClick(PointerEventData eventData) {
            if (game.gameHasEnded) 
                return;
            quack.Play();
            Debug.Log($"My ID is {id}");
            if (id != game.id) {
                game.EndGame();
            } else {
                game.gotTheDuck = true;
                game.EndGame();
            }
            game.gameHasEnded = true;
        }
    }
}
