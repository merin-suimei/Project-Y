using System;
using System.ComponentModel.Design;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;


public class LightZoneColliderRaycast : MonoBehaviour
{
    public Avatar player;
    public bool CheckRaycastToPlayer;
    public Transform playerPos;
    public Transform lightPos;
    private bool PlayerLit = true;
    private bool PlayerInsideCollider = true;
    private RaycastHit[] hits = new RaycastHit[1];

    public void Update()
    {
        Debug.Log(player.IsIlluminated);
        if(!PlayerInsideCollider) return;
        bool raycastHitsPlayer = CheckRaycast();
        if(PlayerLit && !raycastHitsPlayer)
        {
            PlayerLit = false; 
            player.IsIlluminated--;
        }
        if(PlayerLit && raycastHitsPlayer)
        {
        }
        if(!PlayerLit && !raycastHitsPlayer)
        {
        }
        if(!PlayerLit && raycastHitsPlayer)
        {
            PlayerLit = true; 
            player.IsIlluminated++;
        }
    }

    private bool CheckRaycast()
    {
        if(!CheckRaycastToPlayer)
        {
            return true;
        }
        if (player == null || playerPos == null || lightPos == null)
        {
            Debug.LogWarning("Missing references in LightZoneCollider. Assume true.");
            return true;
        }
        Vector3 from = lightPos.position;
        Vector3 to = playerPos.position;
        Vector3 direction = to - from;
        float distance = direction.magnitude;
        // Debug.DrawRay(from, direction, Color.red);
        int hitCount = Physics.RaycastNonAlloc(from, direction.normalized, hits, distance, -1, QueryTriggerInteraction.Ignore);
        // Debug.Log(hits[0].collider.gameObject.name);
        if (hitCount == 1 && hits[0].collider.CompareTag("Player"))
        {
            return true;
        }

        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (player != null)
            {
                PlayerInsideCollider = true;
                if(CheckRaycast())
                {
                   PlayerLit = true; 
                   player.IsIlluminated++; 
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (player != null)
            {
                PlayerInsideCollider = false;
                if(PlayerLit) player.IsIlluminated--;
                PlayerLit = false;
            }
        }
    }
}