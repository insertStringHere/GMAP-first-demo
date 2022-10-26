using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

//class the set the fog in the scene as well as any other room effects needed
public class RoomSettingsScript : MonoBehaviour
{
    [SerializeField] private Color fogColor;
    // Start is called before the first frame update
    void Start()
    {
        //enables fog and chnges fog color
        RenderSettings.fog = true;
        RenderSettings.fogColor = fogColor;
    }
}
