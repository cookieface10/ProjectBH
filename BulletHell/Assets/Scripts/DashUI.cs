using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashUI : MonoBehaviour
{
    public Slider DashBar;
    public GameObject dashIcon1;
    public GameObject dashIcon2;
    public GameObject dashIcon3;

    void Update()
    {
        if (GameManager.dashes >= 1)
        {
            dashIcon1.SetActive(true);
        }
        if (GameManager.dashes >= 2)
        {
            dashIcon2.SetActive(true);
        }
        if (GameManager.dashes >= 3)
        {
            dashIcon3.SetActive(true);
        }
        DashBar.value = GameManager.dashes;
    }
}
