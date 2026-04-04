using UnityEngine;
using UnityEngine.InputSystem;

public class FlashlightToggle : MonoBehaviour
{
    [SerializeField] private Light _flashlight;
    private InputsTypes _input;

    private void Awake()
    {
        _input = new InputsTypes();

        if (_flashlight == null)
        {
            Debug.LogError("Flashlight not assigned");
        }
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
        if (_flashlight == null)
        {
            Debug.LogError("Flashlight not assigned");
            return;
        }

        _flashlight.enabled = !_flashlight.enabled;
    }

    private void OnDestroy()
    {
        _input.Dispose();
    }

}
