using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabObject : MonoBehaviour
{
    [SerializeField] private Camera m_camera;

    [SerializeField]
    private Transform grabPoint;


    [SerializeField]
    private Transform rayPoint;
    [SerializeField]
    private float rayDistance;

    private Vector2 grabVec;
    private float grabz; 


    // offset Dist is magnitude of offset of grabbed object from player
    // offset is the vector of the offset

    // CHANGE offsetDist TO CHANGE HOW FAR THE OBJECT WILL BE
    private float offsetDist;
    private Vector2 offset;

    private GameObject grabbedObject;
    private GameObject player;
    private int layerIndex; 

    private Rigidbody2D grabbedObjectRb;
    // Start is called before the first frame update
    void Start()
    {
        // CHANGE offsetDist TO CHANGE HOW FAR THE OBJECT WILL BE
        offsetDist = 2.31f; // OMO radius, oriignally 1.01f
        layerIndex = LayerMask.NameToLayer("GrabbableObject");

        // required to set player tag
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hitInfo = Physics2D.Raycast(rayPoint.position, (mousePos-(Vector2)rayPoint.position), rayDistance, LayerMask.GetMask("GrabbableObject"));
        //Debug.Log(hitInfo.collider);

        if (grabbedObject != null) {
            UpdateGrabbedObject();
        } 

        if (Input.GetKeyDown(KeyCode.E) && grabbedObject != null)
        {
            ReleaseGrabbedObject();
            return; // Do not drop and grab same frame
        }

        if (hitInfo.collider != null && hitInfo.collider.gameObject.layer == layerIndex)
        {
            if (Input.GetKeyDown(KeyCode.E) && grabbedObject == null) 
            {
                grabbedObject = hitInfo.collider.gameObject;
                GrabOntoObject(mousePos, hitInfo);
                //grabbedObject.transform.SetParent(transform);
            }
            //Debug.Log("Touch");
        }
        //Debug.DrawRay(rayPoint.position, (mousePos-(Vector2)rayPoint.position).normalized * rayDistance);       
    }

    private void UpdateGrabbedObject()
    {
        Transform playerTransform = player.transform;

    
        grabVec = (Vector2)player.transform.position + offset;

        grabbedObjectRb.MovePosition(grabVec);



            //TODO: Reenable later
            //grabbedObject.transform.rotation = new Quaternion(grabbedObject.transform.rotation.x, grabbedObject.transform.rotation.y, grabz, grabbedObject.transform.rotation.w);
    }

    private void ReleaseGrabbedObject()
    {
        // grabbedObject.transform.SetParent(null);

        // Reenable gravity
        grabbedObjectRb.gravityScale = 1;


        grabbedObject = null;
    }

    private void GrabOntoObject(Vector2 mousePos, RaycastHit2D hitInfo)
    {
        
        grabbedObjectRb = grabbedObject.GetComponent<Rigidbody2D>();
        // Disable gravity
        grabbedObjectRb.gravityScale = 0;

        Transform playerTransform = player.transform;



        offset = offsetDist * (Vector2)(grabbedObject.transform.position - player.transform.position).normalized;

        
        // grabbedObject.transform.SetParent(playerTransform);
        
        grabVec = (Vector2)player.transform.position + offset;



        grabbedObjectRb.MovePosition(grabVec);

        grabbedObject.GetComponent<Rigidbody2D>().freezeRotation = true;


        // TODO: re-enable later
        // grabz = grabbedObject.transform.rotation.z;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        Debug.Log("Grabbed Object Collided");
    }
}
