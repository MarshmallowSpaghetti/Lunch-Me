﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    private Animator m_animator;

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

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GetAnimOffset(Vector3 _faceDir, Vector3 _motionDir)
    {
        Quaternion motionRot = Quaternion.FromToRotation(_faceDir, _motionDir);
        Vector3 offset = motionRot * Vector3.forward;
        //print("offset (" + offset.x + ", " + offset.z + ")");
        Animator.SetFloat("XOffset", offset.x);
        Animator.SetFloat("ZOffset", offset.z);
        Animator.SetFloat("Speed", _motionDir.magnitude);
    }

    public void Push()
    {
        Animator.SetBool("IsHold", false);
        Animator.Play("Push");
    }

    public void Hold()
    {
        Animator.SetBool("IsHold", true);
    }
}
