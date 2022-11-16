using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallThroughFix : MonoBehaviour
{
    public float UnitsToMoveUp = 1;

    void OnTriggerEnter(Collider other) {
        if(!GameObject.ReferenceEquals(other.gameObject, this.transform.parent.parent.gameObject) && !(other.gameObject.transform.tag == "Ground")) {
            other.gameObject.transform.Translate(Vector3.up * Time.deltaTime * UnitsToMoveUp);
            Debug.Log(other.name);
        }
    }
}