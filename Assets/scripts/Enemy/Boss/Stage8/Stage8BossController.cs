using UnityEngine;

public class Stage8CoreController : MonoBehaviour
{
    [SerializeField] private EnemyHealth coreHealth;
    [SerializeField] private GameObject barrier2;
    [SerializeField] private GameObject barrier3;

    private bool barrier2Shown;
    private bool barrier3Shown;

    private void Update()
    {
        if (coreHealth == null)
            return;

        if (!barrier2Shown &&
            coreHealth.CurrentHP <= coreHealth.MaxHP * 2f / 3f)
        {
            barrier2.SetActive(true);
            barrier2Shown = true;
        }

        if (!barrier3Shown &&
            coreHealth.CurrentHP <= coreHealth.MaxHP / 3f)
        {
            barrier3.SetActive(true);
            barrier3Shown = true;
        }
    }
}