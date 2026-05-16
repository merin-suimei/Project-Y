using UnityEngine;

public enum InteractableID
{
    Exit1_1
}

public abstract class InteractableBase: MonoBehaviour
{
    [SerializeField] protected InteractableID id;
}
