﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

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
    private PlayerMoveComponent m_playerMoveComp;
    private Vector3 m_nextLandingPoint = new Vector3(0, -999, 0);

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

    public PlayerMoveComponent PlayerMoveComp
    {
        get
        {
            if (m_playerMoveComp == null)
            {
                m_playerMoveComp = GetComponent<PlayerMoveComponent>();
            }
            return m_playerMoveComp;
        }
    }

    private void Awake()
    {
        //PlayerMoveComp.onHitGround += () =>
        //{
        //    print("Jump again");
        //    PlayerMoveComp.IsOnGround = false;
        //    //Rig.velocity = Vector3.up * 15f;
        //};
        PlayerMoveComp.onBounceFromGround += (velocity) =>
        {
            if (m_nextLandingPoint.y > -990)
            {
                ProjectCtrl.SetTargetWithAngle(m_nextLandingPoint);
                ProjectCtrl.Launch();
                m_nextLandingPoint = new Vector3(0, -999, 0);
                return;
            }
            // Bounce threadhold
            if (velocity.y < 3f)
                return;

            Rig.velocity = velocity.SetX(0).SetZ(0) * 0f;
            PlayerMoveComp.IsOnGround = false;
        };
    }

    // Update is called once per frame
    void Update()
    {
        CheckBounce(1, 3f);
    }

    private void CheckMouseStatus_HoldToLaunch()
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

    private void CheckMouseStatus_ClickToSelect()
    {
        //if (Input.GetMouseButton(0) && m_isAiming == false)
        //{
        //    //ProjectCtrl.SetEnable(true);
        //    m_isAiming = true;
        //}
        //else if (Input.GetMouseButton(0) == false && m_isAiming == true)
        //{
        //    m_isAiming = false;
        //    //ProjectCtrl.SetEnable(false);
        //    //ProjectCtrl.DirectlyLaunch();
        //    m_nextLandingPoint = MouseInput.Instance.MousePos;
        //}
        ProjectCtrl.SetEnable(true);
        if (Input.GetMouseButtonUp(0))
        {
            m_nextLandingPoint = MouseInput.Instance.MousePos;
            //Rig.velocity = Vector3.down * 10;
            ProjectCtrl.SetTargetWithAngle(m_nextLandingPoint);
            ProjectCtrl.Launch();
            m_nextLandingPoint = new Vector3(0, -999, 0);
        }
    }

    private void CheckBounce(float _velocityThreshold, float _distanceThreshold)
    {
        // Close enough to ground
        //if(PlayerMoveComp.IsOnGround
        //    || Physics.Raycast(transform.position, Vector3.down, _distanceThreshold, 1 << LayerMask.NameToLayer("Ground")))
        //{
        //    if (Mathf.Abs(Rig.velocity.y) > _velocityThreshold)
        //    {
        //        ProjectCtrl.Type = ProjectileController.LaunchType.useInitialAngle;
        //        CheckMouseStatus_ClickToSelect();
        //    }
        //    else
        //    {
        //        ProjectCtrl.Type = ProjectileController.LaunchType.useBothAngleAndSpeed;
        //        CheckMouseStatus_HoldToLaunch();
        //    }
        //}
        //else
        //{
        //    m_isAiming = false;
        //    ProjectCtrl.SetEnable(false);
        //}

        if (PlayerMoveComp.IsOnGround)
        {
            //ProjectCtrl.Type = ProjectileController.LaunchType.useBothAngleAndSpeed;
            //CheckMouseStatus_HoldToLaunch();
            ProjectCtrl.Type = ProjectileController.LaunchType.useInitialAngle;
            CheckMouseStatus_ClickToSelect();
        }
        else
        {
            ProjectCtrl.Type = ProjectileController.LaunchType.useInitialAngle;
            CheckMouseStatus_ClickToSelect();
        }
    }
}
