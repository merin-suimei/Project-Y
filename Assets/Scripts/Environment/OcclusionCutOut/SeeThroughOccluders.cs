using UnityEngine;

public class SeeThroughOccluders : MonoBehaviour
{
    /*
        Makes object transparent dynamically.

        Material's Surface type should be transparent. 
        Alpha clipping - Off.
    */
    [Header("Settings")]
    [SerializeField]
    float alpha = 0.8f;

    [Header("Objects")]
    [SerializeField]
    private Transform player; // TODO: maybe get player data from somewhere else
    [SerializeField]
    private Transform mainCamera;

    private (Material, Color)[] affectedObjects;
    private LayerMask occluderLayerMask;

    // buffer for raycast
    [Header("Raycast Settings")]
    RaycastHit[] hits;
    [SerializeField]
    private int MAX_HITS_BUFFER_SIZE = 3;

    void Start()
    {
        affectedObjects = new (Material, Color)[MAX_HITS_BUFFER_SIZE];
        hits = new RaycastHit[MAX_HITS_BUFFER_SIZE];

        occluderLayerMask = LayerMask.GetMask("Occluder");
        if (!mainCamera)
        {
            mainCamera = Camera.main.transform;
        }

        if (!player)
        {
            Debug.LogWarning("Player not set.");
        }
    }

    void Update()
    {
        if (!player)
            return;

        // return affected objects to primal state
        for (var i = 0; i < MAX_HITS_BUFFER_SIZE; ++i)
        {
            var (material, color) = affectedObjects[i];
            if (material != null)
                material.color = color;
            affectedObjects[i] = default;
        }

        // get player and camera geodata
        Vector3 from, to;
        from = player.position;
        to = mainCamera.transform.position;
        Vector3 direction = to - from;
        float distance = direction.magnitude;

        // raycasting
        Debug.DrawRay(from, direction, Color.red);
        int hitCount = Physics.RaycastNonAlloc(from, direction, hits, distance, occluderLayerMask);

        for (var i = 0; i < MAX_HITS_BUFFER_SIZE; ++i)
        {
            var hit = hits[i];
            if (hit.collider != null)
            {
                Renderer renderer = hit.collider.GetComponent<Renderer>();
                Color currentColor = renderer.material.color;
                Color newColor = new(currentColor.r, currentColor.g, currentColor.b, alpha);
                renderer.material.color = newColor;
                affectedObjects[i] = (renderer.material, currentColor);
            }
            hits[i] = default;
        }
    }
}
