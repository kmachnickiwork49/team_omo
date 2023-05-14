using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// INSTRUCTIONS:
//
// 1. Attach this script to the large object to be broken
// 2. In the editor, drag all subObjects fragments onto the parent object, so they are visually overlapping. There's no need to hide them in the editor, as the script will automatically de-activate them. 
// 3. Give each sub-object the component of Box Collider 2D and RigidBody 2D. Don't worry about attributes, as the script will change them.
// 4. Add all the fragment subObjects to the "subObjects" array in the inspector. These subobjects will be hidden initially, until the parent object is broken.
//  Warning: DO NOT ADD parent object of the subObjects. It may look like its working, but the mass system will be broken.
// 5. Set the "breakVelocity" in the inspector. This is the minimum velocity required to break the object.
// 6. New sub-Object mass is calculated by dividing the original mass by the number of subObjects. To change that, uncomment the line for variable subObjectMassMultiplier, and change the like that calculates subObjectMass.
// 7. Newly emerged subobjects will automatically be in layer 10, "GrabbableObject". Their RigidBody2D Body type will be set to "Dynamic", and the mass will be changed as aforementioned.

public class BreakableObject : MonoBehaviour
{

    // This should contain all the subObjects themselves, NOT the parent object
    [SerializeField] private GameObject[] subObjects;


    // [SerializeField] private float subObjectMassMultiplier = 1f;

    // Arbitrarily set, will change later
    private float breakVelocity = 2f;

    private void Start()
    {
        DeactivateSubObjects();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.relativeVelocity.magnitude >= breakVelocity)
        {
            BreakIntoSubObjects();
        }
    }

    private void BreakIntoSubObjects()
    {
        
        int numSubObjects = subObjects.Length;
        float originalMass = GetComponent<Rigidbody2D>().mass;
        float subObjectMass = originalMass / numSubObjects;
        // float subObjectMass = originalMass * subObjectMassMultiplier;
        
        Destroy(gameObject);

        foreach (GameObject subObject in subObjects)
        {
            // GameObject newSubObject = Instantiate(subObject, transform.position, transform.rotation);
            Instantiate(subObject, transform.position, transform.rotation);
            subObject.SetActive(true);
            subObject.layer = 10;   // 10. "GrabbableObject"

            Rigidbody2D subObjectRigidbody = subObject.GetComponent<Rigidbody2D>();
            subObjectRigidbody.bodyType = RigidbodyType2D.Dynamic;
            subObjectRigidbody.mass = subObjectMass;
            // Debug.Log("New mass is " + subObjectRigidbody.mass);
        }

        
    }

    private void DeactivateSubObjects()
    {
        foreach (GameObject subObject in subObjects)
        {
            subObject.SetActive(false);
        }
    }
}