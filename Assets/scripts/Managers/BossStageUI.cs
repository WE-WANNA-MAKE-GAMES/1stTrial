using UnityEngine;
using UnityEngine.SceneManagement;

public class BossStageUI : MonoBehaviour
{
    private const float SurvivalTimeFallback = 30f;
    private const float WarningDuration = 2.5f;
    private const float BossWarningDistance = 12f;

    private EnemyHealth stage2BossHealth;
    private Stage3Boss stage3Boss;
    private Texture2D panelTexture;
    private Texture2D accentTexture;
    private float warningStartedAt;
    private bool warningStarted;
    private bool bossSpawned;
    private bool bossHidden;

    private void Awake()
    {
        panelTexture = CreateTexture(new Color(0.03f, 0.05f, 0.08f, 0.92f));
        accentTexture = CreateTexture(new Color(0.95f, 0.2f, 0.18f, 1f));
    }

    private void Update()
    {
        if (GetStageNumber() == 2 && stage2BossHealth == null)
        {
            EnemyHealth[] enemies = FindObjectsByType<EnemyHealth>();
            foreach (EnemyHealth enemy in enemies)
            {
                if (enemy.gameObject.name.Contains("Boss"))
                {
                    stage2BossHealth = enemy;
                    break;
                }
            }
        }

        if (GetStageNumber() == 3 && stage3Boss == null)
        {
            stage3Boss = FindAnyObjectByType<Stage3Boss>();
        }

        if (!bossHidden && (stage2BossHealth != null || stage3Boss != null))
        {
            SetBossActive(false);
            bossHidden = true;
        }

        if (!warningStarted && IsWithinWarningDistance())
        {
            warningStarted = true;
            warningStartedAt = Time.unscaledTime;
        }

        if (warningStarted && !bossSpawned &&
            Time.unscaledTime - warningStartedAt >= WarningDuration)
        {
            bossSpawned = true;
            SetBossActive(true);
        }
    }

    private void OnGUI()
    {
        int stage = GetStageNumber();
        if (stage != 2 && stage != 3)
        {
            return;
        }

        if (!warningStarted)
        {
            return;
        }

        bool warningActive = warningStarted && !bossSpawned;
        bool bossExists = stage == 2
            ? stage2BossHealth != null && stage2BossHealth.gameObject.activeInHierarchy
            : stage3Boss != null && stage3Boss.gameObject.activeInHierarchy;

        if (!bossExists && !warningActive)
        {
            return;
        }

        bool encounterStarted = !warningActive && (stage == 2
            ? IsOnScreen(stage2BossHealth.transform)
            : stage3Boss.HasStarted);

        DrawCondition(stage);

        if (!encounterStarted)
        {
            DrawWarning();
            return;
        }

        if (stage == 2)
        {
            DrawBossHealth();
        }
        else
        {
            DrawSurvivalTimer();
        }
    }

    private void DrawCondition(int stage)
    {
        string condition = stage == 2
            ? "CLEAR CONDITION\nDEFEAT THE BOSS"
            : "CLEAR CONDITION\nSURVIVE FOR 30 SECONDS";

        GUIStyle style = new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.MiddleCenter,
            fontSize = Mathf.Max(14, Screen.height / 48),
            fontStyle = FontStyle.Bold,
            normal = { textColor = Color.white }
        };

        Rect area = new Rect(Screen.width * 0.5f - 180f, 22f, 360f, 58f);
        GUI.Label(area, condition, style);
    }

    private void DrawWarning()
    {
        if (Mathf.FloorToInt(Time.unscaledTime * 3f) % 2 == 0)
        {
            return;
        }

        GUIStyle style = new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.MiddleCenter,
            fontSize = Mathf.Max(24, Screen.height / 18),
            fontStyle = FontStyle.Bold,
            normal = { textColor = new Color(1f, 0.2f, 0.15f) }
        };

        Rect area = new Rect(Screen.width * 0.5f - 250f, Screen.height * 0.34f, 500f, 70f);
        GUI.Label(area, "WARNING!\nBOSS APPROACHING", style);
    }

    private void DrawBossHealth()
    {
        float ratio = stage2BossHealth.MaxHP > 0f
            ? stage2BossHealth.CurrentHP / stage2BossHealth.MaxHP
            : 0f;

        DrawBottomPanel("BOSS HP", ratio, $"{Mathf.CeilToInt(stage2BossHealth.CurrentHP)} / {Mathf.CeilToInt(stage2BossHealth.MaxHP)}");
    }

    private void DrawSurvivalTimer()
    {
        float survivalTime = stage3Boss.SurvivalTime > 0f
            ? stage3Boss.SurvivalTime
            : SurvivalTimeFallback;
        float ratio = stage3Boss.RemainingTime / survivalTime;
        string time = $"{stage3Boss.RemainingTime:0.0}s";

        DrawBottomPanel("SURVIVE", ratio, time);
    }

    private void DrawBottomPanel(string title, float ratio, string value)
    {
        float width = Mathf.Min(Screen.width - 48f, 760f);
        float left = (Screen.width - width) * 0.5f;
        float top = Screen.height - 82f;
        Rect panel = new Rect(left, top, width, 54f);
        Rect bar = new Rect(left + 16f, top + 27f, width - 32f, 14f);

        GUI.DrawTexture(panel, panelTexture);
        GUI.DrawTexture(bar, panelTexture);
        GUI.DrawTexture(new Rect(bar.x, bar.y, bar.width * Mathf.Clamp01(ratio), bar.height), accentTexture);

        GUIStyle labelStyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = Mathf.Max(13, Screen.height / 60),
            fontStyle = FontStyle.Bold,
            normal = { textColor = Color.white }
        };

        GUI.Label(new Rect(left + 16f, top + 4f, width * 0.5f, 22f), title, labelStyle);
        labelStyle.alignment = TextAnchor.UpperRight;
        GUI.Label(new Rect(left + width * 0.5f, top + 4f, width - 16f, 22f), value, labelStyle);
    }

    private int GetStageNumber()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        string stageNumberText = sceneName.Replace("Stage", "");
        return int.TryParse(stageNumberText, out int stageNumber) ? stageNumber : 0;
    }

    private bool IsOnScreen(Transform target)
    {
        if (Camera.main == null || target == null)
        {
            return false;
        }

        Vector3 position = Camera.main.WorldToViewportPoint(target.position);
        return position.z > 0f &&
            position.x >= 0f && position.x <= 1f &&
            position.y >= 0f && position.y <= 1f;
    }

    private bool IsWithinWarningDistance()
    {
        Transform bossTransform = stage2BossHealth != null
            ? stage2BossHealth.transform
            : stage3Boss != null ? stage3Boss.transform : null;

        if (Camera.main == null || bossTransform == null)
        {
            return false;
        }

        float distanceToBoss = bossTransform.position.x - Camera.main.transform.position.x;
        return distanceToBoss <= BossWarningDistance;
    }

    private void SetBossActive(bool isActive)
    {
        if (stage2BossHealth != null)
        {
            stage2BossHealth.gameObject.SetActive(isActive);
        }

        if (stage3Boss != null)
        {
            stage3Boss.gameObject.SetActive(isActive);
        }
    }

    private Texture2D CreateTexture(Color color)
    {
        Texture2D texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, color);
        texture.Apply();
        return texture;
    }

    private void OnDestroy()
    {
        if (panelTexture != null)
        {
            Destroy(panelTexture);
        }

        if (accentTexture != null)
        {
            Destroy(accentTexture);
        }
    }
}
