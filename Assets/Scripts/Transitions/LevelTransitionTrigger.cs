using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTransitionTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    public LevelLoader lLoad;

    public void OnTriggerEnter(Collider other){
        if(other.CompareTag("Player"))
            lLoad.NextScene();
    }

}
