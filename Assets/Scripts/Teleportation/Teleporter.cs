using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] private Transform destination;
    
    public Transform GetDestination()
    {
        return destination;
    }

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.gameObject.TryGetComponent<ObjectTeleportation>(out ObjectTeleportation teleportedObject) && !teleportedObject.onCooldown)
        {
            teleportedObject.onCooldown = true;
            collision.gameObject.transform.position = destination.position;
        }
    }
}
