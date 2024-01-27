using Core;
using UnityEngine;

namespace Menu
{
    public class MainMenuScript : MonoBehaviour
    {
        public void StartGame()
        {
            GameManager.Instance.startGame = true;
        }

        public void ShowCredits()
        {
            
        }
    }
}
