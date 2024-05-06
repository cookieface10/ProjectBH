using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class InvIcons : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject[] Icons = new GameObject[16];
    public bool active;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            active = !active;
            for (int i = 0; i < 16; i++)
            {
                Icons[i].SetActive(active);
            }
            
        }

        for (int i = 0; i < 16; i++)
        {
            if (GameManager.PlayerInv[i] != null)
            {
                Icons[i].GetComponent<RawImage>().color = Color.red;
            }
            else
            {
                Icons[i].GetComponent<RawImage>().color = Color.white;
            }
            
        }

    }
}
