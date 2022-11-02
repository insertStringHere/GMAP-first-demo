using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandAnimation : MonoBehaviour
{
    public Animator timeRewindAnimator;
    public float transitionTime = 1;
    public KeyCode rewindPrimary = KeyCode.Mouse0;
    public KeyCode rewindSecondary = KeyCode.Q;

    void Update()
    {
        if(Input.GetKey(rewindPrimary) || Input.GetKey(rewindSecondary))
        {
            timeRewindAnimator.SetBool("Open", true);
        }
        else
        {
            timeRewindAnimator.SetBool("Open", false);
        }
    }
}
