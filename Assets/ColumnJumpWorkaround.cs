using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColumnJumpWorkaround : MonoBehaviour
{
    public float jumpForce = 5;

    public void OnCollisionStay(Collision collision) {
        if (collision.gameObject.tag == "Player") {
            if (collision.gameObject.GetComponentInChildren<PhysicsPlayerController>().jump) {
                collision.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, collision.gameObject.GetComponent<Rigidbody>().mass * jumpForce, 0), ForceMode.Impulse);
            }
        }
    }
}
