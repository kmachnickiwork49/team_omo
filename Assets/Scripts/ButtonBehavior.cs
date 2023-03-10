using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonBehavior : MonoBehaviour
{
    // Add as many game objects as necessary
    public GameObject door;


    private void OnCollisionEnter2D(Collision2D col) 
    {
        // Add another if statement for "box" objects or any other interactable objects that will push button
        if (col.gameObject.CompareTag("Player") || col.gameObject.CompareTag("Box"))
        {
            door.SetActive(false);
            //transform.position = transform.position + new Vector3(0, -0.1f, 0);
        }
        else 
        {
            door.SetActive(true);
        }
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player") || col.gameObject.CompareTag("Box"))
        {
            door.SetActive(true);
            //transform.position = transform.position + new Vector3(0, 0.1f, 0);
        }
    }
}
