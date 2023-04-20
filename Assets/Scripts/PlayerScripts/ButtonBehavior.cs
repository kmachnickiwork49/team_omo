using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonBehavior : MonoBehaviour
{
    // Add as many game objects as necessary
    public GameObject[] targets;
    [SerializeField] int numOn;

    private void Start() {
        numOn = 0;
    }

    private void OnTriggerEnter2D(Collider2D col) 
    {
        // Add another if statement for "box" objects or any other interactable objects that will push button
        if (col.gameObject.CompareTag("Player") || col.gameObject.CompareTag("Box"))
        {
            numOn += 1;
            foreach (GameObject o in targets) {
                o.SetActive(false);
            }
            //transform.position = transform.position + new Vector3(0, -0.1f, 0);
        }
        else 
        {
            //door.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player") || col.gameObject.CompareTag("Box"))
        {
            numOn -= 1;
            if (numOn <= 0) {
                foreach (GameObject o in targets) {
                    o.SetActive(true);
                }
            }
            //transform.position = transform.position + new Vector3(0, 0.1f, 0);
        }
    }
}
