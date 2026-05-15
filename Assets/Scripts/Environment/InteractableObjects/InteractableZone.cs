using UnityEngine;

public class InteractableZone : InteractableBase
{
    [SerializeField] private string checkedTag;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(checkedTag))
            EventBus.Raise(EventType.OnObjectToggle, id, true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(checkedTag))
            EventBus.Raise(EventType.OnObjectToggle, id, false);
    }
}
