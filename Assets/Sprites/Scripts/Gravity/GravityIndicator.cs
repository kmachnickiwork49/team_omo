using UnityEngine;

public class GravityIndicator : MonoBehaviour
{
    Transform m_transform;
    [SerializeField] Attractable m_attractable;
    Quaternion targetRotation;

    // Start is called before the first frame update
    void Start()
    {
        m_transform = GetComponent<Transform>();
        targetRotation = Quaternion.AngleAxis(0, Vector3.forward);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_attractable.currentAttractor != null)
        {
            Vector2 dir = m_attractable.attractionDir;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            targetRotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);
        }
        else //set a default orientation 
        {
            targetRotation = Quaternion.AngleAxis(0, Vector3.forward);
        }

        StepTowardsTarget();
    }

    void StepTowardsTarget()
    {
        Quaternion currRotation = m_transform.rotation;
        m_transform.rotation = Quaternion.Lerp(currRotation, targetRotation, Time.deltaTime * 20);
    }


}
