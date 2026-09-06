using UnityEngine;
using Manager;

public class Stage3Boss : MonoBehaviour
{
    [SerializeField] private float survivalTime = 5f;

    private float timer = 0f;
    private bool isCleared = false;

    private void Update()
    {
        if (isCleared)
        {
            return;
        }

        if (!IsOnScreen())
        {
            timer = 0f;
            return;
        }

        timer += Time.deltaTime;

        if (timer >= survivalTime)
        {
            ClearBoss();
        }
    }

    private void ClearBoss()
    {
        if (isCleared)
        {
            return;
        }

        isCleared = true;

        Debug.Log("Stage3 Boss Clear");

        if (GameManager.Instance.CurrentStage >=
            GameManager.Instance.TotalStages)
        {
            GameManager.Instance.GameClear();
        }
        else
        {
            GameManager.Instance.StageClear();
        }
    }
    private bool IsOnScreen()
    {
        Vector3 position = Camera.main.WorldToViewportPoint(transform.position);

        return position.z > 0f &&
            position.x >= 0f && position.x <= 1f &&
            position.y >= 0f && position.y <= 1f;
    }
}