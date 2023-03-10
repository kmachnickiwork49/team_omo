using UnityEngine;

public class Tutorial_GrapplingRope : MonoBehaviour
{
    [Header("Arm len:")]
    public float armLen = 8.0f;

    [Header("General Refernces:")]
    public Tutorial_GrapplingGun grapplingGun;
    public LineRenderer m_lineRenderer;

    [Header("General Settings:")]
    [SerializeField] private int percision = 40;
    /*[Range(0, 20)]*/[SerializeField] private float straightenLineSpeed = 5;

    [Header("Rope Animation Settings:")]
    public AnimationCurve ropeAnimationCurve;
    /*[Range(0.01f, 4)]*/[SerializeField] private float StartWaveSize = 0;
    float waveSize = 0;

    [Header("Rope Progression:")]
    public AnimationCurve ropeProgressionCurve;
    [SerializeField][Range(1, 50)] private float ropeProgressionSpeed = 1;

    float moveTime = 0;

    /*[HideInInspector]*/ [SerializeField] public bool isGrappling = false;

    public bool strightLine = true;

    private void OnEnable()
    {
        moveTime = 0;
        m_lineRenderer.positionCount = percision;
        waveSize = StartWaveSize;
        strightLine = false;

        LinePointsToFirePoint();

        m_lineRenderer.enabled = true;
    }

    private void OnDisable()
    {
        m_lineRenderer.enabled = false;
        isGrappling = false;
    }

    private void LinePointsToFirePoint()
    {
        for (int i = 0; i < percision; i++)
        {
            m_lineRenderer.SetPosition(i, grapplingGun.firePoint.position);
        }
    }

    private void Update()
    {
        moveTime += Time.deltaTime;
        DrawRope();
    }

    void DrawRope()
    {
        if (!strightLine)
        {
            //if (m_lineRenderer.GetPosition(percision - 1).x == grapplingGun.grapplePoint.x)
            // Floating point rounding error when out of range
            float accuracyOfGrab = 0.03f;
            if (m_lineRenderer.GetPosition(percision - 1).x <= grapplingGun.grapplePoint.x + accuracyOfGrab && m_lineRenderer.GetPosition(percision - 1).x >= grapplingGun.grapplePoint.x - accuracyOfGrab && m_lineRenderer.GetPosition(percision - 1).y <= grapplingGun.grapplePoint.y + accuracyOfGrab && m_lineRenderer.GetPosition(percision - 1).y >= grapplingGun.grapplePoint.y - accuracyOfGrab)
            {
                // Does not enter here if out of grapple range
                strightLine = true;
            }
            else
            {
                //Debug.Log("" + m_lineRenderer.GetPosition(percision - 1).x + " " + grapplingGun.grapplePoint.x);
                DrawRopeWaves();
            }
        }
        else
        {
            if (!isGrappling)
            {
                //Debug.Log("enter Grapple()");
                // Enters Grapple() once within range... but already drawn connection to point
                // Which is problematic if you aim while within range, but then exit max range
                // Bug fix --> now begins to grapple as soon as rope reaches its contact point
                grapplingGun.Grapple();
                isGrappling = true;
            }
            if (waveSize > 0)
            {
                waveSize -= Time.deltaTime * straightenLineSpeed;
                DrawRopeWaves();
            }
            else
            {
                waveSize = 0;

                if (m_lineRenderer.positionCount != 2) { m_lineRenderer.positionCount = 2; }

                DrawRopeNoWaves();
            }
        }
    }

    void DrawRopeWaves()
    {
        for (int i = 0; i < percision; i++)
        {
            float delta = (float)i / ((float)percision - 1f);
            Vector2 offset = Vector2.Perpendicular(grapplingGun.grappleDistanceVector).normalized * ropeAnimationCurve.Evaluate(delta) * waveSize;
            Vector2 targetPosition = Vector2.Lerp(grapplingGun.firePoint.position, grapplingGun.grapplePoint, delta) + offset;
            //Vector2 currentPosition = Vector2.Lerp(grapplingGun.firePoint.position, targetPosition, ropeProgressionCurve.Evaluate(moveTime) * ropeProgressionSpeed);
            // Instead of Lerp, which makes it so the grapple hook speed changes based on how far it has to go
            // Make it progress at a constant speed
            // z issue?
            Vector2 currentPosition = grapplingGun.firePoint.position +  (new Vector3(targetPosition.x, targetPosition.y, grapplingGun.firePoint.position.z) - grapplingGun.firePoint.position).normalized * armLen * (ropeProgressionSpeed * ropeProgressionCurve.Evaluate(moveTime));
            RaycastHit2D rope_contact = Physics2D.Raycast(grapplingGun.firePoint.position, (new Vector3(targetPosition.x, targetPosition.y, grapplingGun.firePoint.position.z) - grapplingGun.firePoint.position).normalized, armLen, grapplingGun.layerMask);
            
            /*
            if (rope_contact != null && Vector2.Distance(rope_contact.point, grapplingGun.firePoint.position) <= armLen) {
                // If rope sees it will hit a grabbable, stop growing in length
                currentPosition = targetPosition;
            }
            */

            m_lineRenderer.SetPosition(i, currentPosition);
        }
    }

    void DrawRopeNoWaves()
    {
        m_lineRenderer.SetPosition(0, grapplingGun.firePoint.position);
        m_lineRenderer.SetPosition(1, grapplingGun.grapplePoint);
    }
}