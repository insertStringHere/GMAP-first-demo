using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandAnimation : MonoBehaviour
{
    public Animator timeRewindAnimator;
    public float transitionTime = 1;
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
            Invoke("setParticlesActive", 0.55f);
        }
        else
        {
            timeRewindAnimator.SetBool("Open", false);
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
