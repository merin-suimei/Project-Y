using UnityEngine;
using UnityEngine.InputSystem;

public class FlashlightToggle : MonoBehaviour
{
    [SerializeField] private Light _flashlight;
    private InputsTypes _input;

    private void Awake()
    {
        if (_flashlight == null)
        {
            Debug.LogError("Flashlight not assigned");
            return;
        }
        _input = new InputsTypes();
    }

    private void OnEnable()
    {
        _input.Enable();
        _input.Player.Flashlight.performed += OnFlashlight;
    }

    private void OnDisable()
    {
        _input.Player.Flashlight.performed -= OnFlashlight;
        _input.Disable();
    }

    private void OnFlashlight(InputAction.CallbackContext ctx)
    {
        _flashlight.enabled = !_flashlight.enabled;
    }

}
