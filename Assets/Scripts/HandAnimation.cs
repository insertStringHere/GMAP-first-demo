using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandAnimation : MonoBehaviour
{
    /// <summary>
    /// The animation controller for the chronometer
    /// </summary>
    public Animator timeRewindAnimator;
    /// <summary>
    /// The animation controller for the player hand
    /// </summary>
    public Animator handRewindAnimator;
    /// <summary>
    /// The particle system for active rewindi
    /// </summary>
    public GameObject goodParticlePrefab;
    public GameObject badParticlePrefab;
    //public GameObject timeTurnerAnimationObject;
    public KeyCode rewindPrimary = KeyCode.Mouse0;
    public KeyCode rewindSecondary = KeyCode.Q;

    void Update()
    {
        if(Input.GetKey(rewindPrimary) || Input.GetKey(rewindSecondary))
        {
            timeRewindAnimator.SetBool("Open", true);
            handRewindAnimator.SetBool("Open", true);
            Invoke("setParticlesActive", 0.55f);
        }
        else
        {
            timeRewindAnimator.SetBool("Open", false);
            handRewindAnimator.SetBool("Open", false);
            goodParticlePrefab.SetActive(false);
            badParticlePrefab.SetActive(false);
        }

    }

    // Set in method as to invoke after delay
    private void setParticlesActive()
    {
        goodParticlePrefab.SetActive(true);
    }



}
