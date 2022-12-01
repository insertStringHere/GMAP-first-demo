using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// A controller for player audio.
/// </summary>
public class AudioController : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] walkSounds;
    public AudioSource timeAudio;
    public AudioClip timeRewindSound;
    public AudioClip ttOpenCloseSounds;
    public AudioClip rewindStartSound;
    public AudioClip rewindLimitWarningSound;
    private int walkSoundIndex;
    private bool playSound = true;
    public float waitStep = .5f;
    private bool ttclosed = true;

    public PhysicsPlayerController ppc;

    /// <summary>
    /// Controls player walking and time rewind sounds.
    /// </summary>
    void Update()
    {
        //checking to see if we can play a footstep sound
        if (playSound && Math.Abs(Input.GetAxisRaw("Horizontal")) + Math.Abs(Input.GetAxisRaw("Vertical")) > .1f && ppc.grounded) {
            walkSoundIndex = Random.Range(0, walkSounds.Length);
            audioSource.PlayOneShot(walkSounds[walkSoundIndex]);
            playSound = false;
            StartCoroutine(WaitForNextStep());
        }

        //play time rewind sound when pressing Q
        if (Input.GetKeyDown(KeyCode.Q)) {
            TimeTurnerOpen();
        }
        if (Input.GetKeyUp(KeyCode.Q))
        {
            TimeTurnerClose();
        }
    }
    /// <summary>
    /// a method triggered by pressing the time rewind button that plays the <see cref="ttOpenCloseSounds"/> sound effect 
    /// </summary>
    void TimeTurnerOpen()
    {
        ttclosed = false;
        timeAudio.loop = false;
        if (timeAudio.isPlaying)
            timeAudio.Stop();
        timeAudio.PlayOneShot(ttOpenCloseSounds);
        StartCoroutine(WaitTilOpen());
    }
    /// <summary>
    /// A method triggered by releasing the time rewind button  that plays the <see cref="ttOpenCloseSounds"/> sound effect
    /// </summary>
    void TimeTurnerClose()
    {
        ttclosed = true;
        timeAudio.loop = false;
        if (timeAudio.isPlaying)
            timeAudio.Stop();
        timeAudio.PlayOneShot(ttOpenCloseSounds);
    }

    void PlayTimeRewind()
    {
        timeAudio.PlayOneShot(rewindStartSound);
        StartCoroutine(WaitForRewind());
    }

    void PlayRewindSFX()
    {
        timeAudio.clip = timeRewindSound;
        timeAudio.loop = true;
        timeAudio.Play();
    }
    /// <summary>
    /// A coroutine method that waits until the next step
    /// audio should be played
    /// </summary>
    /// <returns>A <see cref="WaitForSeconds"/> object.</returns>
    public IEnumerator WaitForNextStep() {
        yield return new WaitForSeconds(waitStep);
        playSound = true;
    }
    /// <summary>
    /// A coroutine method that waits until the <see cref="ttOpenCloseSounds"/> has finished,
    /// then starts the <see cref="PlayTimeRewind"/> method.
    /// </summary>
    /// <returns>>A <see cref="WaitForSeconds"/> object.</returns>
    public IEnumerator WaitTilOpen()
    {
        yield return new WaitForSeconds(ttOpenCloseSounds.length);
        if (!ttclosed)
        {
            PlayTimeRewind();
        }           
    }

    public IEnumerator WaitForRewind()
    {
        yield return new WaitForSeconds(rewindStartSound.length);
        if (!ttclosed)
        {
            PlayRewindSFX();
        }
    }
}
