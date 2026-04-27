using UnityEngine;

public class LightZoneCollider : MonoBehaviour
{
    public Avatar player;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (player != null)
            {
                player.IsIlluminated++;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (player != null)
            {
                player.IsIlluminated--;
            }
        }
    }
}