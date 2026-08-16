using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public void StartGame()
    {
        Time.timeScale = 1f; // Ensure the game time is running at normal speed
        //!=======================================
        //!ここはタイトル後表示したいステージの名前に変更
        //!=======================================
        SceneManager.LoadScene("Stage2");
    }
}
