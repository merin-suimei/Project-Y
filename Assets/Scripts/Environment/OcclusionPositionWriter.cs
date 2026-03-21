using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class OcclusionPositionWriter : MonoBehaviour
{
    // set values from the inspector
    public Material OccluderMaterial;
    public Camera MainCamera;
    public Transform Player;
    public bool ShouldInterpolate;

    // only select occluder layer mask - they will have the shader needed 
    // TODO: maybe do it easier
    private static readonly int PlayerPositionID = Shader.PropertyToID("_PlayerPosition");
    private LayerMask occluderLayerMask;

    // timer
    float TimeElapsed = 0;
    public float TimeBetweenUpdates = 0.1f;

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
    }

    void OnDrawGizmos()
    {
        if (Player == null || MainCamera == null)
            return;
        
        Vector3 from = Player.position;
        Vector3 to = MainCamera.transform.position;
        Vector3 direction = (to - from).normalized;
        float radius = 0.5f;
        Gizmos.DrawWireSphere(from + direction, radius);
    }


    private RaycastHit[] hits;

    private Vector3 prevPos;
    public Vector3 GetPlayerPosition(bool lerpWithPrevPos)
    {
        if (Player == null || MainCamera == null)
            return Vector3.zero;
        
        Vector3 pos = Player.position;
        Vector3 from = pos;
        Vector3 to = MainCamera.transform.position;
        // Vector3 direction = (to - from).normalized;
        float distance = Vector3.Distance(from, to);

        var ray = MainCamera.ScreenPointToRay(Input.mousePosition);
        int hitCount = Physics.RaycastNonAlloc(ray, hits, distance, occluderLayerMask);
        if (ShouldInterpolate && hitCount > 0)
        {
            Vector3 hitPoint = hits[0].point;
            pos = Vector3.Lerp(pos, hitPoint, 0.1f);
        }

        if(lerpWithPrevPos) pos = Vector3.Lerp(prevPos, pos, 0.1f);
        prevPos = pos;

        return pos;
    }

    void Update()
    {
        TimeElapsed += Time.deltaTime;
        if (TimeElapsed > TimeBetweenUpdates && OccluderMaterial != null)
        {
            OccluderMaterial.SetVector(PlayerPositionID, GetPlayerPosition(true));
            TimeElapsed = 0;
        }
    }
}
