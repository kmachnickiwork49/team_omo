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

    private int extending;
    int RETRACT = 0;
    int EXTEND = 1;
    int STAY = 2; // Should only trigger at max length, if min length, use RETRACT

    [SerializeField] float len;
    [SerializeField] float extendRate;
    [SerializeField] float maxLen;
    Rigidbody2D my_rb;

    bool startLaunch;
    float launch_arm_len;
    float launch_angle;
    //public bool pistonPush;
    // Turn pistonPush into extending
    [SerializeField] float recoil_force = 20f;

    // Start is called before the first frame update
    void Start()
    {
        targetLocked = false;
        player = GameObject.Find("Player");
        reticle = GameObject.Find("TargetReticle");
        //m_camera = GameObject.Find("Main Camera"); 
        // Init camera in GUI
        extending = RETRACT;
        len = 0.0f;
        if (maxLen < 1.0f) {
            maxLen = 1.0f;
        }
        if (extendRate < 0.1f) {
            extendRate = 0.1f;
        }
        my_rb = GetComponent<Rigidbody2D>();

        startLaunch = false;
        launch_arm_len = 0;
        launch_angle = 0;
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
        // Can only extend while a target is locked (left mouse click at least once)
        if (targetLocked && Input.GetKeyDown(KeyCode.Mouse1)) {
            if (extending == EXTEND) { extending = RETRACT; }
            else if (extending == RETRACT) { extending = EXTEND; } 
            else { extending = RETRACT; }
        }
        if (extending == EXTEND) {
            if (len < maxLen) {
                len += extendRate * Time.deltaTime;
            } else {
                len = maxLen;
                extending = STAY;
            }
        } else if (extending == RETRACT) {
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
            // Debug prevent rotate while retracted
            my_rb.angularVelocity = 0.0f;
        }

        // From Chris' changes
        Launch();
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
        //Debug.Log("target: " + adjust_angle + " z: " + transform.localRotation.eulerAngles.z);
        
        // Only accelerate to a max
        if (my_rb.angularVelocity > 50.0f) {
            my_rb.angularVelocity = 50.0f;
        } else if (my_rb.angularVelocity < -50.0f) {
            my_rb.angularVelocity = -50.0f;
        }


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

    // On contact with an interactable object other than OMO itself, interact
    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("OnCollisionEnter2D");
        if (col.gameObject.layer != LayerMask.NameToLayer("Player")
            && col.gameObject.layer != LayerMask.NameToLayer("Arm")) {
            // Collided with object, must interact
            // If can pick up, pick up
            // If can swing, swing
            // If neither (large object), push off
            Debug.Log(col.gameObject.name);
            if (col.gameObject.layer == LayerMask.NameToLayer("GrabbableObject")) {
                // PICK UP
                Debug.Log("grab");
                extending = STAY; // Trigger STAY when starting a grab
            } else if (col.gameObject.tag == "Ground") {
                // SWING
                Debug.Log("swing");
                extending = STAY; // Trigger STAY when starting a swing
            } else { 
                // PUSH OFF
                Debug.Log("recoil");
                // TODO: only apply force if object collided with is not grabbable - so only surfaces/terrain (later, tag all recoilable surfaces as "ground" and check for that)
                // to start a launch, piston must be in the middle of extending (not at max length)
                if (!startLaunch && extending == EXTEND && (len < maxLen))
                {
                    startLaunch = true;
                    //store info at time of collision:
                    launch_arm_len = len;
                    // Previous: - 90
                    launch_angle = Mathf.PI / 180 * (gameObject.transform.rotation.eulerAngles.z + 180);
                    // Debug.Log(launch_arm_len);
                    extending = RETRACT; //start retracting arm
                }
            }
        }
    }
    void Launch()
    {
        if (startLaunch)
        {
            Debug.Log("startLaunch");
            //apply force, scaled by arm's length at launch time
                //the closer you are to the wall, the more you are launched
                //if arm is at min length, apply max force (and vice versa)
            float launch_multiplier = ((maxLen - launch_arm_len)/maxLen) * recoil_force;
            Vector2 launch_direction = new Vector2(Mathf.Cos(launch_angle), Mathf.Sin(launch_angle)).normalized;
            player.GetComponent<Rigidbody2D>().AddForce(launch_direction * launch_multiplier, ForceMode2D.Impulse);

            startLaunch = false;
        }
    }

}
