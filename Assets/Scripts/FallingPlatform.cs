using System.Collections;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    //[SerializeField] [Range(0, 10)] private float fallDelay = 1f;
    //[SerializeField] [Range(0, 10)] private float destroyDelay = 2f;

    [SerializeField][Range(0, 10)] private double timeBeforeDestroyed = 5;

    private bool enabeled = true;
    private double elapsedTime = 0;

    void Start()
    {
        Vector3 pos = transform.position;
        enabled = true;
    }

    public bool getEnabeled()
    {
        return enabeled;
    }

    public void setEnabeled()
    {
        print("platform enabeled");
        enabeled = true;
        elapsedTime = 0;
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (elapsedTime > timeBeforeDestroyed)
            {
                enabeled = false;
                gameObject.SetActive(false);

                //StartCoroutine(Fall());
            }
            else
            {
                print("Elapsed Time: " + elapsedTime);
                elapsedTime += 0.02;
            }
        }
    }
}

    //private IEnumerator Fall()
    //{
    //    yield return new WaitForSeconds(fallDelay);
    //    rb.bodyType = RigidbodyType2D.Dynamic;
    //    Destroy(gameObject, destroyDelay);
    //    destroyed = true;
    //}