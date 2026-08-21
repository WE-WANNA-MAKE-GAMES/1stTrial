using UnityEngine;

public class Goal : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        // Boss が残っている間はクリアさせない
        if (boss != null)
            return;

        GameManager.Instance.GameClear();
    }
}