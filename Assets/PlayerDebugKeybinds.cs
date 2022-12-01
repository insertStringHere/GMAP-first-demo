using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDebugKeybinds : MonoBehaviour
{
    /// <summary>
    /// Allows the customization of shortcut keys for various debug functions
    /// 
    /// Provides Shortcut buttons for 
    ///     relaoding current scene
    ///     loading previous scene
    ///     loading next scene
    /// </summary>
    /// 

    public KeyCode restartLevel = KeyCode.R;
    public KeyCode loadPreviousLevel = KeyCode.Alpha9;
    public KeyCode loadNextLevel = KeyCode.Alpha0;
    
    [SerializeField] private LevelLoader l;

    /// <summary>
    /// Finds the levelloader in the current scene
    /// </summary>
    private void Start()
    {
        l = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();
    }

    
    void Update()
    {
        // Reload Current Scene
        if (Input.GetKey(restartLevel)) l.ReloadScene();

        // Reload Previous level
        if (Input.GetKey(loadPreviousLevel)) l.PreviousScene();

        // Reload Next level
        if (Input.GetKey(loadNextLevel)) l.NextScene();

    }
}
