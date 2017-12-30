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

    private ProjectileComponent m_projectComp;

    public Transform launchTrans;

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

    public ProjectileComponent ProjectComp
    {
        get
        {
            if (m_projectComp == null)
                m_projectComp = GetComponent<ProjectileComponent>();
            return m_projectComp;
        }

        set
        {
            m_projectComp = value;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckMouseStatus();

        ProjectComp.SetTargetWithAngle(MouseInput.Instance.MousePos, 45);
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

            ProjectComp.Launch();
        }

        if(m_startAimTime >= 0)
        {
            //UpdateDynamicalMaxSpeed();
        }
    }

    //private void OnControllerColliderHit(Collision collision)
    //{
    //    print("on colision enter " + collision.gameObject.name);
    //    if (collision.gameObject.name == "Sphere")
    //    {
    //        //print("Pick " + collision.gameObject.name);
    //        if (holdingItem == null)
    //        {
    //            collision.transform.position = (transform.position + Vector3.up);
    //            collision.transform.SetParent(transform);
    //            collision.gameObject.GetComponent<Rigidbody>().isKinematic = true;
    //            holdingItem = collision.transform;
    //        }
    //    }
    //}

    private void OnControllerColliderHit(ControllerColliderHit collision)
    {
        print("on colision enter " + collision.gameObject.name);
        if (collision.gameObject.name == "Sphere")
        {
            //print("Pick " + collision.gameObject.name);
            if (holdingItem == null)
            {
                collision.transform.position = (launchTrans.position);
                collision.transform.SetParent(transform);
                collision.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                holdingItem = collision.transform;
            }
        }
    }
}
