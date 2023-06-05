using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateShell : MonoBehaviour
{
    private Animator animator;
    Transform m_transform;

    // Start is called before the first frame update
    void Start()
    {
        m_transform = GetComponent<Transform>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float inputX = Input.GetAxis("Horizontal");
        animator.SetFloat("turn", inputX);
        
        if (m_transform.up.y < 0) //upside down
        {
            animator.SetFloat("turn", -inputX); //reverse turns
        }
        else
        {
            animator.SetFloat("turn", inputX);
        }

    }


}


