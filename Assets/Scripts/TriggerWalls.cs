using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerWalls : MonoBehaviour
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

        ///<summary>
        /// This script is for triggers, when player enter triggers it will set other obstacles active or not(opening, or closing, different passageways) 
        ///</summary>



        // if (other.gameObject.CompareTag("TWall"))
        // {
        //     other.gameObject.SetActive(false);
        //     TWallcount = TWallcount + 1;
        // }
        // if (other.gameObject.CompareTag("GWall"))
        // {
        //     other.gameObject.SetActive(false);
        //     Bridge.gameObject.SetActive(true);
        // }
    }

    // // Start is called before the first frame update
    // void Start()
    // {

    // }

    // // Update is called once per frame
    // void Update()
    // {

    // }
}
