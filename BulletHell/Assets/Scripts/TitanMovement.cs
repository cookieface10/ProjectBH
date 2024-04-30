using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TitanMovement : MonoBehaviour
{
    
    public GameObject Pilot;
    public GameObject TitanSoul;
    float horizontalInput;
    float verticalInput;
    public Transform orientation;
    Vector3 moveDirection;
    public Rigidbody rb;

    private void FixedUpdate()
    {
        if (GameManager.IsTitan)
        {
            Pilot.transform.parent = TitanSoul.transform;
            

            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");

            moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

            rb.AddForce(moveDirection.normalized * 1000f, ForceMode.Force);


            /*
            if (Input.GetKey(KeyCode.Space) && canJump == true && grounded == true)
            {
                canJump = false;

                JumpTime = true;

            }
            */
        }
        else
        {
            Pilot.transform.parent = null;
        }
    }
}
