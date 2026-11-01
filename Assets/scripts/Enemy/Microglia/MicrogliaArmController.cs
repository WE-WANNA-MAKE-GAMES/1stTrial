using UnityEngine;

public class MicrogliaArmController : MonoBehaviour
{
    [SerializeField]
    private float triggerDistance = 3f;

    [SerializeField]
    private float armExtendDistance = 1.4f;

    [SerializeField]
    private float armWidth = 0.08f;

    [SerializeField]
    private float extendSpeed = 10f;

    [SerializeField]
    private float retractSpeed = 14f;

    private Transform player;
    private Transform armPivot;
    private LineRenderer armRenderer;
    private float currentLength;

    private void Awake()
    {
        BuildArm();
    }

    private void Update()
    {
        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                player = playerObject.transform;
            }
            else
            {
                return;
            }
        }

        Vector2 toPlayer = player.position - transform.position;
        float distance = toPlayer.magnitude;
        float targetLength = distance <= triggerDistance ? armExtendDistance : 0f;

        float speed = targetLength > currentLength ? extendSpeed : retractSpeed;
        currentLength = Mathf.MoveTowards(currentLength, targetLength, speed * Time.deltaTime);

        if (armPivot != null)
        {
            armPivot.right = toPlayer.normalized;
        }

        if (armRenderer != null)
        {
            armRenderer.SetPosition(0, Vector3.zero);
            armRenderer.SetPosition(1, new Vector3(currentLength, 0f, 0f));
            armRenderer.startWidth = armWidth;
            armRenderer.endWidth = armWidth;
        }
    }

    private void BuildArm()
    {
        GameObject pivotObject = new("MicrogliaArmPivot");
        pivotObject.transform.SetParent(transform, false);
        armPivot = pivotObject.transform;

        armRenderer = pivotObject.AddComponent<LineRenderer>();
        armRenderer.useWorldSpace = false;
        armRenderer.material = new Material(Shader.Find("Sprites/Default"));
        armRenderer.startColor = Color.white;
        armRenderer.endColor = Color.white;
        armRenderer.positionCount = 2;
        armRenderer.startWidth = armWidth;
        armRenderer.endWidth = armWidth;
    }
}
