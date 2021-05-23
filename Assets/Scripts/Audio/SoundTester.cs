using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundTester : MonoBehaviour
{
    //private int testSounds;

    void Update()
    {
        if (Input.GetKeyDown("h"))
        {
            AudioManager.instance.shouldRandomizePitch = true;
            AudioManager.instance.PlaySound("Temp1");
        }
        if (Input.GetKeyDown("j"))
        {
            AudioManager.instance.shouldRandomizePitch = true;
            AudioManager.instance.PlaySound("Temp2");
        }
        if (Input.GetKeyDown("k"))
        {
            AudioManager.instance.shouldRandomizePitch = true;
            AudioManager.instance.PlaySound("Temp3");
        }
        if (Input.GetKeyDown("l"))
        {
            AudioManager.instance.shouldRandomizePitch = true;
            AudioManager.instance.PlaySound("Temp4");
        }
    }
}
