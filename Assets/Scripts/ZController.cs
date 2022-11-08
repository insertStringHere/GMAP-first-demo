using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
//using UnityEditor.SearchService;

/// <summary>
/// A script to control all <see cref="IRewinder"/>s in the current <see cref="GameObject"/>'s children
/// and process their state capture, motion, and rewind.
/// </summary>
/// <remarks>
/// Because like, control + z, get it? Because it undoes?
/// </remarks>
public class ZController : MonoBehaviour {
    /// <summary>
    /// A collection of all <see cref="IRewinder"/>s within this controller's 
    /// influence.
    /// </summary>
    private IEnumerable<IRewinder> rewinds;

    /// <summary>
    /// The interval at which a record state should be taken in seconds.
    /// </summary>
    public float recordInterval = .2f;
    /// <summary>
    /// A measure of how quickly the rewinds should occur as a multiplier
    /// </summary>
    public float rewindScale = 1f;
    /// <summary>
    /// The amount of time that has passed since an update; used to calculate
    /// the record interval and the rewind limit
    /// </summary>
    [SerializeField] private float deltaTime;
    /// <summary>
    /// A variable to keep track of if the rewind buttons are being pressed;
    /// </summary>
    private bool rewinding;

    /// <summary>
    /// How long in seconds a rewind can be held at a fully rewound state before
    /// "burning out" and needing to cooldown. -1 is disabled
    /// </summary>
    public float rewindLimit = -1f;
    /// <summary>
    /// How long the player should be made to wait until they can use the rewind again
    /// after burning it out.
    /// </summary>
    public float rewindCooldown = 2f;
    /// <summary>
    /// Keeps track of how long the cooldown has left before players can use rewind again.
    /// </summary>
    [SerializeField] private float cooldown = 0f;

    /// <summary>
    /// A measure of how close to zero a vector needs to be for a child <see cref="IRewinder"/>
    /// to consider it to be zero; use primarily for <see cref="IRewinder.NeedUpdate"/>.
    /// </summary>
    public float approximateLeniency = .01f;

    /// <summary>
    /// How much time in seconds the player has with this <see cref="ZController"/> before they can't use 
    /// it anymore. 0 means disabled.
    /// </summary>
    public float timeAllowance = 0f;
    
    /// <summary>
    /// The internal tracking of how much time the player has with the <see cref="ZController"/>
    /// </summary>
    public float timeRemaining; 



    /// <summary>
    /// A boolean to determine whether or not this controller is active.
    /// </summary>
    /// <remarks>
    /// The Unity version won't work because it disables the update methods when the
    /// object is inactive.
    /// </remarks>
    public bool active;

    /// <summary>
    /// Initialize the time and extract the children <see cref="IRewinder"/>s. If the
    /// rewind scale is below zero, set it to 1 to prevent crashes.
    /// </summary>
    void Start() {
        deltaTime = 0;
        timeRemaining = timeAllowance; 
        if (rewindScale <= 0) rewindScale = 1f;
        rewinds = GetComponentsInChildren<IRewinder>();
    }

    /// <summary>
    /// Captures the input for rewinding and continues playing the scene
    /// if the keys released.
    /// </summary>
    private void Update() {
        rewinding = Input.GetMouseButton(0) || Input.GetKey(KeyCode.Q);

        // Finally, if the player stopped rewinding, then play the states
        if (Input.GetKeyUp(KeyCode.Q) || Input.GetMouseButtonUp(0))
            foreach (IRewinder r in rewinds) if (r.isActiveAndEnabled) r.Play();
    }

    /// <summary>
    /// Handle all rewind code. 
    /// <list type="bullet">
    /// <item>
    /// If the player is doing a rewind, then iterate
    /// through the rewind states for each <see cref="IRewinder"/>
    /// </item>
    /// <item>
    /// If all states have been rewound and the rewind limit is reached,
    /// set the cooldown and play motion.
    /// </item>
    /// <item>
    /// If there is an IRewinder that is still moving, all IRewinders
    /// capture their current states.
    /// </item>
    /// </list>
    /// </summary>
    void FixedUpdate() {
        if (active) {
            deltaTime += Time.fixedDeltaTime;
             
            // Decrement the cooldown if necessary
            if(cooldown > 0) {
                cooldown -= Time.fixedDeltaTime;
            }

            // If it's time to rewind a state and the player is trying to rewind a state
            if (deltaTime >= (recordInterval / rewindScale) &&
                timeRemaining >= 0 && 
                rewinding && 
                cooldown <= 0) {
                // If there are states to rewind, apply them
                if (rewinds.FirstOrDefault()?.HasStates() ?? false) {
                    foreach (IRewinder r in rewinds) if (r.isActiveAndEnabled) r.RewindState((int)(deltaTime / (recordInterval / rewindScale)));
                    deltaTime = 0;

                }
                // If there aren't and player exceeded the limit, then set the cooldown
                // and play the objects.
                else if (deltaTime > rewindLimit && rewindLimit >= 0) {
                    cooldown = rewindCooldown;
                    foreach (IRewinder r in rewinds) if (r.isActiveAndEnabled) r.Play();
                    deltaTime = 0;
                }

                if (timeAllowance > 0) {
                    timeRemaining -= Time.fixedDeltaTime;

                    if (timeRemaining <= 0)
                        foreach (IRewinder r in rewinds) if (r.isActiveAndEnabled) r.Play();
                }
                // If it's time to capture a state, capture the next state
            } else if (deltaTime >= recordInterval) {
                foreach (IRewinder r in rewinds) if(r.isActiveAndEnabled) r.Store();
                deltaTime = 0;
            }

            

        }
    }
    
    /// <summary>
    /// Allows for multiple controllers to exist within the same scene, setting activation to a bound
    /// set of triggers. Whenever the trigger attached to this <see cref="GameObject"/> is entered, all other
    /// controllers will be paused.
    /// </summary>
    private void OnTriggerEnter(Collider other) {
        foreach (ZController z in transform.parent?.GetComponentsInChildren<ZController>()) {
            z.Pause();
            z.active = false;
        }

        foreach (IRewinder r in rewinds) {
            r.enabled = true;
            r.Play();
        }

        timeRemaining = timeAllowance;
        active = true;

        // Set additional elements to use the new ZController
        UnityEngine.Object o;
        if ((o = FindObjectOfType<DebugPanel>()) != null)
            (o as DebugPanel).zc = this;
        if ((o = FindObjectOfType<RewindTimeBar>()) != null)
            (o as RewindTimeBar).zc = this;

    }

    /// <summary>
    /// Pauses all <see cref="IRewinder"/>s under this controller's control.
    /// </summary>
    public void Pause() {
        foreach (IRewinder r in rewinds)
            r.Pause();
    }

    /// <summary>
    /// Checks if two vectors are approximately equivalent with <see cref="approximateLeniency"/> leniency.
    /// </summary>
    /// <param name="a">First vector</param>
    /// <param name="b">Second vector</param>
    /// <returns>True if the difference between the vectors is less than <see cref="approximateLeniency"/></returns>
    public bool Approximate(Vector3 a, Vector3 b)
    {
        return Mathf.Abs(a.x - b.x) < approximateLeniency &&
               Mathf.Abs(a.y - b.y) < approximateLeniency &&
               Mathf.Abs(a.z - b.z) < approximateLeniency;
    }
}
