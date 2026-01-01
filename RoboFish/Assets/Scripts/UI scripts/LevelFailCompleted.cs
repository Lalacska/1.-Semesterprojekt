using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelFailCompleted : MonoBehaviour
{
    
    public void Next()
    {
        int nextScene = SceneManager.GetActiveScene().buildIndex + 1;
        Time.timeScale = 1;
        SceneManager.LoadScene(nextScene);
        Time.timeScale = 1;
    }
}
