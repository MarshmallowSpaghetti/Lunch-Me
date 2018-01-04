using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustMovement : MonoBehaviour
{
    public PlayerMoveComponent playerMoveComp;
    public ParticleSystem dust;
    private Vector3 initLocalPos;

    // TODO: maybe move to another script?
    public TrailRenderer trailInAir;

    // Use this for initialization
    void Start()
    {
        initLocalPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = initLocalPos 
            + Quaternion.Inverse(playerMoveComp.transform.rotation) 
                * playerMoveComp.Rig.velocity.normalized * -playerMoveComp.Rig.velocity.magnitude * 0.1f;

        if (playerMoveComp.IsOnGround == false && dust.isStopped == false)
        {
            dust.Stop();
            trailInAir.enabled = true;
        }
        else if (playerMoveComp.IsOnGround == true && dust.isPlaying == false)
        {
            dust.Play();
            trailInAir.enabled = false;
        }
    }
}
