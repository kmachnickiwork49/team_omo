using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonBehavior : MonoBehaviour
{
    // Add as many game objects as necessary
    public GameObject door;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D col) 
    {
        // Add another if statement for "box" objects or any other interactable objects that will push button
        if (col.gameObject.CompareTag("Player"))
        {
            door.SetActive(false);
        }
        else 
        {
            door.SetActive(true);
        }
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            door.SetActive(true);
        }
    }
}
