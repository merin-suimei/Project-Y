using UnityEngine;

public class CollisionRelay : MonoBehaviour
{
    [SerializeField] private PlayerStateMachine mainScript;

    private void Awake()
    {
        if (mainScript == null)
            mainScript = GetComponentInParent<PlayerStateMachine>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (mainScript != null)
        {
            mainScript.OnChildTriggerEnter(other);
        }
    }
}