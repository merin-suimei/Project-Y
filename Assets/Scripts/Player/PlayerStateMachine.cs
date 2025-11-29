using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    private enum PlayerState
    {
        Idle,
        Move
    }

    private PlayerState state = PlayerState.Idle;

    // For movement
    [SerializeField] private Camera cameraMain;
    [SerializeField] private Rigidbody rb;

    // Input
    [SerializeField] private MonoBehaviour inputSource;

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float turnSpeed = 720f;


    private IPlayerInput _input;
    private Vector3 moveDir;

    private void Awake()
    {
        _input = inputSource as IPlayerInput;
        if (_input == null)
        {
            Debug.LogError("inputSource does not implement IPlayerInput!");
        }
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
}
