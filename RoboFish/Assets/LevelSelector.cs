using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    public void LevelOne()
    {
        int nextScene = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextScene);
    }

    public void LevelTwo()
    {
        int nextScene = SceneManager.GetActiveScene().buildIndex + 2;
        SceneManager.LoadScene(nextScene);
    }


}
