using System;
using UnityEngine;

namespace Assets
{
    public class MainMenu : MonoBehaviour
    {
        public static event Action PlayGame;
        public static event Action MainMenuOpened;

        /// <summary>
        /// Disable all main menu canvas objects and invokes PlayGame event
        /// </summary>
        public void Play()
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }

            OnPlayGame();
        }

        /// <summary>
        /// Enabled all main menu canvas object and invokes MainMenuOpened event
        /// </summary>
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

        /// <summary>
        /// Invoke PlayGame event
        /// </summary>
        private static void OnPlayGame()
        {
            PlayGame?.Invoke();
        }

        /// <summary>
        /// Invoke MainMenuOpened event
        /// </summary>
        private static void OnMainMenuOpened()
        {
            MainMenuOpened?.Invoke();
        }
    }
}
