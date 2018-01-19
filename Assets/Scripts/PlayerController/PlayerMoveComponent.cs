using System;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerMoveComponent : MonoBehaviour
{
    public float speed = 1;

    private Vector3 m_motion;
    private bool m_isOnGround;

    public Transform motionRing;

    private CharacterController m_charController;
    private PlayerAnim m_playerAnimController;

    public Action onHitGround;
    public Action<Vector3> onBounceFromGround;

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

    public bool IsOnGround
    {
        get
        {
            return m_isOnGround;
        }

        set
        {
            if (m_isOnGround == true && value == false)
            {
                m_isOnGround = value;
                motionRing.gameObject.SetActive(false);
            }
            else if (m_isOnGround == false && value == true)
            {
                // Prevenet player slide after landing
                //Rig.velocity = Vector3.zero;

                // Hardcoded
                motionRing.gameObject.SetActive(true);
                motionRing.forward = transform.forward;

                m_isOnGround = value;
                print("Hit on ground");
                if (onHitGround != null)
                    onHitGround();
            }
        }
    }

    private Rigidbody m_rig;

    private void FixedUpdate()
    {
        // If the character is in the air, do nothing
        //if (CharController.enabled == false)
        //    return;

        MoveWhileFaceMouse();
        //MoveInForward();

        // Apply gravity
        //CharController.Move(Rig.velocity + Physics.gravity * Time.deltaTime);
        //print("gravity " + Rig.velocity + Physics.gravity * Time.deltaTime);
    }

    private void MoveWhileFaceMouse()
    {
        m_motion = new Vector3(
            CrossPlatformInputManager.GetAxis("Horizontal"),
            0,
            CrossPlatformInputManager.GetAxis("Vertical"));

        motionRing.position = transform.position;
        if (m_motion.magnitude > 0.01f)
            motionRing.forward =
                Vector3.Slerp(motionRing.forward,
                m_motion.normalized,
                0.2f);

        //CharController.Move(m_motion * speed);

        float yVel = Rig.velocity.y;
        if (IsOnGround)// && CheckGroundInRange(1.4f))
            Rig.velocity = (m_motion * speed / Time.fixedDeltaTime).SetY(yVel);

        Rig.velocity += Physics.gravity * Time.fixedDeltaTime;

        // Always keep in horizontal plane
        transform.forward =
            Vector3.Slerp(transform.forward,
            (MouseInput.Instance.MousePos - transform.position).SetY(0), 0.2f);
        // Without lerp
        //transform.forward = (MouseInput.Instance.MousePos - transform.position).SetY(0);

        PlayerAnimController.GetAnimOffset(transform.forward, m_motion);
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

    private void OnCollisionEnter(Collision collision)
    {
        //print("collide with " + collision.gameObject.layer);
        // We give player control only when player land on ground
        if (collision.gameObject.layer == 8)
        {
            bool isOnGroundBefore = m_isOnGround;
            IsOnGround = true;
            if (isOnGroundBefore == false)
            {
                onBounceFromGround(collision.relativeVelocity);
            }
        }
        else
        {
            Rig.AddForce(collision.impulse * 0.2f, ForceMode.Impulse);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == 8)
        {
            //print("Leave ground");
            //IsOnGround = false;
        }
    }

    private bool CheckGroundInRange(float _distance)
    {
        return Physics.Raycast(transform.position, Vector3.down, _distance, 1 << LayerMask.NameToLayer("Ground"));
    }
}