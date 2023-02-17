using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmController : MonoBehaviour
{

    GameObject playerRef;
    GameObject clawRef;

    bool pistonPush;
    Collider2D[] results;
    Vector3 target;

    //[SerializeField] float max_rot_speed = 10f;
    [SerializeField] float max_arm_len = 4f;
    float arm_len;
    [SerializeField] float extend_rate = 0.005f;
    [SerializeField] float recoil_force = 2f;

    // Start is called before the first frame update
    void Start()
    {
        playerRef = GameObject.Find("Player");
        clawRef = GameObject.Find("Claw");
        target = new Vector3(0,0,0);
        arm_len = 0;
        pistonPush = false;
        results = new Collider2D[1];
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
            float angle = Mathf.Atan2(distanceVector.y, distanceVector.x) * Mathf.Rad2Deg;
            
            //Cast a ray in the direction specified in the inspector.
            RaycastHit2D hit = Physics2D.Raycast(this.gameObject.transform.position, -distanceVector, arm_len, LayerMask.GetMask("Default"));

            // Second part of condition should not occur
            if (hit.collider == null || (hit.collider != null && hit.distance >= arm_len))
            {
                // Rotate if no impedance detected, no jump
                // Removed attempt at implementation

                //if ((gameObject.transform.rotation.z+360 < angle+90+360 + 3) && (gameObject.transform.rotation.z+360 > angle+90+360 - 3)) {
                //if (arm_len < 0.5 || (Quaternion.Angle(transform.rotation, Quaternion.AngleAxis(angle + 90, Vector3.forward)) > -10 && Quaternion.Angle(transform.rotation, Quaternion.AngleAxis(angle + 90, Vector3.forward)) < 10)) {
                    gameObject.transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);
                //}
            }
        //}
    }

    
    void OnCollisionStay2D(Collision2D collision)
    {
        
        //Debug.Log(gameObject.transform.rotation.eulerAngles);
        playerRef.GetComponent<Rigidbody2D>().AddForce((new Vector2(Mathf.Cos(Mathf.PI / -180 * (gameObject.transform.rotation.eulerAngles.z-90)), Mathf.Sin(Mathf.PI / 180 * (gameObject.transform.rotation.eulerAngles.z-90)))).normalized * recoil_force, ForceMode2D.Impulse);
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
    }
}
