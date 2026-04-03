using UnityEngine;

namespace Environment
{
    public interface IInteractable
    {
        void Interact();
        Transform GetTransform();
    }
}