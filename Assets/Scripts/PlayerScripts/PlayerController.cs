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
    [SerializeField] public bool grounded;
    [SerializeField] public bool allowJumpFromGrabbableObjects;
    private Animator animator;
    public float health;

    [SerializeField] float speed = 5f;
    [SerializeField] float maxSpeed = 3f;
    [SerializeField] float jumpHeight = 5f;
    [SerializeField] public float maxHealth = 10f;

    [SerializeField] float knockbackX = 5f;
    [SerializeField] float knockbackY = 4f;

    private int grabIndex;

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
        allowJumpFromGrabbableObjects = false; // Change this if you want omo to jump from objects that do not have the ground tag
        grabIndex = LayerMask.NameToLayer("GrabbableObject");

        health = maxHealth;

        mainCamera = Camera.main;
        // set rigidbody gravityscale to 1
        rb.gravityScale = 1f;
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
        if (collision.gameObject.tag == "Ground" || (allowJumpFromGrabbableObjects && collision.gameObject.layer == grabIndex)) {
            Debug.Log("ground");

            grounded = true;
        }

        if (collision.gameObject.tag == "Hazard") {
            //TODO: can modify this so each hazard has its own script that defines how much damage it deals
            //here, access collision object's script for a "damage" variable and subtract from health
            health = health - 3;
            int dir = collision.gameObject.GetComponent<Transform>().position.x > rb.position.x ? -1 : 1;
            rb.velocity = new Vector2(knockbackX*dir, knockbackY);

            SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
            SpriteRenderer shell = (GameObject.Find("Shell")).GetComponent<SpriteRenderer>();

            Color newColor = new Color32(255, 66, 66, 255);
            StartCoroutine(DamageEffectSequence(sr, newColor, 1, 1));
            StartCoroutine(DamageEffectSequence(shell, newColor, 1, 1));
        }
    }

    //turn red after damage
    //credit: https://stackoverflow.com/questions/59958594/is-there-a-way-to-change-sprite-color-temporarily-after-collision-with-c-sharp-i
    IEnumerator DamageEffectSequence(SpriteRenderer sr, Color dmgColor, float duration, float delay)
    {
        // save origin color
        Color originColor = sr.color;

        // tint the sprite with damage color
        sr.color = dmgColor;

        // you can delay the animation
        yield return new WaitForSeconds(delay/7);

        // lerp animation with given duration in seconds
        for (float t = 0; t < 1.0f; t += Time.deltaTime*10/duration)
        {
            sr.color = Color.Lerp(dmgColor, originColor , t);

            yield return null;
        }

        // restore origin color
        sr.color = originColor;
    }

    // DISABLED THIS SCRIPT TO FIX GROUNDING BUG WITH GRABBABLE OBJECTS

    // void OnCollisionExit2D(Collision2D collision) {
    //     if(collision.gameObject.tag == "Ground") {
    //         grounded = false;
    //     }
    // }
/**/

}
