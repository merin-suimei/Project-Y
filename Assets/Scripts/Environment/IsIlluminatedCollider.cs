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
                player.IsIlluminated = true;
                Debug.Assert(player.IsIlluminated == true);
                // Debug.Log(player.IsIlluminated);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (player != null)
            {
                player.IsIlluminated = false;
                Debug.Assert(player.IsIlluminated == false);
                // Debug.Log(player.IsIlluminated);
            }
        }
    }
}