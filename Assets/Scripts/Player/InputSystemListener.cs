using UnityEngine;
using UnityEngine.InputSystem;

public class InputSystemListener : MonoBehaviour, IPlayerInput
{
    private InputsTypes _input;
    private Vector3 _moveDirection;
    private Vector3 _aimWorldPoint;

    public Vector3 MoveDirection => _moveDirection;
    public Vector3 AimWorldPoint => _aimWorldPoint;


    [SerializeField] private Camera cameraMain;
    [Tooltip("For player - rigidbody")]
    [SerializeField] private Transform aimOrigin;

    private void Awake()
    {
        _input = new InputsTypes();
        if (aimOrigin == null)
            aimOrigin = transform;
    }

    private void OnEnable() => _input.Enable();
    private void OnDisable() => _input.Disable();

    void Update()
    {
        ReadAim();
        ReadMovement();
    }

    void ReadMovement()
    {
        Vector2 _move = _input.Player.Move.ReadValue<Vector2>();
        Vector3  inputVector = new Vector3(_move.x, 0, _move.y);

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
        Vector2 mousePos = _input.Player.Look.ReadValue<Vector2>();
        Ray ray = cameraMain.ScreenPointToRay(mousePos);

        Plane groundPlane = new Plane(Vector3.up, aimOrigin.position);

        if (groundPlane.Raycast(ray, out float distance))
        {
            _aimWorldPoint = ray.GetPoint(distance);
        }
    }

}