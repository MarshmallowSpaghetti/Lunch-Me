using System;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(CharacterController))]
class PlayerMoveComponent : MonoBehaviour
{
    public float speed = 1;

    private Vector3 m_motion;

    private CharacterController m_charController;

    public CharacterController CharController
    {
        get
        {
            if (m_charController == null)
                m_charController = GetComponent<CharacterController>();
            return m_charController;
        }

        set
        {
            m_charController = value;
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

        set
        {
            m_rig = value;
        }
    }

    private Rigidbody m_rig;

    private void Update()
    {
        // If the character is in the air, do nothing
        if (CharController.enabled == false)
            return;

        MoveWhileFaceMouse();
        //MoveInForward();

        // Apply gravity
        //CharController.Move(Physics.gravity.normalized * speed);
        CharController.Move(Rig.velocity + Physics.gravity * Time.deltaTime);
        //print("gravity " + Rig.velocity + Physics.gravity * Time.deltaTime);
    }

    private void MoveWhileFaceMouse()
    {
        m_motion = new Vector3(
            CrossPlatformInputManager.GetAxis("Horizontal"),
            0,
            CrossPlatformInputManager.GetAxis("Vertical"));

        CharController.Move(m_motion * speed);
        // Always keep in horizontal plane
        transform.forward =
            Vector3.Slerp(transform.forward,
            (MouseInput.Instance.MousePos - transform.position).SetY(0), 0.1f);
        // Without lerp
        //transform.forward = (MouseInput.Instance.MousePos - transform.position).SetY(0);
    }

    private void MoveInForward()
    {
        m_motion = new Vector3(
            CrossPlatformInputManager.GetAxis("Horizontal"),
            0,
            CrossPlatformInputManager.GetAxis("Vertical"));

        float angleInAFrame = 300 * Time.deltaTime;
        if (Vector3.Angle(transform.forward, m_motion.SetY(0)) < angleInAFrame * 2)
        {
            CharController.Move(m_motion * speed);
            transform.forward = m_motion.normalized;
            GizmosHelper.DrawLine(transform.position, transform.position + m_motion * 10, Color.blue);
        }
        else if (m_motion.SetY(0).sqrMagnitude.Sgn() > 0)
        {
            transform.Rotate(Vector3.up, Vector3.SignedAngle(transform.forward, m_motion, Vector3.up).Sgn() * angleInAFrame);
        }
    }
}