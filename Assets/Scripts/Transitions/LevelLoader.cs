using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public List<Scene> SceneList ;
    public Animator Transition;
    public float TransitionTime = 1;

    private void Start()
    {
    // Get all scenes 
     for (int i = 0; i < SceneManager.sceneCount; i++)
     {
        SceneList.Add(SceneManager.GetSceneAt(i));
     }

    }

    public IEnumerator LoadLevel(int sceneIndex)
    {
        Transition.SetTrigger("Start");

        yield return new WaitForSeconds(TransitionTime);
        
        SceneManager.LoadScene(sceneIndex);
    }

    public void ChangeScene(int sceneIndex)
    {
        if(sceneIndex >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.Log("Invalid Scene Index: Bad Warp");
            Debug.Log($"Input:{sceneIndex}, NumScenes:{SceneManager.sceneCountInBuildSettings}");
        }
        else
        {
            StartCoroutine(LoadLevel(sceneIndex));
        }
    }

    public void NextScene()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (sceneIndex >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.Log($"Input:{sceneIndex}, NumScenes:{SceneManager.sceneCountInBuildSettings}");
            StartCoroutine(LoadLevel(0));
            // SceneManager.LoadScene(0);
        }
        else
        { 
            StartCoroutine(LoadLevel(sceneIndex));
            // SceneManager.LoadScene(sceneIndex);
        }
    }
}
