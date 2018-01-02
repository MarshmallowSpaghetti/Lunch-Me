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

    private ProjectileController m_projectileCtrl;

    public Transform launchTrans;

    private bool m_isInAir;

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

    public ProjectileController ProjectCtrl
    {
        get
        {
            if (m_projectileCtrl == null)
                m_projectileCtrl = GetComponent<ProjectileController>();
            return m_projectileCtrl;
        }

        set
        {
            m_projectileCtrl = value;
        }
    }

    public bool IsInAir
    {
        get
        {
            return m_isInAir;
        }

        set
        {
            m_isInAir = value;
            if(m_isInAir)
            {
                //GetComponent<BoxCollider>().enabled = true;
                //GetComponent<CharacterController>().enabled = false;
                //GetComponent<Rigidbody>().useGravity = true;

                // Hardcoded
                GetComponent<PlayerMoveComponent>().motionRing.gameObject.SetActive(false);
            }
            else
            {
                //GetComponent<BoxCollider>().enabled = false;
                //GetComponent<CharacterController>().enabled = true;
                //GetComponent<Rigidbody>().useGravity = false;

                //// Prevenet player slide after landing
                //GetComponent<Rigidbody>().velocity = Vector3.zero;

                // Hardcoded
                GetComponent<PlayerMoveComponent>().motionRing.gameObject.SetActive(true);
                GetComponent<PlayerMoveComponent>().motionRing.forward = transform.forward;
            }
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

            ProjectCtrl.SetEnable(true);
            ProjectCtrl.StartCharge();
        }
        else if (Input.GetMouseButton(0) == false && m_isAiming == true)
        {
            m_isAiming = false;
            m_startAimTime = -1;

            ProjectCtrl.SetEnable(false);
            ProjectCtrl.StopChargeAndLaunch();
        }

        if(m_startAimTime >= 0)
        {
            ProjectCtrl.SetLaunchChargeTime(Time.time - m_startAimTime);
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit collision)
    {
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

    private void OnCollisionEnter(Collision collision)
    {
        print("collide with " + collision);
        IsInAir = false;
    }
}
