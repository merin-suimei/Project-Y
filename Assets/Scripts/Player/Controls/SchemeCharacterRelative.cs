using UnityEngine;

[CreateAssetMenu(fileName = "SchemeCharacterRelative", menuName = "Controls/Character Relative")]
public class SchemeCharacterRelative : ControlScheme
{
    public override Vector3 CalculateMoveDirection(Vector2 moveInput, Transform playerTransform, Transform cameraTransform)
    {
        if (moveInput == Vector2.zero) return Vector3.zero;

        Vector3 forward = playerTransform.forward;
        Vector3 right = playerTransform.right;
        forward.y = 0;
        right.y = 0;

        return forward.normalized * moveInput.y + right.normalized * moveInput.x;
    }

    public override Vector3 CalculateLookDirection(Vector2 aimInput, Vector3 currentMoveDir, Transform playerTransform, Transform cameraTransform)
    {
        return GetCameraRelativeVector(aimInput, cameraTransform);
    }
}