using UnityEngine;

public class KeyboardMouseInput : MonoBehaviour, IPlayerInput
{
    private Vector3 _moveDirection;
    private Vector3 _aimWorldPoint;

    public Vector3 MoveDirection => _moveDirection;
    public Vector3 AimWorldPoint => _aimWorldPoint;


    [SerializeField] private Camera cameraMain;
    [Tooltip("For player - rigidbody")]
    [SerializeField] private Transform aimOrigin;

    private void Awake()
    {
        if (aimOrigin == null)
            aimOrigin = transform;
    }

    void Update()
    {
        ReadAim();
        ReadMovement();
    }

    void ReadMovement()
    {
        Vector3  inputVector = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        inputVector = inputVector.normalized;

        Vector3 camForward = cameraMain.transform.forward;
        Vector3 camRight = cameraMain.transform.right;

        camForward.y = 0;
        camRight.y = 0;

        camForward.Normalize();
        camRight.Normalize();

        _moveDirection = camForward * inputVector.z + camRight * inputVector.x;
    }

    private void ReadAim()
    {
        Ray ray = cameraMain.ScreenPointToRay(Input.mousePosition);

        Plane groundPlane = new Plane(Vector3.up, aimOrigin.position);

        if (groundPlane.Raycast(ray, out float distance))
        {
            _aimWorldPoint = ray.GetPoint(distance);
        }
    }

}