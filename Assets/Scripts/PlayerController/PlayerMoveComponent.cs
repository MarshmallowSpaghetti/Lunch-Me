using System;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(CharacterController))]
class PlayerMoveComponent : MonoBehaviour
{
    public float speed = 1;

    private Vector3 m_motion;
    private Vector3 m_lookPos;
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

    private void Update()
    {
        UpdateMouseHitPos();
        MoveWhileFaceMouse();
        //MoveInForward();

        // Apply gravity
        CharController.Move(Physics.gravity.normalized * speed);
    }

    private void UpdateMouseHitPos()
    {
        Ray ray = Camera.main.ScreenPointToRay(CrossPlatformInputManager.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, float.MaxValue, 1 << LayerMask.NameToLayer("Ground")))
        {
            m_lookPos = hit.point;
        }
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
            (m_lookPos - transform.position).SetY(0), 0.1f);
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