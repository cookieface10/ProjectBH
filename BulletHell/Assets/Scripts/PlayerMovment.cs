using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovment : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;

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

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(KeyCode.Space) && canJump == true && grounded == true)
        {
            canJump = false;

            JumpTime = true;

        }
    }
    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        if (grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }

        if (!grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
    }
    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }
    private void Jump()
    {

    }
    private void ResetJump()
    {
        canJump = true;
    }

    private void Update()
    {
        //grounded = Physics.Raycast(transform.position, Vector3.down, playerheight * 0.5f + 0.05f, whatIsGround);
            MyInput();
            SpeedControl();


        if (grounded == true)
        {
            rb.drag = groundDrag;
            ResetJump();
        }
        else
        {
            rb.drag = 0;
        }
        Debug.Log(grounded);
    }
    private void FixedUpdate()
    {
        
        MovePlayer();
        
        if (JumpTime)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            JumpTime = false;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Terrain")
        {
            grounded = true;
        }

    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name == "Terrain")
        {
            grounded = false;
        }
    }

}
