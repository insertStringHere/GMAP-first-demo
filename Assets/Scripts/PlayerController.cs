using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Modified from unity docs: https://docs.unity3d.com/ScriptReference/CharacterController.Move.html
public class PlayerController : MonoBehaviour
{   
    public CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    public float playerSpeed = 5.0f;
    public float jumpHeight = 2.0f;
    private float gravityValue = -9.81f;

    public Transform cam;
    private Vector2 turn;
    public float turnSmoothnessTime = 0.1f;
    [Range(0.5f, 3)] public float mouseXSensitiviy = 1;
    [Range(0.5f, 3)] public float mouseYSensitiviy = 1;
    private float turnSmoothSpeed;


    private void Start()
    {
        //controller = gameObject.AddComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;

    }

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 movementDirection = new Vector3(horizontal, 0f, vertical).normalized;

        turn.x += Input.GetAxis("Mouse X") * mouseXSensitiviy;
        turn.y += Input.GetAxis("Mouse Y") * mouseYSensitiviy;
        transform.rotation = Quaternion.Euler(-turn.y, turn.x, 0f);

        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }


        if (movementDirection.magnitude >= 0.1f)
        {
            // Face character in correct direction
            float directionAngle = Mathf.Atan2(movementDirection.x, movementDirection.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, directionAngle, ref turnSmoothSpeed, turnSmoothnessTime);
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, smoothAngle, 0f);

            // Convert rotation to vector by multiplying by normal of z axis
            Vector3 moveDirectionCam = Quaternion.Euler(0f, directionAngle, 0f) * Vector3.forward;
            controller.Move(moveDirectionCam.normalized * playerSpeed * Time.deltaTime);

            
        }

        //playerVelocity.Set(Input.GetAxis("Horizontal") * playerSpeed, playerVelocity.y, Input.GetAxis("Vertical") * playerSpeed);

        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
}
