using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Attractor : MonoBehaviour
{
    public LayerMask AttractionLayer;
    public float gravity = 10;
    [SerializeField] private float effectionRadius = 10;
    public List<Collider2D> AttractedObjects = new List<Collider2D>();
    [HideInInspector] public Transform circleTransform;

    void Awake()
    {
        circleTransform = GetComponent<Transform>();
        AttractionLayer = LayerMask.GetMask("Attractable") | LayerMask.GetMask("Player") | LayerMask.GetMask("GrabbableObject");
    }

    void Update()
    {
        SetAttractedObjects();
    }

    void FixedUpdate()
    {
        AttractObjects();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, effectionRadius);
    }

    void SetAttractedObjects()
    {
        AttractedObjects = Physics2D.OverlapCircleAll(circleTransform.position, effectionRadius, AttractionLayer).ToList();  // can be optimized
    }

    void AttractObjects()
    {
        for (int i = 0; i < AttractedObjects.Count; i++)
        {
            if (AttractedObjects[i] != null) {
                AttractedObjects[i].GetComponent<Attractable>().Attract(this);
            }
        }
    }

}

