using UnityEngine;

public class AimTargetFollower : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float distanceFromPlayer = 6f;
    [SerializeField] private float followSpeed = 1f;

    private IPlayerInput _input;

    private void Awake()
    {
        if (player == null)
        {
            Debug.LogError("Player not assigned");
            return;
        }

        _input = player.GetComponent<IPlayerInput>();
        if (_input == null)
        {
            Debug.LogError("Player does not have a component implementing IPlayerInput");
        }
    }

    void Update()
    {

        if (_input == null || player == null) return;

        Vector3 aimPoint = _input.AimWorldPoint;
        Vector3 dir = aimPoint - player.position;
        dir.y = 0;

        Vector3 targetPos;

        if (dir.sqrMagnitude > 0.001f)
        {
            dir = dir.normalized;
            targetPos = player.position + dir * distanceFromPlayer;
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
}
