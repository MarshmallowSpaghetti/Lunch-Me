using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustMovement : MonoBehaviour
{
    public PlayerMoveComponent playerMoveComp;
    public ParticleSystem dust;
    private Vector3 initLocalPos;

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

        if (playerMoveComp.IsOnGround == false && dust.isPlaying)
        {
            dust.Stop();
        }
        else if (playerMoveComp.IsOnGround == true && dust.isStopped)
        {
            dust.Play();
        }
    }
}
