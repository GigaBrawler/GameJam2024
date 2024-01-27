using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MiniGame6
{
    public class DucksGame : MiniGameCore
    {
        [Header("Ducks Data")] 
        [SerializeField] private List<GameObject> ducks = new List<GameObject>();
        public GameObject duckToGet;
        public int id;
        private Coroutine _duckCoroutine;
        [SerializeField] private SpriteRenderer duckSprite;
        public float duckSpeed;
        public bool gotTheDuck;

        void Start() {
            var duck = ducks[Random.Range(0, ducks.Count)];
            duckToGet = duck;
            duckSpeed += GameManager.Instance.timeModifier;
            id = duck.GetComponent<DuckScript>().id;
            duckSprite.sprite = duck.GetComponent<SpriteRenderer>().sprite;
        }

        void Update()
        {
            _duckCoroutine ??= StartCoroutine(nameof(PopulateDucks));
        }

        private IEnumerator PopulateDucks()
        {
            var duck = Instantiate(ducks[Random.Range(0, ducks.Count)]);
            duck.GetComponent<DuckScript>().game = this;
            duck.transform.position = new Vector3(10, 0);
            yield return new WaitForSeconds(1f);
            _duckCoroutine = null;
        }
        
        public override void EndGame() {
            if (gameHasEnded) return;
            if (!gotTheDuck) GameManager.Instance.lives -= 1;
            Debug.Log(gotTheDuck ? "You Won!" : "What a loser...");
            base.EndGame();
        }
    }
}
