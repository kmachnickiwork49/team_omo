using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmController : MonoBehaviour
{
    GameObject playerRef;
    GameObject clawRef;

public    bool pistonPush;
    Collider2D[] results;
    Vector3 target;

    //[SerializeField] float max_rot_speed = 10f;
    [SerializeField] float max_arm_len = 4f;
    float arm_len;
    [SerializeField] float rot_rate = 50f;
    [SerializeField] float checkAng = 5f;
    [SerializeField] float extend_rate = 0.005f;
    [SerializeField] float recoil_force = 20f;

    bool startLaunch;
    float launch_arm_len;
    float launch_angle;

    // Start is called before the first frame update
    void Start()
    {
        playerRef = GameObject.Find("Player");
        clawRef = GameObject.Find("Claw");
        target = new Vector3(0,0,0);
        arm_len = 0;
        pistonPush = false;
        results = new Collider2D[1];
        startLaunch = false;
        launch_arm_len = 0;
        launch_angle = 0;
    }

    void RotateToTarget()
    {

        // Target follows user cursor, but stay within range of OMO
        Vector3 mousePos = Input.mousePosition;
        target = Camera.main.ScreenToWorldPoint(mousePos);
        target = new Vector3 (target.x, target.y, 0); 
        // For some reason, default -10 z
        // Need to keep track of z for visual hierarchy within scene, for now, 0

        //if (Physics2D.GetContacts(gameObject.GetComponent<Collider2D>(), results) == 0) {
            Vector2 distanceVector = gameObject.transform.position - target;
            //float target_angle = Mathf.Atan2(distanceVector.y, distanceVector.x) * Mathf.Rad2Deg;
            float target_angle = Vector2.SignedAngle(distanceVector, new Vector2(1, 0));
            float target_angle_alt = Vector2.SignedAngle(distanceVector, new Vector2(0, 1));
            launch_angle = Mathf.PI / 180 * (gameObject.transform.rotation.eulerAngles.z - 90);
            Vector2 launch_direction = new Vector2(Mathf.Cos(launch_angle), Mathf.Sin(launch_angle)).normalized;

            Debug.Log("target: " + (-target_angle_alt + 180) + " curr: " + (launch_angle * Mathf.Rad2Deg + 90));

            
            //Cast a ray in the direction specified in the inspector.
            RaycastHit2D hit = Physics2D.Raycast(this.gameObject.transform.position, -distanceVector, arm_len, LayerMask.GetMask("Default"));
            //Debug.Log((Quaternion.Euler(0, 0, 3)) * -distanceVector + " " + -distanceVector);
            RaycastHit2D hitP = Physics2D.Raycast(this.gameObject.transform.position, ((Quaternion.Euler(0, 0, checkAng)) * -launch_direction ), arm_len, LayerMask.GetMask("Default"));
            RaycastHit2D hitN = Physics2D.Raycast(this.gameObject.transform.position, ((Quaternion.Euler(0, 0, -checkAng)) * -launch_direction ), arm_len, LayerMask.GetMask("Default"));
            //Debug.Log(hitP.collider == null && hitN.collider == null);

            // Second part of condition should not occur
            if (/* (hit.collider == null || (hit.collider != null && hit.distance >= arm_len)) && */ ((hitP.collider == null &&  (-target_angle_alt + 180 + 360) > (launch_angle * Mathf.Rad2Deg + 90 + 360)) || (hitN.collider == null && (-target_angle_alt + 180 + 360) < (launch_angle * Mathf.Rad2Deg + 90 + 360))) )
            {
                // Rotate if no impedance detected, no jump
                //if ((gameObject.transform.rotation.z+360 < angle+90+360 + 3) && (gameObject.transform.rotation.z+360 > angle+90+360 - 3)) {
                //if (arm_len < 0.5 || (Quaternion.Angle(transform.rotation, Quaternion.AngleAxis(angle + 90, Vector3.forward)) > -10 && Quaternion.Angle(transform.rotation, Quaternion.AngleAxis(angle + 90, Vector3.forward)) < 10)) {
                if (arm_len > 0.5) {
                    //gameObject.transform.rotation = Quaternion.AngleAxis(-target_angle + 90, Vector3.forward);
                    gameObject.transform.rotation = Quaternion.RotateTowards(gameObject.transform.rotation, Quaternion.AngleAxis(-target_angle + 90, Vector3.forward), Time.deltaTime * rot_rate);
                } else {
                    gameObject.transform.rotation = Quaternion.AngleAxis(-target_angle + 90, Vector3.forward);
                }
            }
        //}
            else {
                // Keep this fixed
                //gameObject.transform.rotation = Quaternion.RotateTowards(gameObject.transform.rotation, Quaternion.AngleAxis(angle + 90, Vector3.forward), Time.deltaTime * rot_rate);
            }
    }
    
     void OnCollisionEnter2D(Collision2D collision) //apply force only at initial collision time
    {
        //TODO: only apply force if object collided with is not grabbable - so only surfaces/terrain (later, tag all recoilable surfaces as "ground" and check for that)

        //to start a launch, piston must be in the middle of extending (not at max length)
        if (!startLaunch && pistonPush && (arm_len < max_arm_len))
        {
            startLaunch = true;

            //store info at time of collision:
            launch_arm_len = arm_len;
            launch_angle = Mathf.PI / 180 * (gameObject.transform.rotation.eulerAngles.z - 90);
            // Debug.Log(launch_arm_len);
            pistonPush = !pistonPush; //start retracting arm
        }
    }
    
    void Launch()
    {
        if (startLaunch)
        {
            //apply force, scaled by arm's length at launch time
                //the closer you are to the wall, the more you are launched
                //if arm is at min length, apply max force (and vice versa)
            float launch_multiplier = ((max_arm_len - launch_arm_len)/max_arm_len) * recoil_force;
            Vector2 launch_direction = new Vector2(Mathf.Cos(launch_angle), Mathf.Sin(launch_angle)).normalized;
            playerRef.GetComponent<Rigidbody2D>().AddForce(launch_direction * launch_multiplier, ForceMode2D.Impulse);

            startLaunch = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Stay on top of player, rotate to mouse if possible
        gameObject.transform.position = playerRef.transform.position;
        RotateToTarget();

        // Left click for extend toggle
        if (Input.GetMouseButtonDown(0)) {
            pistonPush = !pistonPush;
        }

        // Extend when there is no impedance
        // Impedance gets removed as player object moves
        if (pistonPush && arm_len < max_arm_len) {
            if (Physics2D.GetContacts(gameObject.GetComponent<Collider2D>(), results) == 0) {
                arm_len += extend_rate;
            }
        } else if (pistonPush) {
            // Stay extended
            arm_len = max_arm_len;
        } else if (!pistonPush && arm_len > 0) {
            // Retract when toggled to retract, no other condition
            arm_len -= extend_rate;
        } else {
            // Stay retracted
            arm_len = 0;
        }

        // Scale controls size
        gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x, arm_len, gameObject.transform.localScale.z);

        Launch();
    }
}
