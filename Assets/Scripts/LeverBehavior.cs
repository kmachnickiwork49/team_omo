using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class LeverBehavior : MonoBehaviour
{

    public GameObject[] targets;
    [SerializeField] bool leverOn;
    private int canFlip;
    private SpriteRenderer spr_rend;
    [Header("Top slot, index 0 = off, bottom slot, index 1 = on")]
    [SerializeField] private Sprite[] my_spr;

    private void Start() {
        leverOn = false;
        spr_rend = GetComponent<SpriteRenderer>();
        spr_rend.sprite = my_spr[0];
        canFlip = 0;
    }

    private void Update() {
        if (canFlip > 0 & Input.GetKeyDown(KeyCode.E)) {          
            leverOn = !leverOn;
            if (leverOn) {
                spr_rend.sprite = my_spr[1];
                foreach (GameObject o in targets) {
                        o.SetActive(false);
                }
            } else {
                spr_rend.sprite = my_spr[0];
                foreach (GameObject o in targets) {
                        o.SetActive(true);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col) 
    {
        // Add another if statement for "box" objects or any other interactable objects that will push button
        if (col.gameObject.CompareTag("Player"))
        {
            canFlip += 1;
            
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            canFlip -= 1;
            if (canFlip < 0) {
                canFlip = 0;
                Debug.Log("lever unexpected negative player overlaps");
            }
        }
    }


}
