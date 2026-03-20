using UnityEngine;

public abstract class ControlScheme : ScriptableObject
{
    public abstract Vector3 CalculateMoveDirection(Vector2 moveInput, Transform playerTransform, Transform cameraTransform);
    public abstract Vector3 CalculateLookDirection(Vector2 aimInput, Vector3 currentMoveDir, Transform playerTransform, Transform cameraTransform);

    protected Vector3 GetCameraRelativeVector(Vector2 input, Transform cameraTransform)
    {
        if (input == Vector2.zero) return Vector3.zero;

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        forward.y = 0;
        right.y = 0;

        return forward.normalized * input.y + right.normalized * input.x;
    }
}