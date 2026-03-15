using UnityEngine;

[CreateAssetMenu(fileName = "SchemeAutoAim", menuName = "Controls/Auto Aim (Little Nightmares)")]
public class SchemeAutoAim : ControlScheme
{
    public override Vector3 CalculateMoveDirection(Vector2 moveInput, Transform playerTransform, Transform cameraTransform)
    {
        return GetCameraRelativeVector(moveInput, cameraTransform);
    }

    public override Vector3 CalculateLookDirection(Vector2 aimInput, Vector3 currentMoveDir, Transform playerTransform, Transform cameraTransform)
    {
        // Игнорируем мышь, смотрим туда, куда идем
        return currentMoveDir; 
    }
}