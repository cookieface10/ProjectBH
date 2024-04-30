using GLTF.Schema;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class PlayerMovment : MonoBehaviour
{
    public CapsuleCollider capsule;
    public CapsuleCollider capsuleBut2;
    public GameObject CameraHolder;
    public GameObject Camera;

    [Header("Movement")]
    private float moveSpeed;
    public float walkSpeed;

    
    public float dashSpeed;
    public float dashSpeedChangeFactor;

    public float wallrunSpeed;

    public float maxYSpeed;

    public float groundDrag;


    [Header("Jump")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool canJump;
    bool JumpTime;

    [Header("Ground Check")]
    public float playerheight = 0;
    public LayerMask whatIsGround;
    bool grounded;

    [Header("Titan Check")]
    public GameObject titanObj;

    [Header("Slope Handeling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;


    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    public MovementState state;

    
    public enum MovementState
    {
        walking,
        air,
        wallrunning,
        dashing

    }
    public bool dashing;
    public bool wallrunning;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

    }

    private void MyInput()
    {
        if (!GameManager.IsTitan)
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");

            if (Input.GetKey(KeyCode.Space) && canJump == true && grounded == true)
            {
                canJump = false;

                JumpTime = true;

            }

            if (Input.GetKey(KeyCode.V))
            {
                Can_Titanfall();
            }
        }
        

    }

    private void Can_Titanfall()
    {
        Vector3 _orientation = CameraHolder.transform.rotation.ToEuler();
        
        RaycastHit hit;
        Ray landingRay = new Ray(CameraHolder.transform.position, _orientation);
        if(Physics.Raycast(landingRay, out hit))
        {
                Debug.Log("It found something!");
        }
    }


    private float desiredMoveSpeed;
    private float lastDesiredMoveSpeed;
    private MovementState lastState;
    private bool keepMomentum;
    private void StateHandler()
    {
        if (wallrunning)
        {
            state = MovementState.wallrunning;
            desiredMoveSpeed = wallrunSpeed;
        }
        if (dashing)
        {
            state = MovementState.dashing;
            desiredMoveSpeed = dashSpeed;
            speedChangeFactor = dashSpeedChangeFactor;
        }
        else if (grounded)
        {
            state = MovementState.walking;
            desiredMoveSpeed = walkSpeed;
        }
        else
        {
            state = MovementState.air;

            desiredMoveSpeed = walkSpeed;
        }
        bool desiredMoveSpeedHasChanged = desiredMoveSpeed != lastDesiredMoveSpeed;
        if (lastState == MovementState.dashing) keepMomentum = true;

        if (desiredMoveSpeedHasChanged)
        {
            if (keepMomentum)
            {
                StopAllCoroutines();
                StartCoroutine(SmoothlyLerpMoveSpeed());
            }
            else
            {
                StopAllCoroutines();
                moveSpeed = desiredMoveSpeed;
            }
        }

        lastDesiredMoveSpeed = desiredMoveSpeed;
        lastState = state;
    }

    private float speedChangeFactor;
    private IEnumerator SmoothlyLerpMoveSpeed()
    {
        float time = 0;
        float difference = Mathf.Abs(desiredMoveSpeed - moveSpeed);
        float startValue = moveSpeed;

        float boostFactor = speedChangeFactor;

        while (time < difference)
        {
            moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time / difference);

            time += Time.deltaTime * boostFactor;

            yield return null;
        }

        moveSpeed = desiredMoveSpeed;
        speedChangeFactor = 1f;
        keepMomentum = false;
    }
    private void MovePlayer()
    {
        if (state == MovementState.dashing) return;

        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 20f, ForceMode.Force);

            if (rb.velocity.y > 0)
            {
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }

        }
        if (grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }

        if (!grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
        if(!wallrunning) rb.useGravity = !OnSlope();
    }
    private void SpeedControl()
    {
        if (OnSlope() && !exitingSlope)
        {
            if(rb.velocity.magnitude > moveSpeed)
            {
                rb.velocity = rb.velocity.normalized * moveSpeed;
            }
        }
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }

        if(maxYSpeed != 0 && rb.velocity.y > maxYSpeed)
        {
            rb.velocity = new Vector3(rb.velocity.x, maxYSpeed, rb.velocity.z);
        }
    }
    private void ResetJump()
    {
        canJump = true;

        exitingSlope = false;
    }
    private bool OnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerheight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }
        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }
    
    private bool hasDone = false;
    private void Update()
    {

        if (GameManager.IsTitan && !hasDone)
        {
            rb.position = GameManager.TitanLocation;
            CameraHolder.transform.position = GameManager.TitanLocation;
            CameraHolder.transform.rotation = GameManager.TitanRotation;
            hasDone = true;
        }

        //if (GameManager.IsTitan)
        //{
        //    rb.position = GameManager.TitanLocation;
        //    this.gameObject.transform.rotation = GameManager.TitanRotation;
        //}




        if (!GameManager.IsTitan)
        {
            grounded = Physics.Raycast(transform.position, Vector3.down, playerheight * 0.5f + 0.05f, whatIsGround);

            MyInput();
            SpeedControl();
            StateHandler();

            if (state == MovementState.walking && !GameManager.IsTitan)
            {
                rb.drag = groundDrag;
            }
            else
            {
                rb.drag = 0;
            }
            if (grounded)
            {
                ResetJump();
            }
            Debug.Log("grounded = " + grounded);
            Debug.Log("dashing = " + dashing);
        }
        
        
    }

    
    private void FixedUpdate()
    {
        GameManager.IsNearEmbarkableTitan = (titanObj.transform.position - rb.position).magnitude < 10;

        if (Input.GetKeyDown(KeyCode.E) && GameManager.IsNearEmbarkableTitan)
        {

            GameManager.IsTitan = !GameManager.IsTitan;
            rb.velocity = Vector3.zero;

            
        }
        if (GameManager.IsTitan)
        {
            
            capsuleBut2.enabled = false;
            capsule.enabled = false;
            
            this.gameObject.transform.rotation = GameManager.TitanRotation;
            rb.useGravity = false;
            rb.velocity = Vector3.zero;
            rb.interpolation = RigidbodyInterpolation.None;
            rb.Sleep();
        }

        

        else
        {
            rb.WakeUp();
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            /*
            if(!hasGottenOutOfTitan && !GameManager.IsTitan)
            {
                rb.position.
            }
            */
            capsuleBut2.enabled = true;
            rb.useGravity = true;
            capsule.enabled = true;
            hasDone = false;
        }
        
        if (!GameManager.IsTitan)
        {
            MovePlayer();
        }
        
        
        if (JumpTime)
        {
            exitingSlope = true;
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            JumpTime = false;
        }
    }


}
