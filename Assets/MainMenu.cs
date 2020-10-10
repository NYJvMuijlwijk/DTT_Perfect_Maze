using System;
using UnityEngine;

namespace Assets
{
    public class MainMenu : MonoBehaviour
    {
        public static event Action PlayGame;
        public static event Action MainMenuOpened;

        public void Play()
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }

            OnPlayGame();
        }

        public void OpenMainMenu()
        {
            foreach (Transform child in GetComponentsInChildren<Transform>(true))
            {
                child.gameObject.SetActive(true);
            }

            OnMainMenuOpened();
        }

        /// <summary>
        /// Exit the program
        /// </summary>
        public void Quit()
        {
            Application.Quit();
        }

        private static void OnPlayGame()
        {
            PlayGame?.Invoke();
        }

        private static void OnMainMenuOpened()
        {
            MainMenuOpened?.Invoke();
        }
    }
}
