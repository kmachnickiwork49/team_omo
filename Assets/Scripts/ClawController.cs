using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Transform))]

public class ClawController : MonoBehaviour
{

    Vector3 target;
    //bool pistonPush;
    GameObject arm;

    // Start is called before the first frame update
    void Start()
    {
        target = new Vector3(0,0,0);
        //pistonPush = false;
        arm = GameObject.Find("Arm");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
