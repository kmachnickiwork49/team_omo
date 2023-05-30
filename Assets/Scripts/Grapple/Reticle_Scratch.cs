using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reticle_Scratch : MonoBehaviour
{
    [SerializeField] Camera m_camera;
    [SerializeField] GameObject player;
    private Vector2 target;
    private bool targetLocked;

    // Start is called before the first frame update
    void Start()
    {
        targetLocked = false;
        //m_camera = GameObject.Find("Main Camera"); 
        // Init camera in GUI
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            target = mousePos - (Vector2)player.transform.position;
            gameObject.transform.position = new Vector3(target.x + player.transform.position.x, target.y + player.transform.position.y, gameObject.transform.position.z);
            targetLocked = true;
        }
        if (targetLocked) {
            gameObject.transform.position = new Vector3(target.x + player.transform.position.x, target.y + player.transform.position.y, gameObject.transform.position.z);
        } else {
            gameObject.transform.position = new Vector3(mousePos.x, mousePos.y, gameObject.transform.position.z);
        }
    }
}
