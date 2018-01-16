using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustMovement : MonoBehaviour
{
    public PlayerMoveComponent playerMoveComp;
    public ParticleSystem dust;
    public ParticleSystem impactDust;
    private Vector3 initLocalPos;

    // TODO: maybe move to another script?
    public TrailRenderer trailInAir;

    // Use this for initialization
    void Start()
    {
        initLocalPos = transform.localPosition;
        playerMoveComp.onHitGround += () =>
        {
            if (impactDust != null)
            {
                impactDust.transform.position = transform.parent.position + new Vector3(0, -0.7f, 0);
                impactDust.Play();
            }
        };
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
