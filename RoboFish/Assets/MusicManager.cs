using UnityEngine;
using UnityEngine.SceneManagement;
using PauseMenu;

namespace MusicManager
{
    public class MusicManager : MonoBehaviour
    {
        

        public AudioSource drumSource;
        bool isPaused;

        private void Awake()
        {
            MusicManager[] managers =
            Object.FindObjectsByType<MusicManager>(
                FindObjectsInactive.Include,
                FindObjectsSortMode.None
            );

            if (managers.Length > 3)
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);
        }
     


        private void Update()
        {
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                drumSource.volume = 0f;
                isPaused = true;
                return;
            }
            else
            {
                drumSource.volume = 1f;
                isPaused = false;
            }

            if (Time.timeScale == 0)
            {
                isPaused = true;
            }
            if (Time.timeScale == 1)
            {
                isPaused = false;
            }

            if (isPaused == true)
            {
                drumSource.volume = 0f;
            }
           if (isPaused == false)
            {
                drumSource.volume = 1f;
            }

        }
    }
}
