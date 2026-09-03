using System.Collections;
using UnityEngine;
using GameManager = Manager.GameManager;

public class BossClear : MonoBehaviour
{
    [SerializeField] private float clearDelay = 1.5f;

    private void OnDestroy()
    {
        if (!Application.isPlaying || GameManager.Instance == null)
            return;

        GameManager.Instance.StartCoroutine(ClearAfterDelay());
    }

    private IEnumerator ClearAfterDelay()
    {
        yield return new WaitForSeconds(clearDelay);
        if (GameManager.Instance.CurrentStage >= GameManager.Instance.TotalStages)
        {
            GameManager.Instance.GameClear();
        }
        else
        {
            GameManager.Instance.StageClear();
        }
    }
}
//通知テストよう