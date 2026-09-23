using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Manager;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;

    [SerializeField] private float fireInterval = 0.2f;
    private Transform scrollRoot;
    private float timer = 0f;

    private PlayerControls controls;
    private PlayerPiercing playerPiercing;
    private PlayerSpreadShot playerSpreadShot;
    private readonly List<BulletMode> availableModes = new();
    private BulletMode currentMode = BulletMode.Normal;
    private bool piercingAvailable;
    private bool spreadShotAvailable;
    private TextMeshPro modeLabel;
    private Coroutine modeLabelCoroutine;

    private enum BulletMode
    {
        Normal,
        Piercing,
        SpreadShot
    }

    private void Start()
    {
        // If the GameManager is in debug mode, set the fire interval to the debug value
        if (GameManager.Instance != null && GameManager.Instance.IsDebugMode())
        {
            fireInterval = GameManager.Instance.DebugModeShootInterval;
        }
    }

    private void Awake()
    {
        controls = new PlayerControls();
        GameObject scrollRootObject = GameObject.FindGameObjectWithTag("ScrollRoot");
        if (scrollRootObject != null)
        {
            scrollRoot = scrollRootObject.transform;
        }

        playerPiercing = GetComponent<PlayerPiercing>();
        playerSpreadShot = GetComponent<PlayerSpreadShot>();
        RebuildAvailableModes();
        CreateModeLabel();
    }

   private void OnEnable()
{
    if (controls == null)
    {
        controls = new PlayerControls();
    }

    controls.Enable();
}

private void OnDisable()
{
    controls?.Disable();
}

    private void OnDestroy()
    {
        controls.Dispose();
    }

    private void Update()
    {
        if (controls == null) return;
        timer += Time.deltaTime;

        RefreshAvailableModes();

        if (Keyboard.current != null && Keyboard.current.kKey.wasPressedThisFrame)
        {
            CycleBulletMode();
        }

        if (controls.Player.Shoot.IsPressed() &&
            timer >= fireInterval)
        {
            timer = 0f;

            if (currentMode == BulletMode.SpreadShot)
            {
                ShootAtAngles(-15f, 0f, 15f);
            }
            else
            {
                ShootAtAngles(0f);
            }
        }
    }

    private void ShootAtAngles(params float[] angles)
    {
        foreach (float angle in angles)
        {
            Quaternion rotation = Quaternion.Euler(0f, 0f, angle);

            GameObject bulletObject = Instantiate(
                bulletPrefab,
                firePoint.position,
                rotation,
                scrollRoot
            );

            PlayerBullet bullet = bulletObject.GetComponent<PlayerBullet>();

            if (bullet != null)
            {
                int penetration = currentMode == BulletMode.Piercing && playerPiercing != null
                    ? playerPiercing.AdditionalPenetrations
                    : 0;

                bullet.Initialize(penetration);
            }
        }
    }

    private void RefreshAvailableModes()
    {
        bool hasPiercing = playerPiercing != null && playerPiercing.AdditionalPenetrations > 0;
        bool hasSpreadShot = playerSpreadShot != null && playerSpreadShot.IsActive;

        if (hasPiercing != piercingAvailable || hasSpreadShot != spreadShotAvailable)
        {
            piercingAvailable = hasPiercing;
            spreadShotAvailable = hasSpreadShot;
            RebuildAvailableModes();
        }
    }

    private void RebuildAvailableModes()
    {
        availableModes.Clear();
        availableModes.Add(BulletMode.Normal);

        if (piercingAvailable)
        {
            availableModes.Add(BulletMode.Piercing);
        }

        if (spreadShotAvailable)
        {
            availableModes.Add(BulletMode.SpreadShot);
        }

        if (!availableModes.Contains(currentMode))
        {
            currentMode = BulletMode.Normal;
        }
    }

    private void CycleBulletMode()
    {
        if (availableModes.Count <= 1)
        {
            return;
        }

        int currentIndex = availableModes.IndexOf(currentMode);
        currentMode = availableModes[(currentIndex + 1) % availableModes.Count];
        ShowCurrentMode();
    }

    private void CreateModeLabel()
    {
        GameObject labelObject = new("BulletModeLabel");
        labelObject.transform.SetParent(transform, false);
        labelObject.transform.localPosition = new Vector3(0f, -0.75f, 0f);
        labelObject.transform.localScale = Vector3.one * 0.1f;

        modeLabel = labelObject.AddComponent<TextMeshPro>();
        modeLabel.alignment = TextAlignmentOptions.Center;
        modeLabel.fontSize = 2.5f;
        modeLabel.rectTransform.sizeDelta = new Vector2(8f, 2f);
        modeLabel.color = Color.white;
        modeLabel.sortingOrder = 10;
        labelObject.SetActive(false);
    }

    private void ShowCurrentMode()
    {
        if (modeLabel == null)
        {
            return;
        }

        modeLabel.text = GetModeDisplayName();
        modeLabel.gameObject.SetActive(true);

        if (modeLabelCoroutine != null)
        {
            StopCoroutine(modeLabelCoroutine);
        }

        modeLabelCoroutine = StartCoroutine(HideModeLabel());
    }

    private string GetModeDisplayName()
    {
        return currentMode switch
        {
            BulletMode.Piercing => "貫通弾",
            BulletMode.SpreadShot => "拡散弾",
            _ => "通常弾"
        };
    }

    private IEnumerator HideModeLabel()
    {
        yield return new WaitForSecondsRealtime(1.2f);
        modeLabel.gameObject.SetActive(false);
        modeLabelCoroutine = null;
    }
}

