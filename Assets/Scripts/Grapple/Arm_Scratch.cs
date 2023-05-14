using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arm_Scratch : MonoBehaviour
{

    [SerializeField] GameObject player;
    [SerializeField] GameObject reticle;
    [SerializeField] Camera m_camera;
    private bool targetLocked;
    [SerializeField] float rotationSpeed;

    private bool extending;
    [SerializeField] float len;
    [SerializeField] float extendRate;
    [SerializeField] float maxLen;

    // Start is called before the first frame update
    void Start()
    {
        targetLocked = false;
        player = GameObject.Find("Player");
        reticle = GameObject.Find("TargetReticle");
        //m_camera = GameObject.Find("Main Camera"); 
        // Init camera in GUI
        extending = false;
        len = 0.0f;
        if (maxLen < 1.0f) {
            maxLen = 1.0f;
        }
        if (extendRate < 0.1f) {
            extendRate = 0.1f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position =  new Vector3(player.transform.position.x, player.transform.position.y, gameObject.transform.position.z);
        Vector2 mousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            targetLocked = true;
        }

        // Extension of arm
        if (Input.GetKeyDown(KeyCode.Mouse1)) {
            extending = !extending;
        }
        if (extending) {
            if (len < maxLen) {
                len += extendRate * Time.deltaTime;
            } else {
                len = maxLen;
            }
        } else {
            if (len > 0) {
                len -= extendRate * Time.deltaTime;
            } else {
                len = 0;
            }
        }
        gameObject.transform.localScale = new Vector3(len, 1, 1);

        if (targetLocked) {
            TorqueArm(reticle.transform.position);
        } else {
            // No target lock, retracted
            RotateArm(mousePos);
        }
    }

    void RotateArm(Vector3 lookPoint)
    {
        Vector3 distanceVector = lookPoint - gameObject.transform.position;
        float angle = Mathf.Atan2(distanceVector.y, distanceVector.x) * Mathf.Rad2Deg;
        gameObject.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void TorqueArm(Vector3 lookPoint) {
        // Move towards lookPoint without passing through objects
        // Hit object while trying to get to target --> retract and take damage
        Vector3 distanceVector = lookPoint - gameObject.transform.position;
        float angle = Mathf.Atan2(distanceVector.y, distanceVector.x) * Mathf.Rad2Deg;
        gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), Time.deltaTime * rotationSpeed);
    }
}
