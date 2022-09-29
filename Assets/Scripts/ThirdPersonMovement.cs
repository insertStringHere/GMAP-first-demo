using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public CharacterController _controller;
    public Transform _cam;

    public float walkSpeed = 5f;
    public float turnSmoothnessTime = 0.1f;
    private float turnSmoothSpeed;


    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 movementDirection = new Vector3(horizontal, 0f, vertical).normalized;

        if (movementDirection.magnitude >= 0.1f){
            // Face character in correct direction
            float directionAngle = Mathf.Atan2(movementDirection.x, movementDirection.z) * Mathf.Rad2Deg + _cam.eulerAngles.y;
            float smoothAngle =  Mathf.SmoothDampAngle(transform.eulerAngles.y, directionAngle,  ref turnSmoothSpeed, turnSmoothnessTime);
            transform.rotation = Quaternion.Euler(0f, smoothAngle, 0f);

            // Convert rotation to vector by multiplying by normal of z axis
            Vector3 moveDirectionCam = Quaternion.Euler(0f, directionAngle, 0f) * Vector3.forward;
            _controller.Move(moveDirectionCam.normalized * walkSpeed * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.Space)){
            this.gameObject.transform.position = new Vector3(0 , 0, 0);
        }
    }
}
