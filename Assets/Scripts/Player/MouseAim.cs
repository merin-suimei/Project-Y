using UnityEngine;

public class AimTargetFollower : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float distanceFromPlayer = 6f;
    [SerializeField] private float followSpeed = 1f;
    [SerializeField] private Camera cameraMain;

    private IPlayerInput _input;

    private void Awake()
    {
        if (player == null)
        {
            Debug.LogError("Player not assigned");
            return;
        }

        _input = ObjectResolver.Resolve<IPlayerInput>();
        if (_input == null)
        {
            Debug.LogError("Player does not have a component implementing IPlayerInput");
        }
    }

    void Update()
    {

        if (_input == null || player == null) return;

        Vector3 aimDir = RotateInputVector(_input.AimDirection);
        aimDir.y = 0;

        Vector3 targetPos;

        if (aimDir.sqrMagnitude > 0.001f)
        {
            targetPos = player.position + aimDir * distanceFromPlayer;
        }
        else
        {
            targetPos = player.position;
        }

            
        transform.position = Vector3.Lerp(
            transform.position,
            targetPos,
            followSpeed * Time.deltaTime
        );
    }

    private Vector3 RotateInputVector(Vector2 input)
    {
        if (cameraMain == null) cameraMain = Camera.main;
        if (input == Vector2.zero) return Vector3.zero;

        Vector3 forward = cameraMain.transform.forward;
        Vector3 right = cameraMain.transform.right;
        forward.y = 0;
        right.y = 0;

        return forward.normalized*input.y + right.normalized*input.x;
    }
}
