using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class NeuronConnection : MonoBehaviour
{
    [SerializeField]
    private Color lineColor = new(0.4f, 0.9f, 1f, 0.9f);

    [SerializeField]
    private float lineWidth = 0.08f;

    [SerializeField]
    private Color chargeColor = new(0.35f, 0.9f, 1f, 1f);

    [SerializeField]
    private Color dischargeColor = new(0.85f, 1f, 1f, 1f);

    private Neuron owner;
    private LineRenderer lineRenderer;
    private readonly List<EdgeCollider2D> connectionColliders = new();
    private readonly List<LineRenderer> electricRenderers = new();
    private float chargeAmount;
    private bool discharging;

    private void Awake()
    {
        owner = GetComponent<Neuron>();
        lineRenderer = GetComponent<LineRenderer>();
        ApplyRendererSettings();
    }

    private void Update()
    {
        if (owner == null)
        {
            owner = GetComponent<Neuron>();
        }

        if (lineRenderer == null || owner == null)
        {
            return;
        }

        owner.RemoveDestroyedConnections();

        var connections = owner.ConnectedNeurons;
        if (connections == null || connections.Count == 0)
        {
            lineRenderer.positionCount = 0;
            DisableUnusedConnectionColliders(0);
            DisableUnusedElectricRenderers(0);
            return;
        }

        int totalPoints = 0;
        for (int i = 0; i < connections.Count; i++)
        {
            if (connections[i] != null)
            {
                totalPoints += 2;
            }
        }

        if (totalPoints == 0)
        {
            lineRenderer.positionCount = 0;
            DisableUnusedConnectionColliders(0);
            DisableUnusedElectricRenderers(0);
            return;
        }

        lineRenderer.positionCount = totalPoints;

        int index = 0;
        int colliderIndex = 0;
        foreach (Neuron neuron in connections)
        {
            if (neuron == null)
            {
                continue;
            }

            lineRenderer.SetPosition(index, transform.position);
            lineRenderer.SetPosition(index + 1, neuron.transform.position);
            index += 2;

            EdgeCollider2D edgeCollider = GetOrCreateConnectionCollider(colliderIndex);
            edgeCollider.points = new[]
            {
                (Vector2)transform.InverseTransformPoint(transform.position),
                (Vector2)transform.InverseTransformPoint(neuron.transform.position)
            };

            UpdateElectricRenderer(colliderIndex, neuron);
            colliderIndex++;
        }

        DisableUnusedConnectionColliders(colliderIndex);
        DisableUnusedElectricRenderers(colliderIndex);
    }

    public void SetElectricEffect(float charge, bool isDischarging)
    {
        chargeAmount = Mathf.Clamp01(charge);
        discharging = isDischarging;
    }

    private EdgeCollider2D GetOrCreateConnectionCollider(int index)
    {
        while (connectionColliders.Count <= index)
        {
            EdgeCollider2D edgeCollider = gameObject.AddComponent<EdgeCollider2D>();
            edgeCollider.isTrigger = true;
            connectionColliders.Add(edgeCollider);
        }

        EdgeCollider2D result = connectionColliders[index];
        result.edgeRadius = 0.1f;
        result.enabled = true;
        return result;
    }

    private void DisableUnusedConnectionColliders(int usedCount)
    {
        for (int i = usedCount; i < connectionColliders.Count; i++)
        {
            connectionColliders[i].enabled = false;
        }
    }

    private void UpdateElectricRenderer(int index, Neuron connectedNeuron)
    {
        LineRenderer effectRenderer = GetOrCreateElectricRenderer(index);
        if (!discharging && chargeAmount <= 0f)
        {
            effectRenderer.enabled = false;
            return;
        }

        Vector3 start = transform.position;
        Vector3 end = connectedNeuron.transform.position;
        Color effectColor = discharging ? dischargeColor : chargeColor;
        float strength = discharging
            ? 1f
            : chargeAmount * (0.75f + 0.25f * Mathf.Sin(Time.time * 18f));
        effectColor.a *= strength;
        effectRenderer.startColor = effectColor;
        effectRenderer.endColor = effectColor;
        effectRenderer.startWidth = lineWidth * (discharging ? 1.8f : 0.8f + strength);
        effectRenderer.endWidth = effectRenderer.startWidth;

        if (discharging)
        {
            const int pointCount = 9;
            Vector3 direction = end - start;
            Vector3 perpendicular = new(-direction.y, direction.x, 0f);
            perpendicular = perpendicular.sqrMagnitude > 0f ? perpendicular.normalized : Vector3.up;
            Vector3[] points = new Vector3[pointCount];

            for (int i = 0; i < pointCount; i++)
            {
                float t = (float)i / (pointCount - 1);
                float jitter = i == 0 || i == pointCount - 1
                    ? 0f
                    : Mathf.Sin(Time.time * 95f + i * 2.7f + index) * 0.12f;
                points[i] = Vector3.Lerp(start, end, t) + perpendicular * jitter;
            }

            effectRenderer.positionCount = pointCount;
            effectRenderer.SetPositions(points);
        }
        else
        {
            effectRenderer.positionCount = 2;
            effectRenderer.SetPosition(0, start);
            effectRenderer.SetPosition(1, end);
        }

        effectRenderer.enabled = true;
    }

    private LineRenderer GetOrCreateElectricRenderer(int index)
    {
        while (electricRenderers.Count <= index)
        {
            GameObject effectObject = new($"ElectricArc_{electricRenderers.Count}");
            effectObject.transform.SetParent(transform, false);
            LineRenderer effectRenderer = effectObject.AddComponent<LineRenderer>();
            effectRenderer.useWorldSpace = true;
            effectRenderer.loop = false;
            effectRenderer.material = new Material(Shader.Find("Sprites/Default"));
            effectRenderer.sortingLayerID = lineRenderer.sortingLayerID;
            effectRenderer.sortingOrder = lineRenderer.sortingOrder + 1;
            effectRenderer.enabled = false;
            electricRenderers.Add(effectRenderer);
        }

        return electricRenderers[index];
    }

    private void DisableUnusedElectricRenderers(int usedCount)
    {
        for (int i = usedCount; i < electricRenderers.Count; i++)
        {
            electricRenderers[i].enabled = false;
        }
    }

    private void ApplyRendererSettings()
    {
        if (lineRenderer == null)
        {
            return;
        }

        lineRenderer.useWorldSpace = true;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.startColor = lineColor;
        lineRenderer.endColor = lineColor;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.material.color = lineColor;
    }
}
