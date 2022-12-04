using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A script for SFX sounds that are only heard when the player interacts with something,
/// like walking over a pressure plate. Put this script on an object with a trigger that you
/// you want to play a sound when the player walks into it. Add the sound's audio source in the
/// script's inspector.
/// </summary>
public class TriggerSFX : MonoBehaviour
{
    public AudioSource triggerSound; 

    private void OnTriggerEnter(Collider other)
    {
        triggerSound.Play();
    }
}
