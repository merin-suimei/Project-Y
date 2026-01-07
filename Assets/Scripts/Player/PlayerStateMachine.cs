using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    private enum PlayerState
    {
        Idle,
        Move,
        Stuck
    }

    private PlayerState state = PlayerState.Idle;

    // For movement
    [SerializeField] private Camera cameraMain;
    [SerializeField] private Rigidbody rb;
    public Rigidbody Body => rb;

    // Input
    [SerializeField] private MonoBehaviour inputSource;

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float turnSpeed = 720f;


    private IPlayerInput _input;
    private Vector3 moveDir;
    
    private Vector3 _lastCheckpointPosition;

    private void Awake()
    {
        _input = inputSource as IPlayerInput;
        if (_input == null)
        {
            Debug.LogError("inputSource does not implement IPlayerInput!");
        }

        if (rb != null) 
            _lastCheckpointPosition = rb.position;
        else 
            _lastCheckpointPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (_input != null)
        {
            moveDir = _input.MoveDirection;
        }

        switch (state)
        {
            case PlayerState.Idle:
                UpdateIdle();
                break;
            case PlayerState.Move:
                UpdateMove();
                break;
            case PlayerState.Stuck:
                UpdateStuck();
                break;
        }

        RotateToAim();
    }

    private void FixedUpdate()
    {
        if (state == PlayerState.Move && moveDir.sqrMagnitude > 0.001f)
        {
            rb.MovePosition(rb.position + moveDir * moveSpeed * Time.fixedDeltaTime);
        }
        var v = rb.linearVelocity;
        v.x = 0f;
        v.z = 0f;
        rb.linearVelocity = v;
    }

    void ChangeState(PlayerState newState)
    {
        state = newState;
        
        if (state == PlayerState.Stuck)
        {
            TeleportToCheckpoint();
        }
    }

    void UpdateMove()
    {
        if (moveDir.sqrMagnitude < 0.001f)
            ChangeState(PlayerState.Idle);
    }

    void UpdateIdle()
    {
        if (moveDir.sqrMagnitude > 0.001f)
            ChangeState(PlayerState.Move);
    }

    void UpdateStuck()
    {
        ChangeState(PlayerState.Idle);
    }

    private void RotateToAim()
    {
        if (_input == null) return;
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

    private void TeleportToCheckpoint()
    {
        //Debug.Log("Teleporting to checkpoint...");
        
        //rb.isKinematic = true; 

        rb.position = _lastCheckpointPosition + Vector3.up * 0.1f;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        //rb.isKinematic = false;
    }

    public void OnChildTriggerEnter(Collider other)
    {
        if (other.CompareTag("StuckZone"))
        {
            ChangeState(PlayerState.Stuck);
        }

        if (other.CompareTag("Checkpoint"))
        {
            _lastCheckpointPosition = new Vector3(
                other.transform.position.x, 
                rb.position.y,
                other.transform.position.z
            );
            //Debug.Log($"Checkpoint updated: {_lastCheckpointPosition}");
        }
    }

    public void ForceUnstuck()
    {
        if (state != PlayerState.Stuck)
        {
            ChangeState(PlayerState.Stuck);
        }
    }

}
