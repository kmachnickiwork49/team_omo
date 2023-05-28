using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawCollDetect : MonoBehaviour {

    [SerializeField] GameObject arm;
    [SerializeField] GameObject toPickUp;

    // Start is called before the first frame update
    void Start()
    {
        arm = GameObject.Find("Arm");
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        //Debug.Log("OnCollisionEnter2D");
        // Collided with object, must interact
        // If can pick up, pick up
        // If can swing, swing
        // If neither (large object), push off
        //Debug.Log(col.gameObject.name);
        if (col.gameObject.layer == LayerMask.NameToLayer("GrabbableObject")) {
            // PICK UP
            Debug.Log("ccd grab");
            toPickUp = col.gameObject;
        } 
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject == toPickUp) {
            // PICK UP
            Debug.Log("ccd drop");
            toPickUp = null;
        } 
    }

    public GameObject getPickUp() {
        if (toPickUp != null && toPickUp.layer == LayerMask.NameToLayer("GrabbableObject")) {
            return toPickUp;
        }
        return null;
    }
}
