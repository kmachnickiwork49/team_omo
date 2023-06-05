using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBehavior : MonoBehaviour
{
    [SerializeField] GameObject locked_wall;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D col) 
    {
        // Add another if statement for "box" objects or any other interactable objects that will push button
        if (col.gameObject.CompareTag("Player"))
        {
            Destroy(locked_wall);   
            Destroy(gameObject);         
        }
    }
}
