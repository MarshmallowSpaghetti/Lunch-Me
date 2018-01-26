using System.Collections;
using System.Collections.Generic;
using TasiYokan.Audio;
using UnityEngine;

public class GameController : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        print(AudioManager.Instance);
        new SingleAudio("BGM_LOOP").OnStart(() =>
        {
            print("hi");
        }).Play();

        int a = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
