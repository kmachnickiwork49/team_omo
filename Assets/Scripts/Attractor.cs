using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Attractor : MonoBehaviour
{
    public List<LayerMask> AttractionLayers;
    public float gravity = 10;
    [SerializeField] private float effectionRadius = 10;
    public List<Collider2D> AttractedObjects = new List<Collider2D>();
    [HideInInspector] public Transform circleTransform;

    void Awake()
    {
        circleTransform = GetComponent<Transform>();
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
        // Can optimize, for now, just check the many layers that can be attracted
        AttractedObjects.Clear();
        foreach (LayerMask l in AttractionLayers) {
            foreach (Collider2D c in Physics2D.OverlapCircleAll(circleTransform.position, effectionRadius, l).ToList()) {
                AttractedObjects.Add(c);
            }
        }
    }

    void AttractObjects()
    {
        for (int i = 0; i < AttractedObjects.Count; i++)
        {
            AttractedObjects[i].GetComponent<Attractable>().Attract(this);
        }
    }

}
