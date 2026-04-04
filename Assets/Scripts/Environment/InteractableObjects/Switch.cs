using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Collider))]
public class SwitchView : InteractableBase
{
    // [Header("Interaction Settings")]
    // [SerializeField] protected GameObject promptUI; // UI подсказка над объектом

    [SerializeField] private bool isOn = false;
    [Tooltip("Если true, объект нельзя выключить после включения")]
    [SerializeField] private bool preventTurningOff = false;

    [Space(10)]
    [SerializeField] private string animationParamName = "IsOn";
    private Animator _animator;

    private IPlayerInput _input;

    private void Awake()
    {
        //if (promptUI != null) promptUI.SetActive(false);

        _animator = GetComponent<Animator>();
        _input = ObjectResolver.Resolve<IPlayerInput>();
    }

    private void Start()
    {
        _animator.SetBool(animationParamName, isOn);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            _input.OnInteract += Interact;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            _input.OnInteract -= Interact;
    }

    // public void SetHighlight(bool isActive)
    // {
    //     if (promptUI != null && promptUI.activeSelf != isActive)
    //     {
    //         promptUI.SetActive(isActive);
    //     }
    // }

    public void Interact()
    {
        // Если запрещено выключать И объект уже включен — запрещаем действие
        if (preventTurningOff && isOn)
        {
            Debug.Log("Switch back is not allowed");
            // Тут можно добавить звук, анимацию или другой эффект ошибки
            return;
        }

        isOn = !isOn;
        _animator.SetBool(animationParamName, isOn);

        EventBus.Raise(EventType.OnObjectToggle, id, isOn);
    }
}
