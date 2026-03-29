using System;
using UnityEngine;

public class OcclusionPositionWriter : MonoBehaviour
{
    // set values from the inspector
    public Material OccluderMaterial;
    public Camera MainCamera;
    public bool ShouldInterpolate;
    public float InterpolationWithHitPoint = 0.3f;
    public float InterpolationWithPreviousPosition = 0.3f;
    public float SphereCastRadius = 0.5f;

    // assigned from GameManager
    private Transform Player;

    // when no hit - smoooth dissapearance is provided with this 
    public float FadeOutSpeed = 0.1f;
    private float CutOutFade = 1.0f;
    private float CutOutFadeSign = 1.0f;

    // only select occluder layer mask - they will have the shader needed 
    // TODO: maybe do it easier
    private static readonly int PlayerPositionID = Shader.PropertyToID("_PlayerPosition");
    private static readonly int CutOutFadeOutID = Shader.PropertyToID("_CutOutFadeOut");
    private LayerMask occluderLayerMask;

    // timer
    float TimeElapsed = 0;
    public float TimeBetweenUpdates = 0.1f;

    void Awake()
    {
        Player = GameManager.instance.player.transform;
    }

    void Start()
    {
        hits = new RaycastHit[1];

        occluderLayerMask = LayerMask.GetMask("Occluder");
        if (!MainCamera)
        {
            Debug.Log("Camera not set in OcclusionPositionWriter. Using main.");
            MainCamera = Camera.main;
        }

        if (!Player)
        {
            Debug.LogWarning("Player Transform not set in OcclusionPositionWriter.");
        }

        if (!OccluderMaterial)
        {
            Debug.LogWarning("OccluderMaterial not set in OcclusionPositionWriter.");
        }

        if (OccluderMaterial != null)
        {
            OccluderMaterial.SetVector(PlayerPositionID, GetPlayerPosition(false));
            TimeElapsed = 0;
        }

        if (Player)
        {
            // get basic cutout value and sign - 0.0 (-1) if hit is present, 1.0 (1) if not
            Vector3 from = Player.position;
            Vector3 to = MainCamera.transform.position;
            Vector3 direction = (to - from).normalized;
            float distance = Vector3.Distance(from, to);
            hitCount = Physics.SphereCastNonAlloc(Player.position, SphereCastRadius, direction, hits, distance, occluderLayerMask);
            CutOutFade = hitCount > 0 ? 0.0f : 1.0f;
            CutOutFadeSign = hitCount > 0 ? -1 : 1;
        }


    }

    private RaycastHit[] hits;
    private int hitCount = 0;
    private Vector3 prevPos;

    private void OnDrawGizmos()
    {
        if (!MainCamera)
        {
            MainCamera = Camera.main;
        }
        if (Player != null)
        {
            Vector3 pos = Player.position;
            Vector3 from = pos;
            Vector3 to = MainCamera.transform.position;
            Vector3 direction = (to - from).normalized;
            Gizmos.DrawRay(from, direction);
        }
        if (hitCount != 0) Gizmos.DrawSphere(hits[0].point, SphereCastRadius);
    }

    public Vector3 GetPlayerPosition(bool lerpWithPrevPos)
    {
        if (!MainCamera)
        {
            MainCamera = Camera.main;
        }
        if (Player == null || OccluderMaterial == null)
        {
            return Vector3.zero;
        }

        Vector3 pos = Player.position;

        Vector3 from = pos;
        Vector3 to = MainCamera.transform.position;
        Vector3 direction = (to - from).normalized;
        float distance = Vector3.Distance(from, to);
        hitCount = Physics.SphereCastNonAlloc(from, SphereCastRadius, direction, hits, distance, occluderLayerMask);
        if (ShouldInterpolate && hitCount > 0)
        {
            Vector3 hitPoint = Vector3.Lerp(pos, hits[0].point, InterpolationWithHitPoint);
            pos = hitPoint;
        }

        CutOutFadeSign = hitCount > 0 ? -1 : 1;
        CutOutFade = Math.Clamp(CutOutFade + CutOutFadeSign * FadeOutSpeed, 0.0f, 1.0f);

        if (lerpWithPrevPos)
        {
            pos = Vector3.Lerp(prevPos, pos, InterpolationWithPreviousPosition);
        }
        prevPos = pos;

        return pos;
    }

    void Update()
    {
        if (Player == null || OccluderMaterial == null)
        {
            return;
        }

        TimeElapsed += Time.deltaTime;
        if (TimeElapsed > TimeBetweenUpdates && OccluderMaterial != null)
        {
            OccluderMaterial.SetVector(PlayerPositionID, GetPlayerPosition(true));
            OccluderMaterial.SetFloat(CutOutFadeOutID, CutOutFade);
            TimeElapsed = 0;
        }
    }
}
