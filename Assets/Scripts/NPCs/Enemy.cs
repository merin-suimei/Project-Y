using System;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.Splines.Interpolators;

public class Enemy : MonoBehaviour
{
    [Header("")]
    [Tooltip("Range to detect possible collision")]
    [SerializeField] private float detectionRange = 10f;
    [Tooltip("Semicon angle for detection (in degrees)")]
    [Range(0f, 90f)]
    [SerializeField] private float detectionSemiconeAngle = 45f;
    [SerializeField] private Transform enemyEye;

    [SerializeField] private LayerMask playerMask;
    public StateMachine stateMachine { get; private set; }
    public EnemyChaseState chaseState { get; private set; }
    public EnemyDetectState detectState { get; private set; }
    public virtual EnemyState patrolState { get; protected set; }
    public NavMeshAgent agent { get; private set; }
    public Transform player { get; private set; }



    public float detectDelay { get; private set; } = 0.5f;

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        stateMachine = new StateMachine();
        chaseState = new EnemyChaseState(this, stateMachine, "IsChase");
        detectState = new EnemyDetectState(this, stateMachine, "IsDetect");
    }

    protected virtual void Start() // Сделал protected virtual чтобы можно было переопределить
    {
        player = GameManager.instance.player.rb.transform;
    }
    public virtual void Update()
    {
        stateMachine.CurrentState.StateUpdate();
    }

    public bool IsPlayerVisible()
    {
        Vector3 dir = (player.position - enemyEye.position).normalized;

        if (Vector3.Angle(enemyEye.forward, dir) > detectionSemiconeAngle)
            return false;
        if (Vector3.Distance(enemyEye.position, player.position) > detectionRange)
            return false;
        if (Physics.Raycast(enemyEye.position, dir, out RaycastHit hit, detectionRange))
        {
            if (hit.transform == player)
                return true;
        }

        return false;
    }

    public bool IsPlayerChaseable() =>
        Vector3.Distance(enemyEye.position, player.position) <= detectionRange;


#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        DrawVisionConeGizmos();
        UnityEditor.Handles.color = Color.red;
        UnityEditor.Handles.DrawWireArc(transform.position, Vector3.up, transform.forward, 360f, detectionRange);
    }

    private void DrawVisionConeGizmos()
    {
        Vector3 leftRay = transform.position +
            Quaternion.Euler(0, detectionSemiconeAngle, 0) *
            (transform.forward * detectionRange);

        Vector3 rightRay = transform.position +
            Quaternion.Euler(0, -detectionSemiconeAngle, 0) *
            (transform.forward * detectionRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, leftRay);
        Gizmos.DrawLine(transform.position, rightRay);
    }
#endif
}
