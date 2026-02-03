using UnityEngine;

namespace Environment
{
    [RequireComponent(typeof(Collider))] 
    public abstract class InteractableBase : MonoBehaviour, IInteractable
    {
        // [Header("Interaction Settings")]
        // [SerializeField] protected GameObject promptUI; // UI подсказка, которая будет вырисовываться над объектом

        protected virtual void Awake()
        {
            //if (promptUI != null) promptUI.SetActive(false);
        }
        
        protected virtual void OnTriggerEnter(Collider other)
        {
            Debug.Log("123");
            if (other.CompareTag("Player"))
            {
                Debug.Log(other.name + "entered");
                IPlayerInput input = ObjectResolver.Resolve<IPlayerInput>();
                input.OnInteract += Interact;
            }
        }

        protected virtual void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log(other.name + "exited");
                IPlayerInput input = ObjectResolver.Resolve<IPlayerInput>();
                input.OnInteract -= Interact;
            }
        }

        public Transform GetTransform() => transform;
        
        // public void SetHighlight(bool isActive)
        // {
        //     if (promptUI != null && promptUI.activeSelf != isActive)
        //     {
        //         promptUI.SetActive(isActive);
        //     }
        // }
        
        public abstract void Interact(); 
        
    }
}