using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsPlayerController : MonoBehaviour {
    private Rigidbody rigidBody;

    public Vector3 playerAcceleration = new Vector3(10, 10, 20);
    public Vector3 maxSpeed = new Vector3(3, 15, 5);

    [SerializeField] private GameObject ground = null;
    [SerializeField] private float maxSlope = 20f;

    public Transform cam;
    private Vector2 turn;
    [Range(0.5f, 3)] public float mouseXSensitiviy = 1;
    [Range(0.5f, 3)] public float mouseYSensitiviy = 1;


    private void Start() {
        rigidBody = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;

    }

    void Update() {
        turn.x += Input.GetAxis("Mouse X") * mouseXSensitiviy;
        turn.y += Input.GetAxis("Mouse Y") * mouseYSensitiviy;
        transform.rotation = Quaternion.Euler(0f, turn.x, 0f);
        cam.transform.rotation = Quaternion.Euler(-turn.y, turn.x, 0f);

        float x = Input.GetAxisRaw("Horizontal");
        float xVel = transform.InverseTransformVector(rigidBody.velocity).x;
        float z = Input.GetAxisRaw("Vertical");
        float zVel = transform.InverseTransformVector(rigidBody.velocity).z;

    
        if (Math.Abs(x) > .001) {
            if (Math.Abs(xVel) <= maxSpeed.x)
                x *= rigidBody.mass * playerAcceleration.x;
            else {
                x = 0;
                xVel = maxSpeed.x * (xVel / Math.Abs(xVel));
            }
        }

        if (Math.Abs(z) > .001) {
            if (Math.Abs(zVel) <= maxSpeed.z)
                z *= rigidBody.mass * playerAcceleration.z;
            else {
                z = 0;
                zVel = maxSpeed.z * (zVel / Math.Abs(zVel));
            }
        }

        rigidBody.AddForce(transform.TransformVector(new Vector3(x, 0f, z)));


        float jump = 0;
        float yVel = transform.InverseTransformVector(rigidBody.velocity).y;

        // Changes the height position of the player..

        if(ground == null) {
            if (Math.Abs(yVel) >= maxSpeed.y)
                yVel = maxSpeed.y * (yVel / Math.Abs(yVel));
        } else if (Input.GetButtonDown("Jump"))
            jump = playerAcceleration.y * rigidBody.mass;


        rigidBody.AddForce(new Vector3(0f, jump, 0f), ForceMode.Impulse);

        rigidBody.velocity = transform.TransformVector(new Vector3(xVel, yVel, zVel));
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
}
