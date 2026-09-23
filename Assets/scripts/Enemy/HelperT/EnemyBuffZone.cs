using System.Collections.Generic;
using UnityEngine;

public class EnemyBuffZone : MonoBehaviour
{
    [SerializeField] private float radius = 5f;
    [Range(1f, 360f)]
    [SerializeField] private float arcAngle = 180f;
    [SerializeField] private float centerAngle = 180f;
    [SerializeField] private float speedMultiplier = 1.2f;
    [SerializeField] private Color zoneColor = new(0.2f, 0.7f, 1f, 0.2f);

    private readonly HashSet<EnemyBuffReceiver> buffedEnemies = new();
    private Mesh zoneMesh;
    private MeshRenderer zoneRenderer;

    private void Awake()
    {
        CreateZoneVisual();
    }

    private void Update()
    {
        ClearBuffs();

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (Collider2D collider in colliders)
        {
            EnemyHealth enemy = collider.GetComponentInParent<EnemyHealth>();
            if (enemy == null)
            {
                continue;
            }

            EnemyBuffReceiver receiver = enemy.GetComponent<EnemyBuffReceiver>();
            if (receiver == null)
            {
                receiver = enemy.gameObject.AddComponent<EnemyBuffReceiver>();
            }

            if (receiver == null || !IsInsideArc(receiver.transform.position))
            {
                continue;
            }

            receiver.SetSpeedMultiplier(speedMultiplier);
            buffedEnemies.Add(receiver);
        }
    }

    private bool IsInsideArc(Vector3 targetPosition)
    {
        Vector2 direction = targetPosition - transform.position;
        float angle = Mathf.Abs(Mathf.DeltaAngle(
            centerAngle,
            Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg
        ));

        return angle <= arcAngle * 0.5f;
    }

    private void ClearBuffs()
    {
        foreach (EnemyBuffReceiver receiver in buffedEnemies)
        {
            if (receiver != null)
            {
                receiver.SetSpeedMultiplier(1f);
            }
        }

        buffedEnemies.Clear();
    }

    private void CreateZoneVisual()
    {
        GameObject zoneObject = new("EnemyBuffZoneVisual");
        zoneObject.transform.SetParent(transform, false);

        zoneMesh = new Mesh { name = "EnemyBuffZoneMesh" };
        MeshFilter meshFilter = zoneObject.AddComponent<MeshFilter>();
        zoneRenderer = zoneObject.AddComponent<MeshRenderer>();
        meshFilter.sharedMesh = zoneMesh;
        zoneRenderer.material = new Material(Shader.Find("Sprites/Default"))
        {
            color = zoneColor
        };
        zoneRenderer.sortingOrder = -2;

        BuildZoneMesh();
    }

    private void BuildZoneMesh()
    {
        int segmentCount = 32;
        Vector3[] vertices = new Vector3[segmentCount + 2];
        int[] triangles = new int[segmentCount * 3];
        vertices[0] = Vector3.zero;

        float startAngle = centerAngle - arcAngle * 0.5f;
        for (int i = 0; i <= segmentCount; i++)
        {
            float angle = (startAngle + arcAngle * i / segmentCount) * Mathf.Deg2Rad;
            vertices[i + 1] = new Vector3(
                Mathf.Cos(angle) * radius,
                Mathf.Sin(angle) * radius,
                0f
            );
        }

        for (int i = 0; i < segmentCount; i++)
        {
            int triangleIndex = i * 3;
            triangles[triangleIndex] = 0;
            triangles[triangleIndex + 1] = i + 1;
            triangles[triangleIndex + 2] = i + 2;
        }

        zoneMesh.vertices = vertices;
        zoneMesh.triangles = triangles;
        zoneMesh.RecalculateBounds();
    }

    private void OnDestroy()
    {
        ClearBuffs();
        if (zoneMesh != null)
        {
            Destroy(zoneMesh);
        }
    }
}
