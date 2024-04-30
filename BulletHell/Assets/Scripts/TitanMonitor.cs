using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitanMonitor : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject monitor;
    public GameObject Camera;
    public GameObject Hatches;
    // Update is called once per frame
    void FixedUpdate()
    {
        if (GameManager.IsTitan)
        {
            //Camera.SetActive(true);
            //monitor.SetActive(true);
            Hatches.SetActive(false);
        }
        else
        {
            //Camera.SetActive(false);
            //monitor.SetActive(false);
            Hatches.SetActive(true);
        }
    }
}
