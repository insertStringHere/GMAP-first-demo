using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VignetteRewind : MonoBehaviour
{
    [SerializeField]
    Volume volume;
    //[SerializeField]
   // Vignette vignette;
    [SerializeField]
    private float _intensity;
    // Update is called once per frame
   
    void Update()
    {
        if(Input.GetKey(KeyCode.Q))
        {
            VignetteOn();
        }
        else
        {
            VignetteOff();
        }
    }

    //Turn on vignette and add intensity
    void VignetteOn()
    {
        volume.enabled = true;
    }
    void VignetteOff()
    {
        volume.enabled = false;
    }
}
