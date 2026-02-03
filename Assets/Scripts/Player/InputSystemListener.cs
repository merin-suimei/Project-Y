using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputSystemListener : IPlayerInput, IDisposable
{
    private readonly InputsTypes _input;

    public Vector3 MoveDirection => CalculateMoveDirection();
    public Vector3 AimWorldPoint => CalculateAimPoint();
    public event Action OnInteract;
    
    private Camera camera;
    public InputSystemListener()
    {
        _input = new InputsTypes();
        _input.Enable();
        _input.Player.Interact.performed += InteractPerformed;
    }

    private Vector3 CalculateMoveDirection()
    {
        if (camera == null)
        {
            camera = Camera.main;
        }


        Vector2 move = _input.Player.Move.ReadValue<Vector2>();
        if (move == Vector2.zero) return Vector3.zero;

        Vector3 forward = camera.transform.forward;
        Vector3 right = camera.transform.right;
        forward.y = 0;
        right.y = 0;

        return (forward.normalized * move.y + right.normalized * move.x);
    }

    private Vector3 CalculateAimPoint()
    {
        if (camera == null)
        {
            camera = Camera.main;
        }

        Vector2 mousePos = _input.Player.Look.ReadValue<Vector2>();
        Ray ray = camera.ScreenPointToRay(mousePos);

        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

        return groundPlane.Raycast(ray, out float dist) ? ray.GetPoint(dist) : Vector3.zero;
    }
    
    private void InteractPerformed(InputAction.CallbackContext context)
    {
        OnInteract?.Invoke();
    }

    public void Dispose()
    {
        _input.Player.Interact.performed -= InteractPerformed;
        _input.Disable();
        _input.Dispose();
    }
}