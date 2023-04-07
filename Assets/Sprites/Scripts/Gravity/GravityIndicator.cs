using UnityEngine;

public class GravityIndicator : MonoBehaviour
{
    Transform m_transform;
    [SerializeField] Attractable m_attractable;

    // Start is called before the first frame update
    void Start()
    {
        m_transform = GetComponent<Transform>();
        //m_attractable = GetComponent<Attractable>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_attractable.currentAttractor != null)
        {
            Vector2 dir = m_attractable.attractionDir;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            m_transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);
        }
    }
}
