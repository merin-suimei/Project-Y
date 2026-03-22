using UnityEngine;

public class Enemy : Avatar
{
    [field: SerializeField] public EnemyType type { get; private set; }

    public float detectionRange { get; private set; }
    public float nearbyDetectionRange { get; private set; }
    public float detectionSemiconeAngle { get; private set; }
    public float catchThreshold { get; private set; }

    public float detectDelay { get; private set; }
    private LayerMask raycastIgnore;

    private float patrolSpeed;
    private float chaseSpeed;

    [field: SerializeField] public Transform EnemyEye { get; private set; }
    [field: SerializeField] public EnemyWalkPoint[] EnemyWalkPoints { get; private set; }
    [field: SerializeField] public bool IsPatrolPathClosed { get; private set; }

    protected override void Awake()
    {
        detectionRange = type.DetectionRange;
        nearbyDetectionRange = type.NearbyDetectionRange;
        detectionSemiconeAngle = type.DetectionSemiconeAngle;
        catchThreshold = type.CatchThreshold;

        raycastIgnore = type.RaycastIgnore;

        patrolSpeed = type.PatrolSpeed;
        chaseSpeed = type.ChaseSpeed;
        turnSpeed = type.TurnSpeed;

        detectDelay = type.DetectDelay;


        base.Awake();
        agent.speed = patrolSpeed;

        EventBus.Subscribe<int, bool>(EventType.EnemyEnableChaseSpeed, EnableChaseSpeed);
    }

    void OnDestroy()
    {
        EventBus.Unsubscribe<int, bool>(EventType.EnemyEnableChaseSpeed, EnableChaseSpeed);
    }

    private void EnableChaseSpeed(int targetID, bool startChase)
    {
        if (targetID != ID) return;

        agent.speed = startChase ? chaseSpeed : patrolSpeed;
    }

    public bool HasLineOfSight(Transform target)
    {
        if (Physics.Raycast(EnemyEye.position, (target.position - EnemyEye.position).normalized,out RaycastHit hit, detectionRange*2, ~raycastIgnore))
            return hit.transform == target;

        return false;
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        DrawVisionConeGizmos();

        UnityEditor.Handles.color = Color.red;
        UnityEditor.Handles.DrawWireArc(transform.position, Vector3.up, transform.forward, 360f, type.NearbyDetectionRange);
    }

    private void DrawVisionConeGizmos()
    {
        Vector3 leftRay = transform.position +
            Quaternion.Euler(0, -type.DetectionSemiconeAngle, 0) *
            (transform.forward * type.DetectionRange);

        Vector3 rightRay = transform.position +
            Quaternion.Euler(0, type.DetectionSemiconeAngle, 0) *
            (transform.forward * type.DetectionRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, leftRay);
        Gizmos.DrawLine(transform.position, rightRay);

        UnityEditor.Handles.color = Color.yellow;
        UnityEditor.Handles.DrawWireArc(transform.position, Vector3.up, Quaternion.Euler(0, -type.DetectionSemiconeAngle, 0) * transform.forward,
            type.DetectionSemiconeAngle*2, type.DetectionRange);
    }
#endif
}
