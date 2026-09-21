using UnityEngine;

public class PlayerSpreadShot : MonoBehaviour
{
    [SerializeField] private bool isActive;

    public bool IsActive => isActive;

    public void Activate()
    {
        isActive = true;
        Debug.Log("ショットガンを有効化しました");
    }
}