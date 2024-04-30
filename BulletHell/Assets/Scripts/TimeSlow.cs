using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSlow : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z)){
            Time.timeScale = 0.1f;
            //Debug.Log("Timescale, down");
        }

        if (Input.GetKeyUp(KeyCode.Z)) {

            Time.timeScale = 1.0f;
            //Debug.Log("Timescale, up");
        }
    }
}
