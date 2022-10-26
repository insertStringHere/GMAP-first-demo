using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A <see cref="Rigidbody"/> based player controller.
/// </summary>
public class PhysicsPlayerController : MonoBehaviour {
    /// <summary>
    /// The the attachee <see cref="GameObject"/>'s <see cref="Rigidbody"/>.
    /// </summary>
    private Rigidbody rigidBody;

    //Don't know what these are
    private int TWallcount;
    public GameObject Rock;
    public GameObject Gate;
    public GameObject Rock2;
    public GameObject Bridge;

    /// <summary>
    /// A <see cref="Vector3"/> containing the accelleration value for the player in each direction.
    /// </summary>
    public Vector3 playerAcceleration = new Vector3(10, 10, 20);
    /// <summary>
    /// A <see cref="Vector3"/> containing the maximum absolute velocity that the player can go in any direction.
    /// </summary>
    public Vector3 maxSpeed = new Vector3(3, 15, 5);
    /// <summary>
    /// A <see cref="bool"/> representing whether or not the player is currently touching a ground.
    /// </summary>
    public bool grounded;


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
    /// Attempts to set the <see cref="Rigidbody"/> and camera <see cref="Transform"/> if not set within the editor,
    /// and locks the CursorState.
    /// </summary>
    private void Start() {
        rigidBody = rigidBody == null ? GetComponent<Rigidbody>() : rigidBody;
        cam = cam == null ? GetComponentInChildren<Camera>().transform : cam;
        Cursor.lockState = CursorLockMode.Locked;

    }

    /// <summary>
    /// Updates the player's movement based on input; calls <see cref="UpdateCamera"/>, 
    /// <see cref="DoMovement(ref float, ref float)"/>, and <see cref="DoJump(ref float)"/>. 
    /// <para/>
    /// Also applies velocity cap.
    /// </summary>
    void Update() {
        UpdateCamera();


        // Someone document what this is
        if (TWallcount == 1)
        {
            Rock.gameObject.SetActive(true);
            Gate.gameObject.SetActive(false);
        }
        if (TWallcount == 2)
        {
            Rock2.gameObject.SetActive(true);
        }

        // Extract player velocities
        float xVel = transform.InverseTransformVector(rigidBody.velocity).x;
        float zVel = transform.InverseTransformVector(rigidBody.velocity).z;
        float yVel = transform.InverseTransformVector(rigidBody.velocity).y;
        
        DoMovement(ref xVel, ref zVel);
        DoJump(ref yVel);

        // Apply the velocities after doing movement
        rigidBody.velocity = transform.TransformVector(new Vector3(xVel, yVel, zVel));

        // Like, please; I'm so confused
        if (TWallcount == 1)
        {
            Rock.gameObject.SetActive(true);
            Gate.gameObject.SetActive(false);
        }
        if (TWallcount == 2)
        {
            Rock2.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Updates camera rotations using player mouse input. Player <see cref="GameObject"/>
    /// is also transformed for y rotations, allowing forward to always be where the player is
    /// facing.
    /// </summary>
    public void UpdateCamera()
    {
        turn.x += Input.GetAxis("Mouse X") * mouseXSensitiviy;
        turn.y += Input.GetAxis("Mouse Y") * mouseYSensitiviy;
        transform.rotation = Quaternion.Euler(0f, turn.x, 0f);
        cam.transform.rotation = Quaternion.Euler(-turn.y, turn.x, 0f);
    }

    /// <summary>
    /// Calculates the player's x and z direction movement. Movement is converted
    /// into a <see cref="Rigidbody"/> force using the mass and the <see cref="playerAcceleration"/> 
    /// for that direction. If the current velocity is higher than the max, however, it will be reduced
    /// to the max and stored in <paramref name="xVel"/> and <paramref name="zVel"/>.
    /// </summary>
    /// <param name="xVel">A reference to the x velocity of the player; will be capped to the maxSpeed value if too high</param>
    /// <param name="zVel">A reference to the z velocity of the player; will be capped to the maxSpeed value if too high</param>
    public void DoMovement(ref float xVel, ref float zVel)
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        // If there's input for x
        if (Math.Abs(x) > .001)
        {
            // Check if the velocity is within the speed bounds
            if (Math.Abs(xVel) <= maxSpeed.x)
                // if it is, then set the force.
                x *= rigidBody.mass * playerAcceleration.x;
            else
            {
                // if not, there will be no force and the speed needs to be
                // reduced to the max speed, maintaining the same sign.
                x = 0;
                xVel = maxSpeed.x * (xVel / Math.Abs(xVel));
            }
        }


        // If there's input for z
        if (Math.Abs(z) > .001)
        {
            // Check if the velocity is within the speed bounds
            if (Math.Abs(zVel) <= maxSpeed.z)
                // if it is, then set the force.
                z *= rigidBody.mass * playerAcceleration.z;
            else
            {
                // if not, there will be no force and the speed needs to be
                // reduced to the max speed, maintaining the same sign.
                z = 0;
                zVel = maxSpeed.z * (zVel / Math.Abs(zVel));
            }
        }

        // Add the force, translated to match the current rotation of the player object.
        rigidBody.AddForce(transform.TransformVector(new Vector3(x, 0f, z)));
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
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
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

    // Not sure what this does
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("TWall"))
        {
            other.gameObject.SetActive(false);
            TWallcount = TWallcount + 1;
        }
        if (other.gameObject.CompareTag("GWall"))
        {
            other.gameObject.SetActive(false);
            Bridge.gameObject.SetActive(true);
        }
    }

}
