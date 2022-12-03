using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerObject : MonoBehaviour
{
    public GameObject[] targets;
    public bool Disable = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(Disable){
                foreach (GameObject target in targets)
                {
                    target.gameObject.SetActive(false);
                }
            }
            else{
                foreach (GameObject target in targets)
                {
                    target.gameObject.SetActive(true);
                }
            }
        }
    }
}