using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectTeleportation : MonoBehaviour
{
    public bool onCooldown;
    [SerializeField] private float cooldown;
    private float currentCooldown;

    private void Update() 
    {
        if (onCooldown)
        {
            if (cooldown > currentCooldown)
            {
                currentCooldown += Time.deltaTime;
            }
            else 
            {
                currentCooldown = 0f;
                onCooldown = false;
            }
        }
    }
}
