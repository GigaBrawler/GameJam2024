using Core;
using UnityEngine;

namespace MiniGame2
{
    public class PoseGame : MiniGameCore
    {
        //Hay que programar este minjuego!
        public override void EndGame()
        {
            if (GameHasEnded) return;
            Debug.Log("Mini-game not implemented!");
            base.EndGame();
        }
    }
}
