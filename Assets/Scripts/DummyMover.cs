using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyMover : MonoBehaviour
{
    public float speed = 1;
    public float duration = 1;
    public float boundaryThickness = 0.5f;
    public Transform refTarget;

    private Vector3 m_roomLeftDown;
    private Vector3 m_roomRightUp;
    private Vector3 m_targetPos;
    private Rigidbody m_rig;
    private Vector3 m_startPos;
    private Vector3 m_targetControlPos;
    private Vector3 m_thisControlPos;
    public float controlLength = 1;

    public Rigidbody Rig
    {
        get
        {
            if (m_rig == null)
                m_rig = GetComponent<Rigidbody>();
            return m_rig;
        }

        set
        {
            m_rig = value;
        }
    }


    // Use this for initialization
    void Start()
    {
        FindRoomCornerPoints(out m_roomLeftDown, out m_roomRightUp);
        //m_targetPos = AssignPosInRoom();
        if (refTarget != null)
            m_targetPos = refTarget.position;
        m_startPos = transform.position;
        m_targetControlPos =
            m_targetPos + (transform.position - m_targetPos + Vector3.left * 50).SetY(0).normalized * controlLength;
        m_thisControlPos =
            transform.position + (m_targetPos - transform.position + Vector3.left * 50).SetY(0).normalized * controlLength;

        StartCoroutine(WalkBezierCurveTo());
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(m_targetPos, 0.5f);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(m_thisControlPos, 0.5f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(m_targetControlPos, 0.5f);

        Gizmos.color = Color.white;
        Gizmos.DrawLine(m_startPos, m_thisControlPos);
        Gizmos.DrawLine(m_targetPos, m_targetControlPos);
    }

    IEnumerator WalkStraightTo()
    {
        while (!CloseEnoughTo(m_targetPos))
        {
            Rig.velocity = GetTargetDir() * speed;
            yield return null;
        }

        print("Reach dst");
    }

    IEnumerator WalkBezierCurveTo()
    {
        float startTime = Time.time;
        while (!CloseEnoughTo(m_targetPos))
        {
            //print("percent " + (Time.time - startTime) / duration);
            //transform.position = CalculateCubicBezierPos(
            //    (Time.time - startTime) / duration);
            Vector3 dir = CalculateCubicBezierPos(
                (Time.time - startTime) / duration) - transform.position;
            Rig.velocity = dir.normalized * speed;
            yield return null;
        }

        print("Reach dst");
    }

    private bool CloseEnoughTo(Vector3 _targetPos)
    {
        return (transform.position - _targetPos).SetY(0).sqrMagnitude < boundaryThickness * 2;

    }

    /// <summary>
    /// Return normalized direction from this to target
    /// </summary>
    /// <returns></returns>
    private Vector3 GetTargetDir()
    {
        return (m_targetPos - transform.position).SetY(0).normalized;
    }

    private Vector3 CalculateCubicBezierPos(float _t)
    {
        float u = 1 - _t;
        float t2 = _t * _t;
        float u2 = u * u;
        float u3 = u2 * u;
        float t3 = t2 * _t;

        Vector3 p = u3 * m_startPos
            + t3 * m_targetPos
            + 3 * u2 * _t * m_thisControlPos
            + 3 * u * t2 * m_targetControlPos;

        return p;
    }

    private Vector3 AssignPosInRoom()
    {
        //print("new target pos");
        Vector3 targetPos = new Vector3(
            Random.Range(m_roomLeftDown.x, m_roomRightUp.x), 1.5f, Random.Range(m_roomLeftDown.y, m_roomRightUp.y));
        return targetPos;
    }

    private void FindRoomCornerPoints(out Vector3 leftDown, out Vector3 rightUp)
    {
        Vector3 startPos = transform.position + Vector3.up * 0.5f;
        RaycastHit hitInfoLeft;
        Physics.Raycast(startPos, Vector3.left, out hitInfoLeft, 100, 1 << LayerMask.NameToLayer("Ground"));
        Debug.DrawLine(startPos, hitInfoLeft.transform.position, Color.red);

        RaycastHit hitInfoRight;
        Physics.Raycast(startPos, Vector3.right, out hitInfoRight, 100, 1 << LayerMask.NameToLayer("Ground"));
        Debug.DrawLine(startPos, hitInfoRight.transform.position, Color.red);

        RaycastHit hitInfoFoward;
        Physics.Raycast(startPos, Vector3.forward, out hitInfoFoward, 100, 1 << LayerMask.NameToLayer("Ground"));
        Debug.DrawLine(startPos, hitInfoFoward.transform.position, Color.red);

        RaycastHit hitInfoBackward;
        Physics.Raycast(startPos, Vector3.back, out hitInfoBackward, 100, 1 << LayerMask.NameToLayer("Ground"));
        Debug.DrawLine(startPos, hitInfoBackward.transform.position, Color.red);

        leftDown = new Vector3(hitInfoLeft.transform.position.x + boundaryThickness, hitInfoBackward.transform.position.z + boundaryThickness);
        rightUp = new Vector3(hitInfoRight.transform.position.x - boundaryThickness, hitInfoFoward.transform.position.z - boundaryThickness);
        return;
    }
}
