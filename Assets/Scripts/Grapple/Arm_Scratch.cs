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
    Rigidbody2D my_rb;

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
        my_rb = GetComponent<Rigidbody2D>();
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
        gameObject.transform.localScale = new Vector3(len, gameObject.transform.localScale.y, gameObject.transform.localScale.z);

        if (len > 0.05 && targetLocked) {
            TorqueArm(reticle.transform.position);
        } else {
            // No target lock, retracted
            RotateArm(reticle.transform.position);
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
        //gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), Time.deltaTime * rotationSpeed);

        // Match angle for 0 to 180
        // When angle -180 to -0, turn into 180 to 360
        float adjust_angle = angle;
        if (adjust_angle < 0) {
            adjust_angle += 360.0f;
        }
        // Negative coeff --> clockwise
        // Positive coeff --> counter-clockwise
        Debug.Log("target: " + adjust_angle + " z: " + transform.localRotation.eulerAngles.z);
        if (transform.localRotation.eulerAngles.z > 90 && transform.localRotation.eulerAngles.z < 180
            && adjust_angle > 270
            && adjust_angle - transform.localRotation.eulerAngles.z > 180) {
            // Negative for quadrant 2 -> 4
            my_rb.AddTorque(-0.0005f * rotationSpeed, ForceMode2D.Force);

        } else if (transform.localRotation.eulerAngles.z > 270
            && adjust_angle > 90 && adjust_angle < 180
            && transform.localRotation.eulerAngles.z - adjust_angle > 180) {
            // Positive for quadrant 4 -> 2
            my_rb.AddTorque(0.0005f * rotationSpeed, ForceMode2D.Force);

        } else if (transform.localRotation.eulerAngles.z > 180 && transform.localRotation.eulerAngles.z < 270
            && adjust_angle < 90
            && transform.localRotation.eulerAngles.z - adjust_angle > 180) {
            // Positive for quadrant 3 -> 1
            my_rb.AddTorque(0.0005f * rotationSpeed, ForceMode2D.Force);

        } else if (transform.localRotation.eulerAngles.z < 90
            && adjust_angle > 180 && adjust_angle < 270
            && adjust_angle - transform.localRotation.eulerAngles.z > 180) {
            // Negative for quadrant 1 -> 3
            my_rb.AddTorque(-0.0005f * rotationSpeed, ForceMode2D.Force);

        } else if (transform.localRotation.eulerAngles.z > 270 
            && adjust_angle < 90) {
            my_rb.AddTorque(0.0005f * rotationSpeed, ForceMode2D.Force);

        } else if (transform.localRotation.eulerAngles.z < 90 
            && adjust_angle > 270) {
            my_rb.AddTorque(-0.0005f * rotationSpeed, ForceMode2D.Force);

        } else if (transform.localRotation.eulerAngles.z > 270 
            && adjust_angle > 180 && transform.localRotation.eulerAngles.z - adjust_angle >= 2.0F) {
            my_rb.AddTorque(-0.0005f * rotationSpeed, ForceMode2D.Force);

        } else if (transform.localRotation.eulerAngles.z > 270
            && adjust_angle > 180 && transform.localRotation.eulerAngles.z - adjust_angle < -2.0F) {
            my_rb.AddTorque(0.0005f * rotationSpeed, ForceMode2D.Force);

        } else if (transform.localRotation.eulerAngles.z >= 0 
            && adjust_angle > 180 && transform.localRotation.eulerAngles.z - adjust_angle >= 2.0F) {
            my_rb.AddTorque(-0.0005f * rotationSpeed, ForceMode2D.Force);

        } else if (transform.localRotation.eulerAngles.z >= 0
            && adjust_angle > 180 && transform.localRotation.eulerAngles.z - adjust_angle < -2.0F) {
            my_rb.AddTorque(0.0005f * rotationSpeed, ForceMode2D.Force);

        } else if (adjust_angle <= 180 && transform.localRotation.eulerAngles.z - adjust_angle > 2.0F) {
            my_rb.AddTorque(-0.0005f * rotationSpeed, ForceMode2D.Force);

        } else if (adjust_angle <= 180 && transform.localRotation.eulerAngles.z - adjust_angle < -2.0F) {
            my_rb.AddTorque(0.0005f * rotationSpeed, ForceMode2D.Force);

        } else {
            // When at destination, don't need to apply more force, "cheat"?
            RotateArm(lookPoint);
            my_rb.angularVelocity = 0.0f; 
        }
    }
}
