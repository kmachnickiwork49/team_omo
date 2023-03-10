using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabObject : MonoBehaviour
{
    [SerializeField] private Camera m_camera;

    [SerializeField]
    private Transform grabPoint;

    private Vector2 grabVec;
    private float grabz; 

    [SerializeField]
    private Transform rayPoint;
    [SerializeField]
    private float rayDistance;

    private float offsetDist;

    private GameObject grabbedObject;
    private int layerIndex; 
    // Start is called before the first frame update
    void Start()
    {
        layerIndex = LayerMask.NameToLayer("GrabbableObject");
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hitInfo = Physics2D.Raycast(rayPoint.position, (mousePos-(Vector2)rayPoint.position), rayDistance, LayerMask.GetMask("GrabbableObject"));
        //Debug.Log(hitInfo.collider);

        if (grabbedObject != null) {
            grabbedObject.transform.position = new Vector3(grabVec.x + gameObject.transform.position.x, grabVec.y + gameObject.transform.position.y, grabbedObject.transform.position.z);
            grabbedObject.transform.rotation = new Quaternion(grabbedObject.transform.rotation.x, grabbedObject.transform.rotation.y, grabz, grabbedObject.transform.rotation.w);
        } 

        if (Input.GetKeyDown(KeyCode.E) && grabbedObject != null)
        {
            grabbedObject.GetComponent<Rigidbody2D>().isKinematic = false;
            //grabbedObject.transform.SetParent(null);
            grabbedObject = null;
            return; // Do not drop and grab same frame
        }

        if (hitInfo.collider != null && hitInfo.collider.gameObject.layer == layerIndex)
        {
            if (Input.GetKeyDown(KeyCode.E) && grabbedObject == null) 
            {
                grabbedObject = hitInfo.collider.gameObject;
                grabbedObject.GetComponent<Rigidbody2D>().isKinematic = true;
                //grabbedObject.transform.position = grabPoint.position;
                //grabbedObject.transform.position = hitInfo.point;
                offsetDist = Mathf.Max(1.01f, hitInfo.distance); // OMO radius
                grabVec = (Vector2)grabbedObject.transform.position - (Vector2)hitInfo.point + (Vector2)rayPoint.position + (mousePos-(Vector2)rayPoint.position).normalized * offsetDist - (Vector2)gameObject.transform.position;
                grabz = grabbedObject.transform.rotation.z;
                //grabbedObject.transform.SetParent(transform);
            }
            //Debug.Log("Touch");
        }
        //Debug.DrawRay(rayPoint.position, (mousePos-(Vector2)rayPoint.position).normalized * rayDistance);       
    }
}
