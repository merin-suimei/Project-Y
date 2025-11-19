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
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float turnSpeed = 720f;
    private Vector3 inputVector;
    private Vector3 camForward;
    private Vector3 camRight;
    private Vector3 moveDir;

    // Update is called once per frame
    void Update()
    {
        GatherInput();

        switch (state)
        {
            case PlayerState.Idle:
                UpdateIdle();
                break;
            case PlayerState.Move:
                UpdateMove();
                break;
        }

        AimAtMouse();

    }
    void ChangeState(PlayerState newState)
    {
        state = newState;
    }

    void GatherInput()
    {
        inputVector = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        inputVector = inputVector.normalized;

        camForward = cameraMain.transform.forward;
        camRight = cameraMain.transform.right;

        camForward.y = 0;
        camRight.y = 0;

        camForward.Normalize();
        camRight.Normalize();

        moveDir = camForward * inputVector.z + camRight * inputVector.x;
    }

    private void FixedUpdate()
    {
        if (state == PlayerState.Move && moveDir.sqrMagnitude > 0.001f)
        {
            rb.MovePosition(rb.position + moveDir * moveSpeed * Time.fixedDeltaTime);
        }
        rb.linearVelocity = Vector3.zero;
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

    void AimAtMouse()
    {
        Ray ray = cameraMain.ScreenPointToRay(Input.mousePosition);

        Plane groundPlane = new Plane(Vector3.up, transform.position);

        if (groundPlane.Raycast(ray, out float distance))
        {
            Vector3 hitPoint = ray.GetPoint(distance);
            Vector3 lookDir = hitPoint - transform.position;
            lookDir.y = 0;

            if (lookDir.sqrMagnitude > 0.001f)
            {
                Quaternion targetRot = Quaternion.LookRotation(lookDir, Vector3.up);

                Quaternion newRot = Quaternion.RotateTowards(
                    rb.rotation,
                    targetRot,
                    turnSpeed * Time.deltaTime
                );

                rb.MoveRotation(newRot); // или transform.rotation = newRot;
            }
        }
    }
}
