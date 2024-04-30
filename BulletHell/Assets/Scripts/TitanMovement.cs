using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TitanMovement : MonoBehaviour
{
    
    public GameObject Pilot;
    public GameObject TitanSoull;
    public GameObject PilotCamera;
    public GameObject PilotCameraHolder;

    float horizontalInput;
    float verticalInput;
    public Transform orientation;
    Vector3 moveDirection;
    public Rigidbody rb;
    




    //Animator
    private const string HATCH_OPEN = "test_hatch_open";
    Animator _animator;
    string _currentState;


    private void Start()
    {
        _animator = gameObject.GetComponent<Animator>();
    }
    private void ChangeAnimationState(string newState)
    {
        if(newState == _currentState)
        {
            return;
        }
        _animator.Play(newState);
        _currentState = newState;
    }


    private bool hasFinished = false;
    private void Update()
    {
        if(GameManager.IsTitan && !hasFinished)
        {
            ChangeAnimationState(HATCH_OPEN);
            hasFinished = true;
        }
        else if(!GameManager.IsTitan)
        {
            hasFinished = false;
        }
    }
    private void FixedUpdate()
    {

        if (Input.GetKey(KeyCode.Space))
        {
            rb.AddForce(Vector3.forward);
        }


        if (GameManager.IsTitan)
        {
            PilotCamera.transform.parent = TitanSoull.transform;
            Pilot.transform.parent = TitanSoull.transform;

            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");

            moveDirection = orientation.forward *  horizontalInput + -orientation.right * verticalInput;

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
            PilotCamera.transform.parent = PilotCameraHolder.transform;
            PilotCamera.transform.position = PilotCameraHolder.transform.position;
            PilotCamera.transform.rotation = PilotCameraHolder.transform.rotation;
        }
    }
}
