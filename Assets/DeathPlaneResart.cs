using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPlaneResart : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player found");
            other.gameObject.transform.parent.rotation = Quaternion.Euler(new Vector3(other.transform.rotation.x, other.transform.rotation.y, -90f));
            //other.transform.rotation = Quaternion.Euler(other.transform.rotation.x, other.transform.rotation.y, -90f);
        }
    }
}
