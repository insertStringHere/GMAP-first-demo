using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsPlayerController : MonoBehaviour
{   
    private Rigidbody rigidBody;

    private int TWallcount;
    public GameObject Rock;
    public GameObject Gate;
    public GameObject Rock2;
    public GameObject Bridge;


    public float playerAcceleration = 1.0f;
    public float jumpAcceleration = 2.0f;
    public Vector3 maxSpeed = new Vector3 { x = 3.0f, z = 5.0f };

    [SerializeField] private GameObject ground = null;
    [SerializeField] private float maxSlope = 20f;

    public Transform cam;
    private Vector2 turn;
    [Range(0.5f, 3)] public float mouseXSensitiviy = 1;
    [Range(0.5f, 3)] public float mouseYSensitiviy = 1;


    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;

    }

    void Update()
    {
        float horizontal = 0f;
        float vertical = 0f;
        float jump = 0f;        

        if (Math.Abs(rigidBody.velocity.x) <= maxSpeed.x && Math.Abs(rigidBody.velocity.z) <= maxSpeed.z) {
            horizontal = Input.GetAxisRaw("Horizontal") * rigidBody.mass * playerAcceleration;
            vertical = Input.GetAxisRaw("Vertical") * rigidBody.mass * playerAcceleration;
        }             

        turn.x += Input.GetAxis("Mouse X") * mouseXSensitiviy;
        turn.y += Input.GetAxis("Mouse Y") * mouseYSensitiviy;
        transform.rotation = Quaternion.Euler(0f, turn.x, 0f);
        cam.transform.rotation = Quaternion.Euler(-turn.y, turn.x, 0f);

        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && ground != null)
            jump = jumpAcceleration * rigidBody.mass;

        rigidBody.AddForce(transform.TransformVector(new Vector3(horizontal, 0f, vertical)));
        rigidBody.AddForce(new Vector3(0f, jump, 0f), ForceMode.Impulse);

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

    public void OnCollisionEnter(Collision collision) {
        if (collision.impulse.magnitude > 100) {
            // kill player
            Debug.Log("U died");
        }

        // Calculate if on ground
        float xAngle = Vector3.SignedAngle(Vector3.up, collision.contacts[0].normal, Vector3.right);
        float zAngle = Vector3.SignedAngle(Vector3.up, collision.contacts[0].normal, Vector3.forward);
        if (Math.Abs(xAngle) <= maxSlope && Math.Abs(zAngle) <= maxSlope && rigidBody.velocity.y < 0.1f) {
            ground = collision.gameObject;
        }
    }

    public void OnCollisionExit(Collision collision) {
        if (collision.gameObject == ground)
            ground = null;
    }
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
