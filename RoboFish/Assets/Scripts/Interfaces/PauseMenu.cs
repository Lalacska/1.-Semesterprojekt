using UnityEngine;
using UnityEngine.SceneManagement;

namespace PauseMenu
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] GameObject pauseMenu;
        public bool isPaused;


        public void Pause()
        {
            if (isPaused)
            {
                pauseMenu.SetActive(false);
                isPaused = false;
            }
            else
            {
                pauseMenu.SetActive(true);
                isPaused = true;
                Time.timeScale = 0;
            }
        }

        public void Menu()
        {
            SceneManager.LoadScene("Main Menu");
            Time.timeScale = 1;
        }

        public void Resume()
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1;
        }

        public void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Time.timeScale = 1;
        }
    }
}
