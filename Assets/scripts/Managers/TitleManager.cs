using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public void StartGame()
    {
        Time.timeScale = 1f; // Ensure the game time is running at normal speed
        SceneManager.LoadScene("Scene1");
    }
}
