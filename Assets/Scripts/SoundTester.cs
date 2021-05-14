using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundTester : MonoBehaviour
{
    private int testSounds;

    void Update()
    {
        if (Input.GetKeyDown("j"))
        {
            AudioManager.instance.shouldRandomizePitch = true;
            testSounds = Random.Range(0, 1);
            if (testSounds == 0)
            {
                AudioManager.instance.PlaySound("Falling1");
            }
            else if (testSounds == 1)
            {
                AudioManager.instance.PlaySound("Falling2");
            }
            else if (testSounds == 2)
            {
                AudioManager.instance.PlaySound("Falling3");
            }
            else
            {
                return;
            }
        }
        if (Input.GetKeyDown("k"))
        {
            AudioManager.instance.shouldRandomizePitch = true;
            testSounds = Random.Range(0, 1);
            if (testSounds == 0)
            {
                AudioManager.instance.PlaySound("Falling2");
            }
            else if (testSounds == 1)
            {
                AudioManager.instance.PlaySound("Falling2");
            }
            else if (testSounds == 2)
            {
                AudioManager.instance.PlaySound("Falling3");
            }
            else
            {
                return;
            }
        }
        if (Input.GetKeyDown("l"))
        {
            AudioManager.instance.shouldRandomizePitch = true;
            testSounds = Random.Range(0, 1);
            if (testSounds == 0)
            {
                AudioManager.instance.PlaySound("Falling3");
            }
            else if (testSounds == 1)
            {
                AudioManager.instance.PlaySound("Falling2");
            }
            else if (testSounds == 2)
            {
                AudioManager.instance.PlaySound("Falling3");
            }
            else
            {
                return;
            }
        }
    }
}
