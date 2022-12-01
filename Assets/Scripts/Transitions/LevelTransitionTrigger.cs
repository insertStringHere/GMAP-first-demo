using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransitionTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    public LevelLoader lLoad;
    public bool TargetScene;
    public int SelectedScene = 0;



    /// <summary>
    /// Detects if colliding with player and then triggers the level loader to begin transitioning to the next stage
    /// </summary>
    public void OnTriggerEnter(Collider other){
        if (!TargetScene)
        {
            if (other.CompareTag("Player"))
                lLoad.NextScene();
        }
        else
        {
            lLoad.ChangeScene(SelectedScene);
        }
    }

}
