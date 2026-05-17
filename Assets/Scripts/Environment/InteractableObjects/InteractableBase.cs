using UnityEngine;

public enum InteractableID
{
    Exit1_1, Exit1_2, Exit1_3_A, Exit1_3_B
}

public abstract class InteractableBase: MonoBehaviour
{
    [SerializeField] protected InteractableID id;
}
