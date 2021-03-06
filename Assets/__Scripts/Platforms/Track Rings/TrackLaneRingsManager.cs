﻿using UnityEngine;

public class TrackLaneRingsManager : MonoBehaviour
{
    public TrackLaneRing[] rings { get; private set; }

    public int ringCount = 10;
    public TrackLaneRing prefab;
    public float minPositionStep = 1;
    public float maxPositionStep = 2;
    public float moveSpeed = 1;
    [Header("Rotation")]
    public float rotationStep = 5;
    public int propagationSpeed = 1;
    public float flexySpeed = 1;
    public TrackLaneRingsRotationEffect rotationEffect;

    private bool zoomed = false;

    public void Start()
    {
        prefab.gameObject.SetActive(false);
        rings = new TrackLaneRing[ringCount];
        for (int i = 0; i < rings.Length; i++)
        {
            rings[i] = Instantiate(prefab, transform);
            rings[i].gameObject.SetActive(true);
            rings[i].gameObject.name = $"Ring {i}";
            Vector3 pos = new Vector3(0, 0, i * maxPositionStep);
            rings[i].Init(pos, Vector3.zero);
        }
    }

    public void HandlePositionEvent()
    {
        float step = zoomed ? maxPositionStep : minPositionStep;
        zoomed = !zoomed;
        for (int i = 0; i < rings.Length; i++)
        {
            float destPosZ = i * step;
            rings[i].SetPosition(destPosZ, moveSpeed);
        }
    }

    public void HandleRotationEvent(SimpleJSON.JSONNode customData = null)
    {
        rotationEffect.AddRingRotationEvent(rings[0].GetDestinationRotation(),
            Random.Range(0, rotationStep), propagationSpeed, flexySpeed, customData);
    }

    private void FixedUpdate()
    {
        foreach (TrackLaneRing ring in rings) ring.FixedUpdateRing(TimeHelper.FixedDeltaTime);
    }

    private void LateUpdate()
    {
        foreach (TrackLaneRing ring in rings) ring.LateUpdateRing(TimeHelper.InterpolationFactor);
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 forward = base.transform.forward;
        Vector3 position = base.transform.position;
        float d = 0.5f;
        float num = 45f;
        Gizmos.DrawRay(position, forward);
        Vector3 a = Quaternion.LookRotation(forward) * Quaternion.Euler(0f, 180f + num, 0f) * new Vector3(0f, 0f, 1f);
        Vector3 a2 = Quaternion.LookRotation(forward) * Quaternion.Euler(0f, 180f - num, 0f) * new Vector3(0f, 0f, 1f);
        Gizmos.DrawRay(position + forward, a * d);
        Gizmos.DrawRay(position + forward, a2 * d);
    }
}
