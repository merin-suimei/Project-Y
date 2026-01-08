using UnityEngine;

public class CollisionRelay : MonoBehaviour
{
    [SerializeField] private Player player;

    private void Awake()
    {
        if (player == null) 
            player = GetComponentInParent<Player>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (player != null)
        {
            player.HandleTriggerEnter(other);
        }
    }
}