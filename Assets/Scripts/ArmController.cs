using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmController : MonoBehaviour
{

    GameObject playerRef;
    GameObject clawRef;

    bool pistonPush;
    Vector3 target;

    [SerializeField] float max_rot_speed = 10f;
    [SerializeField] float max_arm_len = 4f;
    float arm_len;
    [SerializeField] float extend_rate = 0.005f;

    // Start is called before the first frame update
    void Start()
    {
        playerRef = GameObject.Find("Player");
        clawRef = GameObject.Find("Claw");
        target = new Vector3(0,0,0);
        arm_len = 0;
        pistonPush = false;
    }

    void RotateToTarget()
    {
        Vector2 distanceVector = gameObject.transform.position - target;
        float angle = Mathf.Atan2(distanceVector.y, distanceVector.x) * Mathf.Rad2Deg;
        gameObject.transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = playerRef.transform.position;
        RotateToTarget();

        // Target follows user cursor, but stay within range of OMO
        Vector3 mousePos = Input.mousePosition;
        target = Camera.main.ScreenToWorldPoint(mousePos);
        target = new Vector3 (target.x, target.y, 0); 
        // For some reason, default -10 z
        // Need to keep track of z for visual hierarchy within scene, for now, 0

        if (Input.GetMouseButtonDown(0)) {
            // Left click for extend toggle
            pistonPush = !pistonPush;
        }

        if (pistonPush && arm_len < max_arm_len) {
            arm_len += extend_rate;
        } else if (pistonPush && arm_len >= max_arm_len) {
            pistonPush = false;
        }

        gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x, arm_len, gameObject.transform.localScale.z);
    }
}
