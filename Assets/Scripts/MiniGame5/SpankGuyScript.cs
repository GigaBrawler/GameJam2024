using System;
using Core;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

namespace MiniGame5
{
    public class SpankGuyScript : MonoBehaviour, IPointerClickHandler
    {
        [Header("Game Data")]
        public SpankGame game;
        
        [Header("Guy Data")] 
        [SerializeField] private Rigidbody2D rb;
        private int _direction;
        [SerializeField] private float speed;
        private bool _switchDirection;
        private float _speedMultiplier;
        public int spanks;

        [Header("Mime Sprites")] 
        [SerializeField] private Sprite mime2;

        void Start() {
            var randomNumber = Random.Range(-1, 1);
            if (randomNumber == 0) randomNumber = 1;
            _direction = randomNumber;
            speed += GameManager.Instance.timeModifier * 5;
            _speedMultiplier = 1;
        }

        void Update()
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = (_direction == 1);
            if (transform.position.x is > 7.5f or < -7.5f && !_switchDirection) {
                _switchDirection = true;
                _direction = -_direction;
            }
            if (transform.position.x is (< 4f and > -5f) or (> -4f and < -5f))
                _switchDirection = false;
            rb.velocity = new Vector2(1, 0) * speed * _direction * _speedMultiplier;
        }
        
        public void OnPointerClick(PointerEventData eventData) {
            if (game.gameHasEnded) return;
            Debug.Log("Ouch!");
            _speedMultiplier += 0.5f;
            spanks++;
            if (spanks > 5)
                gameObject.GetComponent<SpriteRenderer>().sprite = mime2;
        }
    }
}
