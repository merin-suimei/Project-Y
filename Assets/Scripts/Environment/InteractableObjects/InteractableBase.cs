using UnityEngine;

public enum InteractableID
{
    //TODO: Add IDs
}

public abstract class InteractableBase: MonoBehaviour
{
    [SerializeField] protected InteractableID id;
}
