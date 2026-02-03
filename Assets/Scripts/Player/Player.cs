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

    public Animator animator { get; private set; }  

    // For movement
    [SerializeField] private Camera cameraMain;
    [SerializeField] private GameObject isometricCam;
    public Camera CameraMain => cameraMain;
    public GameObject IsometricCam => isometricCam;
    public Rigidbody rb {  get; private set; }

    // Input
    //[SerializeField] private MonoBehaviour inputSource;
    [SerializeField] private float moveSpeed = 7;
    [SerializeField] private float turnSpeed = 720f;
    public float MoveSpeed => moveSpeed;

    private IPlayerInput _input;
    public Vector3 moveDir {  get; private set; }
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        stateMachine = new StateMachine();
        idleState = new PlayerIdleState(this, stateMachine, "IsIdle");
        moveState = new PlayerMoveState(this, stateMachine, "IsMove");
        cutsceneState = new PlayerCutsceneState(this, stateMachine, "CutScene");
    }

    private void Start()
    {
        _input = ObjectResolver.Resolve<IPlayerInput>();
        if (_input == null)
        {
            _input = new InputSystemListener(); //for test (in final version we need to delete this line)
            Debug.LogError("inputSource does not implement IPlayerInput!");
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

        RotateToAim();
        stateMachine.CurrentState.StateUpdate();
    }

    public void SetVelocity(Vector3 velocity)
    {
        rb.linearVelocity = velocity;
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
