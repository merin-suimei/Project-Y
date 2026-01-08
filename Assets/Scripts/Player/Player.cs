using Unity.Cinemachine;
using UnityEngine;

public class Player : MonoBehaviour
{
    private bool isAllowedToRotate;
    public void SetRotationAllowed(bool isAllowed) => isAllowedToRotate = isAllowed;
    private StateMachine stateMachine;
    public PlayerIdleState idleState {  get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerCutsceneState cutsceneState { get; private set; }
    public PlayerStuckState stuckState {get; private set; }

    // For movement
    [SerializeField] private Camera cameraMain;
    [SerializeField] private GameObject isometricCam;
    public Camera CameraMain => cameraMain;
    public GameObject IsometricCam => isometricCam;
    public Rigidbody rb {  get; private set; }

    // Input
    [SerializeField] private MonoBehaviour inputSource;
    [SerializeField] private float moveSpeed = 7;
    [SerializeField] private float turnSpeed = 720f;
    public float MoveSpeed => moveSpeed;

    private IPlayerInput _input;
    public Vector3 moveDir {  get; private set; }

    private Vector3 checkpointPosition;
    private void Awake()
    {
        rb = GetComponentInChildren<Rigidbody>();
        _input = inputSource as IPlayerInput;
        if (_input == null)
        {
            Debug.LogError("inputSource does not implement IPlayerInput!");
        }

        stateMachine = new StateMachine();
        idleState = new PlayerIdleState(this, stateMachine, "IsIdle");
        moveState = new PlayerMoveState(this, stateMachine, "IsMove");
        cutsceneState = new PlayerCutsceneState(this, stateMachine, "CutScene");
        stuckState = new PlayerStuckState(this, stateMachine, "Stuck");
    }

    private void Start()
    {
        if (rb != null)
        {
            SetCheckpoint(rb.position);
        }
        else
        {
            SetCheckpoint(transform.position);
        }

        stateMachine.Initialize(idleState);
    }

    // Update is called once per frame
    void Update()
    {
        if (_input != null)
        {
            moveDir = _input.MoveDirection;
        }

        stateMachine.CurrentState.StateUpdate();
        RotateToAim();
    }

    public void SetVelocity(Vector3 velocity)
    {
        rb.linearVelocity = velocity;
    }

    public void SetCheckpoint(Vector3 pos)
    {
        checkpointPosition = pos;
        //Debug.Log($"New Checkpoint Saved: {pos}");
    }

    public void ForceStuck()
    {
        if (stateMachine.CurrentState != stuckState)
        {
            stateMachine.ChangeState(stuckState);
        }
    }

    public void TeleportToCheckpoint()
    {
        if (rb == null) return;

        Vector3 safePos = checkpointPosition + Vector3.up * 0.01f;

        rb.position = safePos;

        SetVelocity(Vector3.zero);
        rb.angularVelocity = Vector3.zero;

        //Debug.Log("Player Teleported to Checkpoint");
    }

    public void HandleTriggerEnter(Collider other)
    {
        if (other.CompareTag("Checkpoint"))
        {
            SetCheckpoint(new Vector3(other.transform.position.x, rb.position.y, other.transform.position.z));
        }
        else if (other.CompareTag("StuckZone"))
        {
            ForceStuck();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        HandleTriggerEnter(other);
    }

    private void RotateToAim()
    {
        if (_input == null || !isAllowedToRotate) return;
        Vector3 originPos = rb != null ? rb.position : transform.position;

        Vector3 aim = _input.AimWorldPoint;
        Vector3 lookDir = aim - originPos;
        lookDir.y = 0;

        if (lookDir.sqrMagnitude > 0.001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(lookDir, Vector3.up);

            Quaternion newRot = Quaternion.RotateTowards(
                rb.rotation,
                targetRot,
                turnSpeed * Time.deltaTime
            );

            rb.MoveRotation(newRot);
        }
    }
}
