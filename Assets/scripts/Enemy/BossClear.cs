using System.Collections;
using UnityEngine;

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

        GameManager.Instance.GameClear();
    }
}