using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableReciever : MonoBehaviour
{
    protected List<InteractableID> checkedIDs;

    public void Awake()
    {
        EventBus.Subscribe<InteractableID, bool>(EventType.OnObjectToggle, Action);
    }

    void OnDestroy()
    {
        EventBus.Unsubscribe<InteractableID, bool>(EventType.OnObjectToggle, Action);
    }

    protected virtual void Action(InteractableID id, bool state)
    {
        if (!checkedIDs.Contains(id))
            return;

        Debug.Log(gameObject.name + " recieved trigger: " + id.ToString());
    }
}
