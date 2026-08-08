using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject gameClearPanel;

    private void Start()
    {
        gameOverPanel.SetActive(false);
        gameClearPanel.SetActive(false);
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    //* ゲームオーバー処理
    public void GameOver()
    {
        Debug.Log("Game Over");
        Time.timeScale = 0f;
        gameOverPanel.SetActive(true);
    }

    //* ゲームクリア処理
    public void GameClear()
    {
        Debug.Log("Game Clear");
        Time.timeScale = 0f;
        gameClearPanel.SetActive(true);
    }
    //* ゲームオーバー後のリトライ処理
    public void Retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Scene1"); //Scene1に戻る
    }

    //* ゲームオーバー後のタイトル画面に戻る処理
    public void BackToTitle()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Title"); //Titleに戻る
    }
}