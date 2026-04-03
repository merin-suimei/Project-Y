using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputSystemListener : IPlayerInput, IDisposable
{
    private readonly InputsTypes _input;

    public Vector2 MoveDirection => _input.Player.Move.ReadValue<Vector2>().normalized;
    public Vector2 AimDirection => CalculateAimDirection();

    public event Action OnInteract;
    public InputSystemListener()
    {
        _input = new InputsTypes();
        _input.Enable();
        _input.Player.Interact.performed += InteractPerformed;
    }

    // Returns mouse position relative to screen center within largest circle
    // Vector length limited in range from 0.0 to 1.0
    private Vector2 CalculateAimDirection()
    {
        Vector2 mousePos = _input.Player.Look.ReadValue<Vector2>();
        float shortestHalfScreen = Mathf.Min(Screen.width/2, Screen.height/2);

        Vector2 relativeMousePos = mousePos - new Vector2(Screen.width/2, Screen.height/2);

        return Vector2.ClampMagnitude(relativeMousePos, shortestHalfScreen) / shortestHalfScreen;
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