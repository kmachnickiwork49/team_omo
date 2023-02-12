using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private CircleCollider2D circle
    private bool grounded;
    private Animator animator

    [SerializeField] float speed = 5f;
    [SerializeField] float jumpHeight = 5f;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        circle = GetComponent<CircleCollider2D>();
        animator = GetComponent<Animator>();
        grounded = true;
    }

    // Update is called once per frame
    void Update()
    {
        float inputX = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(inputX * speed, rb.velocity.y);
        if((Input.GetKeyDown(KeyCode.Space) || Input.GetKey(KeyCode.W)) && grounded){
            Jump();
        }
    }

    public void Jump() {
        rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Ground") {
            grounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision) {
        if(collision.gameObject.tag == "Ground") {
            grounded = false;
        }
    }
}
