using UnityEngine;
using UnityEngine.VFX;

public class BloodFlowVFXController : MonoBehaviour
{
    private VisualEffect visualEffect;

    private void Awake()
    {
        visualEffect = GetComponent<VisualEffect>();
    }

    private void Start()
    {
        visualEffect.Play();
    }
}