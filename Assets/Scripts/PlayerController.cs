using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private CircleCollider2D circle;
    public bool grounded;
    private Animator animator;
    public float health;

    [SerializeField] float speed = 5f;
    [SerializeField] float maxSpeed = 3f;
    [SerializeField] float jumpHeight = 5f;
    [SerializeField] public float maxHealth = 10f;

    [SerializeField] float knockbackX = 5f;
    [SerializeField] float knockbackY = 4f;

    [Header("Camera")]
    Vector3 cameraPos;
    [SerializeField] Camera mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        circle = GetComponent<CircleCollider2D>();
        animator = GetComponent<Animator>();
        grounded = true;
        health = maxHealth;

        mainCamera = Camera.main;
        if (mainCamera)
        {
            cameraPos = mainCamera.transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            Debug.Log("dead");
            Death();
            //TODO: reset level or gameover scene
        }

        float inputX = Input.GetAxis("Horizontal");
        //rb.velocity = new Vector2(inputX * speed, rb.velocity.y);
        if ((inputX < 0 && -maxSpeed < rb.velocity.x) || (inputX > 0 && rb.velocity.x < maxSpeed)) {
            rb.AddForce(new Vector2(inputX*speed,0));
        }
        if((Input.GetKeyDown(KeyCode.Space) || Input.GetKey(KeyCode.W)) && grounded){          
            Jump();
        }
        if (mainCamera)
        {
            mainCamera.transform.position = new Vector3(transform.position.x, transform.position.y, cameraPos.z);
            // No y movement ->
            //mainCamera.transform.position = new Vector3(transform.position.x, cameraPos.y, cameraPos.z);
        }
    }

    public void Jump() {
        /*
        RaycastHit2D[] my_r = Physics2D.RaycastAll(gameObject.transform.position, -Vector2.up, 1.0f); // Radius + forgiveness as last argument
        foreach (RaycastHit2D x in my_r) {
            if (x.collider.gameObject.tag == "Ground") {
                //grounded = true;
                rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
                break;
            }
        }
        */
        rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
        grounded = false;
    }

    public void Death() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Ground") {
            Debug.Log("ground");

            grounded = true;
        }

        if (collision.gameObject.tag == "Hazard") {
            //TODO: can modify this so each hazard has its own script that defines how much damage it deals
            //here, access collision object's script for a "damage" variable and subtract from health
            health--;

            int dir = collision.gameObject.GetComponent<Transform>().position.x > rb.position.x ? -1 : 1;
            rb.velocity = new Vector2(knockbackX*dir, knockbackY);
            // animator.SetBool("hit", true);
        }
    }

    void OnCollisionExit2D(Collision2D collision) {
        if(collision.gameObject.tag == "Ground") {
            grounded = false;
        }
    }
/**/

}
