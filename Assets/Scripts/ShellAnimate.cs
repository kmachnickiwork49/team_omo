using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellAnimate : MonoBehaviour
{
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        float inputX = Input.GetAxis("Horizontal");
        if (inputX > 0) {
            animator.SetTrigger("turnL");
            Debug.Log("left");
        }
        if (inputX < 0) {
            animator.SetTrigger("turnR");
            Debug.Log("right");

        }
    }
}
