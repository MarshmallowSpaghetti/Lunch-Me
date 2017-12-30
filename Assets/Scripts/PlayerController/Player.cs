using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Animator m_animator;
    
    public Transform holdingItem;
    
    private bool m_isAiming = false;
    private float m_startAimTime = -1;

    public float keyboardSensitivity = 5;

    public Animator Animator
    {
        get
        {
            if (m_animator == null)
                m_animator = GetComponentInChildren<Animator>();
            return m_animator;
        }

        set
        {
            m_animator = value;
        }
    }
    // Update is called once per frame
    void Update()
    {
        CheckMouseStatus();
    }

    private void CheckMouseStatus()
    {
        if (Input.GetMouseButton(0) && m_isAiming == false)
        {
            m_isAiming = true;
            m_startAimTime = Time.time;
        }
        else if (Input.GetMouseButton(0) == false && m_isAiming == true)
        {
            m_isAiming = false;
            m_startAimTime = -1;
        }

        if(m_startAimTime >= 0)
        {
            //UpdateDynamicalMaxSpeed();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if (collision.gameObject.GetComponent<PickableItem>())
        //{
        //    //print("Pick " + collision.gameObject.name);
        //    if (leftHandTrans != null && holdingItem == null)
        //    {
        //        collision.transform.position = (leftHandTrans.position + rightHandTrans.position) * 0.5f;
        //        collision.transform.SetParent(transform);
        //        collision.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        //        holdingItem = collision.transform;
        //    }
        //}
    }
}
