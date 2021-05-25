using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class AnimationTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //var animControl = GetComponent<Animator>();
        //animControl.SetTrigger("RollBack");
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(3.0f);

        var animControl = GetComponent<Animator>();
        animControl.SetTrigger("RollBack");
    }
}
