using UnityEngine;

[CreateAssetMenu(fileName = "SchemeCameraRelative", menuName = "Controls/Camera Relative")]
public class SchemeCameraRelative : ControlScheme
{
    public override Vector3 CalculateMoveDirection(Vector2 moveInput, Transform playerTransform, Transform cameraTransform)
    {
        return GetCameraRelativeVector(moveInput, cameraTransform);
    }

    public override Vector3 CalculateLookDirection(Vector2 aimInput, Vector3 currentMoveDir, Transform playerTransform, Transform cameraTransform)
    {
        return GetCameraRelativeVector(aimInput, cameraTransform);
    }
}