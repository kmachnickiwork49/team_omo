using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reticle_Scratch : MonoBehaviour
{
    [SerializeField] Camera m_camera;
    private Vector3 target;
    private bool targetLocked;

    // Start is called before the first frame update
    void Start()
    {
        targetLocked = false;
        //m_camera = GameObject.Find("Main Camera"); 
        // Init camera in GUI
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            target = mousePos;
            gameObject.transform.position = target;
            targetLocked = true;
        }
        if (targetLocked) {

        } else {
            gameObject.transform.position = mousePos;
        }
    }
}
