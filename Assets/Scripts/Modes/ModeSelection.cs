using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeSelection : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            this.gameObject.GetComponent<Movement>().enabled = true;
            this.gameObject.GetComponent<Placement>().enabled = false;
            this.gameObject.GetComponent<Destruction>().enabled = false;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            this.gameObject.GetComponent<Movement>().enabled = false;
            this.gameObject.GetComponent<Placement>().enabled = true;
            this.gameObject.GetComponent<Destruction>().enabled = false;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Alpha4))
        {
            this.gameObject.GetComponent<Movement>().enabled = false;
            this.gameObject.GetComponent<Placement>().enabled = false;
            this.gameObject.GetComponent<Destruction>().enabled = true;
        }
    }
}
