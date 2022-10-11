using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioController : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] walkSounds;
    public AudioSource timeAudio;
    public AudioClip timeRewindSound;
    private int walkSoundIndex;
    private bool playSound = true;
    public float waitStep = .5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //checking to see if we can play a footstep sound
        if (playSound && Math.Abs(Input.GetAxisRaw("Horizontal")) + Math.Abs(Input.GetAxisRaw("Vertical")) > .1f) {
            walkSoundIndex = Random.Range(0, walkSounds.Length);
            audioSource.PlayOneShot(walkSounds[walkSoundIndex]);
            playSound = false;
            StartCoroutine(WaitForNextStep());
        }

        //play time rewind sound when pressing Q
        if (Input.GetKeyDown(KeyCode.Q)) {
            timeAudio.PlayOneShot(timeRewindSound);
        }
        if (Input.GetKeyUp(KeyCode.Q)) {
            timeAudio.Stop();
        }
    }

    //waits until the next step audio should be played
    public IEnumerator WaitForNextStep() {
        yield return new WaitForSeconds(waitStep);
        playSound = true;
    }
}
