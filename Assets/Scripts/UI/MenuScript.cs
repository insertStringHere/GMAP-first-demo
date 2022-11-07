using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScript : MonoBehaviour
{
    public GameObject panel;
    /// <summary>
    /// Function to display a small menu when the Escape key is pressed.
    /// Allows the player to restart a scene if they get stuck.
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Debug.Log("Toggling menu...");
            if(panel.active = !panel.active) {
                Cursor.lockState = CursorLockMode.Confined;
            } else {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }
}
