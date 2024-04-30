using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update
    // Update is called once per frame

    public GameObject MiddleTextField;

    void Update()
    {
        if(GameManager.IsNearEmbarkableTitan && !GameManager.IsTitan)
        {
            MiddleTextField.SetActive(true);
        }
        else
        {
            MiddleTextField.SetActive(false);
        }
    }
}
