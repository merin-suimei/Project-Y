using System;
using System.Collections.Generic;
using UnityEngine;

public class InteractableReciever : MonoBehaviour
{
    [SerializeField] protected List<InteractableID> checkedIds;

    private Action<InteractableID, bool> checkedAction;

    public void Awake()
    {
        checkedAction = (id, state) => { if (checkedIds.Contains(id)) Action(state); };
        EventBus.Subscribe(EventType.OnObjectToggle, checkedAction);
    }

    public void OnDestroy()
    {
        EventBus.Unsubscribe(EventType.OnObjectToggle, checkedAction);
    }

    protected virtual void Action(bool state)
    {
        Debug.Log(gameObject.name + " recieved trigger");
    }
}
