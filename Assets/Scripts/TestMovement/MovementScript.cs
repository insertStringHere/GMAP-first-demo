using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour
{
    [Header("Player Movement")]
    public float moveSpeed = 1;
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    public bool readyToJump;


    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;


    [Header("Ground Check")]
    public float playerHeight;
    public float groundDrag;
    public LayerMask whatIsGround;
    public bool grounded;


    [Header("References")]
    public Transform orientation;
    public Rigidbody rb;


    private float horizontalInput;
    private float verticalInput;
    Vector3 moveDir;

    private void Start()
    {
        rb.GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;
    }

    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.1f, whatIsGround);

        MyInput();
        SpeedControl();

        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if(Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);

        }
    }

    private void MovePlayer()
    {
        moveDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (grounded)
            rb.AddForce(moveDir.normalized * moveSpeed * 10f, ForceMode.Force);
        else if (!grounded)
            rb.AddForce(moveDir.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);

    }

    private void SpeedControl()
    {
        Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // Cap the max velocity if needed
        if (flatVelocity.magnitude > moveSpeed)
        {
            Vector3 cappedVelocity = flatVelocity.normalized * moveSpeed;
            rb.velocity = new Vector3(cappedVelocity.x, rb.velocity.y, cappedVelocity.z);
        }
    }


    private void Jump()
    {
        // Reset y Velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

}
