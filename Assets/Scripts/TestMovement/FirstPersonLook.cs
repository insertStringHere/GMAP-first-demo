using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonLook : MonoBehaviour
{
    [Header("Preferences")]

    public float turnSpeed = 1;

    [Range(0.5f, 3)] public float mouseXSensitiviy = 1;
    [Range(0.5f, 3)] public float mouseYSensitiviy = 1;


    [Header("References")]
    public Transform cam;
    public Transform orientation;
    public Transform player;
    public Transform playerObj;

    private Vector2 turn;
    private float horizontalInput;
    private float veritcalInput;


    // Start is called before the first frame update
    void Start()
    {
        // Lock and hide cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        MyInput();
        rotateCamera();
        TurnPlayer2();

    }

    // Camera Input
    private void MyInput()
    {
        turn.x += Input.GetAxis("Mouse X") * mouseXSensitiviy;
        turn.y += Input.GetAxis("Mouse Y") * mouseYSensitiviy;

        horizontalInput = Input.GetAxis("Horizontal");
        veritcalInput = Input.GetAxis("Vertical");

    }

    private void rotateCamera()
    {
        cam.transform.rotation = Quaternion.Euler(-turn.y, turn.x, 0f);
        
        //resetTurn();
    }
    private void TurnPlayer2()
    {
        player.transform.rotation = Quaternion.Euler(0f, turn.x, 0f);
    }
    private void TurnPlayer1()
    {
        Vector3 facingDirection = player.position - new Vector3(transform.position.x, player.transform.position.y, transform.position.z);
        orientation.forward = facingDirection.normalized;

        // Rotate player to
        Vector3 inputDir = orientation.forward * veritcalInput + orientation.right * horizontalInput;

        if (inputDir != Vector3.zero)
        {
            print("Halp");
            playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * turnSpeed);
            //playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * turnSpeed);
        }
    }

    private void resetTurn()
    {
        turn = new Vector2(0, 0);
    }
}
