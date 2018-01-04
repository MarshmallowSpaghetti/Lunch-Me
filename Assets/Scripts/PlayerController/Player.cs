using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerAnim m_playerAnimController;

    public Transform holdingItem;

    private bool m_isAiming = false;
    private float m_startAimTime = -1;

    public float keyboardSensitivity = 5;

    private ProjectileController m_projectileCtrl;

    public Transform launchTrans;
    
    private Rigidbody m_rig;

    public PlayerAnim PlayerAnimController
    {
        get
        {
            if (m_playerAnimController == null)
                m_playerAnimController = GetComponent<PlayerAnim>();
            return m_playerAnimController;
        }

        set
        {
            m_playerAnimController = value;
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

    public Rigidbody Rig
    {
        get
        {
            if (m_rig == null)
                m_rig = GetComponent<Rigidbody>();
            return m_rig;
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

            PlayerAnimController.Hold();
        }
        else if (Input.GetMouseButton(0) == false && m_isAiming == true)
        {
            m_isAiming = false;
            m_startAimTime = -1;

            ProjectCtrl.SetEnable(false);
            ProjectCtrl.StopChargeAndLaunch();

            PlayerAnimController.Push();
        }

        if (m_startAimTime >= 0)
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
}
