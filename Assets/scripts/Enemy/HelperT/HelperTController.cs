using System.Collections;
using UnityEngine;

public class HelperTController : MonoBehaviour
{
    [System.Serializable]
    private class ReinforcementEntry
    {
        public GameObject enemyPrefab;
        [Min(0)] public int spawnCount = 1;
    }

    [Header("Reinforcements")]
    [SerializeField] private ReinforcementEntry[] reinforcements;
    [SerializeField] private float alarmSpawnInterval = 3f;
    [SerializeField] private int maxAlarmWaves;
    [SerializeField] private Transform scrollRoot;
    [SerializeField] private float spawnRadius = 1.5f;

    [Header("Warning Fan")]
    [SerializeField] private float warningDistance = 8f;
    [Range(1f, 180f)]
    [SerializeField] private float warningAngle = 75f;
    [SerializeField] private float forwardAngle = 180f;
    [SerializeField] private Color idleFanColor = new(1f, 0.75f, 0.1f, 0.2f);
    [SerializeField] private Color alarmFanColor = new(1f, 0.1f, 0.1f, 0.35f);

    private Transform player;
    private Mesh fanMesh;
    private MeshRenderer fanRenderer;
    private Coroutine reinforcementRoutine;
    private bool alarmActive;
    private int alarmWaveCount;

    public bool IsAlarmActive => alarmActive;

    private void Awake()
    {
        EnsureEnemyComponents();

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }

        if (scrollRoot == null)
        {
            GameObject scrollRootObject = GameObject.FindGameObjectWithTag("ScrollRoot");
            if (scrollRootObject != null)
            {
                scrollRoot = scrollRootObject.transform;
            }
        }

        CreateWarningFan();
    }

    private void EnsureEnemyComponents()
    {
        EnsureComponent<Rigidbody2D>();
        EnsureComponent<CircleCollider2D>();
        EnsureComponent<EnemyAttack>();
        EnsureComponent<EnemyEffect>();
        EnsureComponent<EnemyHealth>();
        EnsureComponent<EnemyKnockback>();
        EnsureComponent<EnemyMovement>();

        GameObject damageArea = new("DamageArea");
        damageArea.transform.SetParent(transform, false);
        damageArea.tag = "EnemyDamageArea";

        CircleCollider2D damageCollider = damageArea.AddComponent<CircleCollider2D>();
        damageCollider.isTrigger = true;
    }

    private void EnsureComponent<T>() where T : Component
    {
        if (GetComponent<T>() == null)
        {
            gameObject.AddComponent<T>();
        }
    }

    private void Update()
    {
        if (player == null)
        {
            return;
        }

        bool playerInFan = IsPlayerInWarningFan();
        if (playerInFan && !alarmActive)
        {
            StartAlarm();
        }
    }

    private bool IsPlayerInWarningFan()
    {
        Vector2 toPlayer = player.position - transform.position;
        if (toPlayer.sqrMagnitude > warningDistance * warningDistance)
        {
            return false;
        }

        float angle = Mathf.Abs(Mathf.DeltaAngle(
            forwardAngle,
            Mathf.Atan2(toPlayer.y, toPlayer.x) * Mathf.Rad2Deg
        ));

        return angle <= warningAngle * 0.5f;
    }

    private void StartAlarm()
    {
        alarmActive = true;
        alarmWaveCount = 0;
        SetFanColor(alarmFanColor);

        if (reinforcementRoutine == null)
        {
            reinforcementRoutine = StartCoroutine(SpawnReinforcements());
        }
    }

    private IEnumerator SpawnReinforcements()
    {
        while (alarmActive && (maxAlarmWaves <= 0 || alarmWaveCount < maxAlarmWaves))
        {
            SpawnWave();
            alarmWaveCount++;
            yield return new WaitForSeconds(alarmSpawnInterval);
        }

        reinforcementRoutine = null;
    }

    private void SpawnWave()
    {
        if (reinforcements == null)
        {
            return;
        }

        foreach (ReinforcementEntry entry in reinforcements)
        {
            if (entry == null || entry.enemyPrefab == null)
            {
                continue;
            }

            for (int i = 0; i < entry.spawnCount; i++)
            {
                Vector2 offset = Random.insideUnitCircle * spawnRadius;
                Instantiate(
                    entry.enemyPrefab,
                    transform.position + (Vector3)offset,
                    Quaternion.identity,
                    scrollRoot
                );
            }
        }
    }

    private void CreateWarningFan()
    {
        GameObject fanObject = new("WarningFan");
        fanObject.transform.SetParent(transform, false);

        fanMesh = new Mesh { name = "HelperTWarningFanMesh" };
        MeshFilter meshFilter = fanObject.AddComponent<MeshFilter>();
        fanRenderer = fanObject.AddComponent<MeshRenderer>();
        meshFilter.sharedMesh = fanMesh;
        fanRenderer.material = CreateFanMaterial(idleFanColor);
        fanRenderer.sortingOrder = -1;

        BuildWarningFanMesh();
    }

    private Material CreateFanMaterial(Color color)
    {
        Material material = new(Shader.Find("Sprites/Default"));
        material.color = color;
        return material;
    }

    private void BuildWarningFanMesh()
    {
        int segmentCount = 24;
        Vector3[] vertices = new Vector3[segmentCount + 2];
        int[] triangles = new int[segmentCount * 3];
        vertices[0] = Vector3.zero;

        float startAngle = forwardAngle - warningAngle * 0.5f;
        for (int i = 0; i <= segmentCount; i++)
        {
            float angle = (startAngle + warningAngle * i / segmentCount) * Mathf.Deg2Rad;
            vertices[i + 1] = new Vector3(
                Mathf.Cos(angle) * warningDistance,
                Mathf.Sin(angle) * warningDistance,
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

        fanMesh.Clear();
        fanMesh.vertices = vertices;
        fanMesh.triangles = triangles;
        fanMesh.RecalculateBounds();
    }

    private void SetFanColor(Color color)
    {
        if (fanRenderer != null)
        {
            fanRenderer.material.color = color;
        }
    }

    private void OnDestroy()
    {
        if (fanMesh != null)
        {
            Destroy(fanMesh);
        }
    }
}
