using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public List<Scene> SceneList ;
    public Animator Transition;
    public float TransitionTime = 1;

    /// <summary>
    /// Start method gets all available scenes for checking if they are available to warp to in ChangeScene()
    /// </summary>
    private void Start()
    {

        for (int i = 0; i < SceneManager.sceneCount; i++)
     {
        SceneList.Add(SceneManager.GetSceneAt(i));
     }

    }

    /// <summary>
    /// Triggers the actual change in scenes after validation from previous method and after scene transition animation is played
    /// </summary>
    public IEnumerator LoadLevel(int sceneIndex)
    {
        Transition.SetTrigger("Start");

        yield return new WaitForSeconds(TransitionTime);
        
        SceneManager.LoadScene(sceneIndex);
    }

    /// <summary>
    /// Level loader mode if public Target Bool is enabled
    /// Allows a designer to chose which scene to warp to for any given level loader 
    /// The chosen stage must be in the build settings to be a valid warp. Gice bad warp error if index is not in build settings
    /// </summary>
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

    /// <summary>
    /// Level loader default
    /// A transition trigger will default to loading the next level in the build settings and will loop back to index 0 at after the final scene
    /// </summary>
    public void NextScene()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (sceneIndex >= SceneManager.sceneCountInBuildSettings || sceneIndex < 0)
        {
            Debug.Log($"Input:{sceneIndex}, NumScenes:{SceneManager.sceneCountInBuildSettings}");
            StartCoroutine(LoadLevel(0));
        }
        else
        { 
            StartCoroutine(LoadLevel(sceneIndex));
        }
    }

    /// <summary>
    /// A transition that will load the previous scene and then loop to the last scene if at scene index 0
    /// </summary>
    public void PreviousScene()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex - 1;

        if (sceneIndex < 0 )
        {
            Debug.Log($"Input:{sceneIndex}, NumScenes:{SceneManager.sceneCountInBuildSettings}");
            StartCoroutine(LoadLevel(SceneManager.sceneCountInBuildSettings - 1));
        }
        else
        {
            StartCoroutine(LoadLevel(sceneIndex));
        }
    }

    /// <summary>
    /// Simply reloads the scene while providing the transition amimation
    /// </summary>
    public void ReloadScene()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        Debug.Log("Reloading Scene...");
        StartCoroutine(LoadLevel(sceneIndex));
    }
}
