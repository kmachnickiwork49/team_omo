using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arm_Scratch : MonoBehaviour
{

    [SerializeField] GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position =  new Vector3(player.transform.position.x, player.transform.position.y, gameObject.transform.position.z);
    }
}
