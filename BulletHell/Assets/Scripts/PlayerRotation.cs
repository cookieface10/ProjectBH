using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotation : MonoBehaviour
{

    public float sensX = 0.01f;
    public float sensY;
    float mouseX;
    float mouseY;
    float yRotation;

    RaycastHit hit;

    bool hasDone = false;
    void Update()
    {
        if (!GameManager.IsTitan)
        {
            mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
            mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;
            yRotation += mouseX;
            transform.rotation = Quaternion.Euler(0, yRotation, 0); 
            hasDone = false;
        }
        else if(!hasDone)
        {

            transform.rotation = GameManager.TitanRotation;
            hasDone = true;
        }
    }
}
