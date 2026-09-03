using UnityEngine;
using UnityEngine.SceneManagement;

namespace Manager
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        [SerializeField] private GameObject gameOverPanel;
        [SerializeField] private GameObject stageClearPanel;
        [SerializeField] private GameObject gameClearPanel;
        [SerializeField] private int totalStages = 3;

        private void Start()
        {
            gameOverPanel.SetActive(false);
            stageClearPanel.SetActive(false);
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

        //*=================================
        //* 現在のステージ番号を公開するプロパティ
        //*=================================
        public int CurrentStage
        {
            get
            {
                string sceneName = SceneManager.GetActiveScene().name;

                string stageNumberText = sceneName.Replace("Stage", "");

                if (int.TryParse(stageNumberText, out int stageNumber))
                {
                    return stageNumber;
                }

                Debug.LogError($"ステージ番号を取得できませんでした: {sceneName}");

                return 0;
            }
        }

        //*==========================
        //* 総ステージ数を公開するプロパティ
        //*==========================
        public int TotalStages => totalStages;

        //*==========================
        //* ゲームオーバー画面表示
        //*==========================
        public void GameOver()
        {
            Debug.Log("Game Over");
            Time.timeScale = 0f;
            gameOverPanel.SetActive(true);
        }

        //*==========================
        //* ステージクリア画面表示
        //*==========================
        public void StageClear()
        {
            Debug.Log("Stage Clear");
            Time.timeScale = 0f;
            stageClearPanel.SetActive(true);
        }

        //*==========================
        //* ゲームクリア画面表示
        //*==========================
        public void GameClear()
        {
            Debug.Log("Game Clear");
            Time.timeScale = 0f;
            gameClearPanel.SetActive(true);
        }

        //*==========================
        //* 次のステージに進む処理
        //*==========================
        public void NextStage()
        {
            string currentSceneName = SceneManager.GetActiveScene().name;

            int currentStage = GetStageNumber(currentSceneName);

            Debug.Log($"現在のステージ: {currentStage}");

            if (currentStage >= totalStages)
            {
                GameClear();
                return;
            }

            Time.timeScale = 1f;

            int nextStage = currentStage + 1;
            string nextSceneName = $"Stage{nextStage}";

            Debug.Log($"次のステージへ: {nextSceneName}");

            SceneManager.LoadScene(nextSceneName);
        }

        //*==========================
        //* ステージ番号を取得する処理
        //*==========================
        private int GetStageNumber(string sceneName)
        {
            string stageNumberText = sceneName.Replace("Stage", "");

            if (int.TryParse(stageNumberText, out int stageNumber))
            {
                return stageNumber;
            }

            Debug.LogError($"ステージ番号を取得できませんでした: {sceneName}");

            return 0;
        }

        //*==========================
        //* ゲームオーバー後のリトライ処理
        //*==========================
        public void Retry()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("Stage2"); //!Stage2に戻る
        }

        //*=======================================
        //* ゲームオーバー後のタイトル画面に戻る処理
        //*=======================================
        public void BackToTitle()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("Title"); //Titleに戻る
        }
    }
}