using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A <see cref="Rigidbody"/> based player controller.
/// </summary>
public class PhysicsPlayerController : MonoBehaviour {
    /// <summary>
    /// The attachee <see cref="GameObject"/>'s <see cref="Rigidbody"/>.
    /// </summary>
    private Rigidbody rigidBody;

    /// <summary>
    /// A <see cref="Vector3"/> containing the accelleration value for the player in each direction.
    /// </summary>
    public Vector3 playerAcceleration = new Vector3(10, 5, 10);
    /// <summary>
    /// A <see cref="Vector3"/> containing the maximum absolute velocity that the player can go in any direction.
    /// </summary>
    public Vector3 maxSpeed = new Vector3(4, 15, 4);
    /// <summary>
    /// A <see cref="bool"/> representing whether or not the player is currently touching a ground.
    /// </summary>
    public bool grounded;

    /// <summary>
    /// A <see cref="bool"/> representing whether or not the player is using force to move.
    /// </summary>
    public bool useForce = true;


    // Does this need to be instance scope?
    [SerializeField] private LayerMask ground;

    // Cam controls

    /// <summary>
    /// The <see cref="Transform"/> for the player's main <see cref="Camera"/>.
    /// </summary>
    public Transform cam;
    /// <summary>
    /// A <see cref="Vector2"/> keeping track of the current x and y rotations of the player
    /// camera.
    /// </summary>
    private Vector2 turn;
    /// <summary>
    /// The mouse sensitivity for the camera going up and down.
    /// </summary>
    [Range(0.5f, 3)] public float mouseXSensitiviy = 1;
    /// <summary>
    /// The mouse sensitivity for the camera going side to side.
    /// </summary>
    [Range(0.5f, 3)] public float mouseYSensitiviy = 1;

    /// <summary>
    /// Caches the player input as a variable for use between Update and FixedUpdate
    /// </summary>
    [SerializeField] private Vector3 horizontalInput;
    /// <summary>
    /// Caches whether or not the jump button is pressed for use between Update and FixedUpdate
    /// </summary>
    [SerializeField] private bool jump;

    [SerializeField] private float SlowMultiplier = 1.1f;


    /// <summary>
    /// Attempts to set the <see cref="Rigidbody"/> and camera <see cref="Transform"/> if not set within the editor,
    /// and locks the CursorState.
    /// </summary>
    private void Start() {
        rigidBody = rigidBody == null ? GetComponent<Rigidbody>() : rigidBody;
        cam = cam == null ? GetComponentInChildren<Camera>().transform : cam;
        Cursor.lockState = CursorLockMode.Locked;

    }

    /// <summary>
    /// Calls <see cref="UpdateCamera"/> and captures player input.
    /// </summary> 
    void Update() {
        UpdateCamera();

        horizontalInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        jump = Input.GetKey(KeyCode.Space);

    }

    /// <summary>
    /// Updates the player's movement based on input; 
    /// <see cref="DoMovement(ref float, ref float)"/> and <see cref="DoJump(ref float)"/>. 
    /// <para/>
    /// Also applies velocity cap.
    /// </summary>    
    void FixedUpdate(){
        // Extract player velocities
        float xVel = transform.InverseTransformVector(rigidBody.velocity).x;
        float zVel = transform.InverseTransformVector(rigidBody.velocity).z;
        float yVel = transform.InverseTransformVector(rigidBody.velocity).y;

        //choose method of movement.
        if(useForce)
            PlayerMove();
        else
            DoMovement(ref xVel, ref zVel);

        DoJump(ref yVel);

        // Apply the velocities after doing movement
        //rigidBody.velocity = transform.TransformVector(new Vector3(xVel, yVel, zVel));
    }

    /// <summary>
    /// Updates camera rotations using player mouse input. Player <see cref="GameObject"/>
    /// is also transformed for y rotations, allowing forward to always be where the player is
    /// facing.
    /// </summary>
    public void UpdateCamera()
    {
        turn.x += Input.GetAxis("Mouse X") * mouseXSensitiviy * Time.deltaTime * 100;
        turn.y += Input.GetAxis("Mouse Y") * mouseYSensitiviy * Time.deltaTime * 100;
        transform.rotation = Quaternion.Euler(0f, turn.x, 0f);
        cam.transform.rotation = Quaternion.Euler(-turn.y, turn.x, 0f);
    }

    /// <summary>
    /// Calculates the player's x and z direction movement. Movement is applied using
    /// <see cref="Rigidbody.MovePosition"/> by applying the <see cref="playerAcceleration"/> with the second
    /// equation of motion. If the current velocity is higher than the max, however, it will be reduced
    /// to the max and stored in <paramref name="xVel"/> and <paramref name="zVel"/>.
    /// </summary>
    /// <param name="xVel">A reference to the x velocity of the player; will be capped to the maxSpeed value if too high</param>
    /// <param name="zVel">A reference to the z velocity of the player; will be capped to the maxSpeed value if too high</param>
    public void DoMovement(ref float xVel, ref float zVel)
    {
        // If there's input for x
        if (Math.Abs(horizontalInput.x) > .001)
        {
            // Check if the velocity is within the speed bounds
            if (Math.Abs(xVel) <= maxSpeed.x)
                // if it is, then set the force.
                horizontalInput.x *= playerAcceleration.x * Time.fixedDeltaTime * Time.fixedDeltaTime;
            else
            {
                // if not, there will be no force and the speed needs to be
                // reduced to the max speed, maintaining the same sign.
                horizontalInput.x = 0;
                xVel = maxSpeed.x * (xVel / Math.Abs(xVel));
            }
        }


        // If there's input for z
        if (Math.Abs(horizontalInput.z) > .001)
        {
            // Check if the velocity is within the speed bounds
            if (Math.Abs(zVel) <= maxSpeed.z)
                // if it is, then set the force.
                horizontalInput.z *= playerAcceleration.z * Time.fixedDeltaTime * Time.fixedDeltaTime;
            else
            {
                // if not, there will be no force and the speed needs to be
                // reduced to the max speed, maintaining the same sign.
                horizontalInput.z = 0;
                zVel = maxSpeed.z * (zVel / Math.Abs(zVel));
            }
        }

        // Add the force, translated to match the current rotation of the player object.
        rigidBody.MovePosition(transform.position + new Vector3(xVel * Time.fixedDeltaTime, 0, zVel * Time.fixedDeltaTime) + transform.TransformDirection(horizontalInput));
 
    }
    /// <summary>
    /// Calculates the players x and z movement using <see cref="rigidBody.AddForce"/> by multiplying the <see cref="playerAcceleration"/>
    /// by the mass of the player. If there is no input, the velocity of the player is devided by 1.1 every physics update.
    /// </summary>
    void PlayerMove()
    {
        //chacks for player input.
        if(MathF.Abs(horizontalInput.x) > 0.01f || MathF.Abs(horizontalInput.z) > 0.01f)
        {
            //checks if player is below max speed, then adds force in the inputed direction
            if (MathF.Abs(rigidBody.velocity.x) < maxSpeed.x && MathF.Abs(rigidBody.velocity.z) < maxSpeed.z)
                rigidBody.AddForce(transform.TransformDirection(new Vector3(horizontalInput.x * playerAcceleration.x * rigidBody.mass, 0f, horizontalInput.z * playerAcceleration.z * rigidBody.mass)));
            //if above max speed, sets the velocity in the indicated direction. This is to fix missed input bugs.
            else
                rigidBody.velocity = transform.TransformDirection(new Vector3(horizontalInput.x * maxSpeed.x, rigidBody.velocity.y, horizontalInput.z * maxSpeed.z));
        }
        else
        {
            //deccelerates player when there is no move input.
            rigidBody.velocity = new Vector3(rigidBody.velocity.x / SlowMultiplier, rigidBody.velocity.y, rigidBody.velocity.z / SlowMultiplier);
        }
    }

    /// <summary>
    /// Calculates the player's vertical velocity and processes jump input. Checks for ground using a raycast,
    /// then if the player is on the ground and jump is pressed enacts an impulse force upwards using the value specified
    /// in <see cref="playerAcceleration"/>. If not on the ground and the velocity is greater than the max speed, <paramref name="yVel"/> will
    /// be set to the value in <see cref="maxSpeed"/>.
    /// </summary>
    /// <param name="yVel">A reference to the z velocity of the player; will be capped to the maxSpeed value if too high</param>
    public void DoJump(ref float yVel)
    {

        //checks for ground
        grounded = Physics.Raycast(transform.position, Vector3.down, 1.1f, ground);

        // Caps the speed if greater than the max
        if (!grounded && Math.Abs(yVel) >= maxSpeed.y)
                yVel = maxSpeed.y * (yVel / Math.Abs(yVel));

        // Imparts a jump force on the player.
        if (jump && grounded)
        {
            rigidBody.AddForce(new Vector3(0, rigidBody.mass * playerAcceleration.y, 0), ForceMode.Impulse);
        }
    }

    /// <summary>
    /// Detects a collision with the player and calculates the force of the impact.
    /// If the impact is too high, the player will be killed.
    /// </summary>
    public void OnCollisionEnter(Collision collision) {
        if (collision.impulse.magnitude > 1000) {
            // kill player
            Debug.Log("U died");
        }
    }

}
