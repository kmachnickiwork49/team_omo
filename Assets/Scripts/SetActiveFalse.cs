using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveFalse : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnMouseDown()
    {
        // Destroy the gameObject after clicking on it
        Destroy(gameObject);
    }
}
