using Core;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MiniGame5
{
    public class SpankGame : MiniGameCore
    {
        [Header("Spank Data")] 
        [SerializeField] private SpankGuyScript guy;

        void Update() {
            if (GameHasEnded) return;
            if (guy.spanks > 5)
                EndGame();
        }

        public override void EndGame() {
            if (GameHasEnded) return;
            if (guy.spanks <= 5) GameManager.Instance.lives -= 1;
            Debug.Log(guy.spanks > 5 ? "You Won!" : "What a loser...");
            base.EndGame();
        }
    }
}
