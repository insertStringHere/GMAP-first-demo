using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An abstraction of the rewinder functionality for use
/// with a <see cref="ZController"/> to rewind an object.
/// </summary>
public abstract class IRewinder : MonoBehaviour {
    /// <summary>
    /// Initial position state of the attachee <see cref="GameObject"/>
    /// </summary>
    public Vector3 startPos { get; protected set; }
    /// <summary>
    /// Initial rotation state of the attachee <see cref="GameObject"/>
    /// </summary>
    public Quaternion startRot { get; protected set; }

    /// <summary>
    /// Parent <see cref="ZController"/> of the current <see cref="GameObject"/>; Primarily
    /// for use of <see cref="ZController.Approximate(Vector3, Vector3)"/> method.
    /// </summary>
    public ZController zController;

    /// <summary>
    /// Stack of <see cref="SnapState"/> objects; used to stash and then unwind different
    /// points within the <see cref="IRewinder"/>'s lifecycle over the course of the
    /// <see cref="ZController"/>'s rewind capture.
    /// </summary>
    protected Stack<SnapState> states;

    /// <summary>
    /// A variable to keep track of what state the IRewinder is currently on. 
    /// Increments with each call to store and decrements as the object is rewound.
    /// </summary>
    [SerializeField] protected int frameNumber;

    /// <summary>
    /// Debug variable to toggle output in the debug console; in child methods, whenever
    /// debug is required, wrap it with an if statment checking this field.
    /// </summary>
    [SerializeField] protected bool printDebug;




    /// <summary>
    /// Initializes <see cref="SnapState"/> list, sets initial position and rotation, and
    /// extracts the <see cref="ZController"/> from parent if it was not explicitly set.
    /// Finally, pauses the object, awaiting the ZController's signal.
    /// </summary>
    public virtual void Start() {
        states = new Stack<SnapState>();
        frameNumber = 0; 

        startPos = transform.position;
        startRot = transform.rotation;

        zController = zController == null ? GetComponentInParent<ZController>() : zController;

        Pause();
    }

    /// <summary>
    /// Checks to see if the object has moved since the last state capture. Uses <see cref="ZController.Approximate(Vector3, Vector3)"/>
    /// to compare the previous state's position with the current's. If they are not equivalent within the range of leniency, then a signal
    /// is sent stating that an update is required.
    /// </summary>
    /// <returns>True if the object has moved</returns>
    public virtual bool NeedUpdate() {
        if(!states.TryPeek(out SnapState state)) {
            state = new SnapState {
                frameNumber = frameNumber,
                position = startPos,
                rotation = startRot
            };
        }
        bool needUpdate = !(zController.Approximate(state.position, transform.position) && zController.Approximate(state.rotation.eulerAngles, transform.rotation.eulerAngles));
        if (printDebug && needUpdate)
            Debug.Log($"{name} needs movement update");
        return needUpdate;
    }

    /// <summary>
    /// Stores the current position and rotation of the object
    /// as well as the current frame number on the
    /// rewind stack if the object needs an update.
    /// </summary>
    public virtual void Store() {
        frameNumber++;
        if (NeedUpdate()) {
            SnapState s = new SnapState {
                frameNumber = frameNumber,
                position = transform.position,
                rotation = transform.rotation
            };

            states.Push(s);
        }
    }

    /// <summary>
    /// Rewinds the states from the stack frame one state at a time,
    /// allowing for easier speed-up of rewind. If there are no states left,
    /// the function returns null
    /// </summary>
    /// <param name="count">
    /// The number of frames that should be undone in a single call to this method.
    /// <remark>Can make the animation choppy if this number is too high!</remark>
    /// </param>
    /// <returns>The popped <see cref="SnapState"/> or null if there are no states remaining.</returns>
    public virtual SnapState RewindState(int count) {
        SnapState state = null;

        if (states.Count > 0) {
            Pause();
            while (states.Count > 0 && count >= 0) {
                if(states.Peek().frameNumber == frameNumber)
                    state = states.Pop();
                frameNumber--;
                count--;
            }
            
            if(state != null)
                transform.SetPositionAndRotation(state.position, state.rotation);

            if (printDebug && state != null)
                Debug.Log($"{name} popping and applying state {states.Count + 1}");
        }

        return state;
    }

    /// <summary>
    /// Resets the object to its initial position and rotation and clears the
    /// <see cref="SnapState"/> stack.
    /// </summary>
    public virtual void Reset() {
        transform.position = startPos;
        transform.rotation = startRot;
        states.Clear();
        frameNumber = 0;
        Pause();
    }

    /// <summary>
    /// Continues time wherever it was left off, however the concrete
    /// implementation needs to accomplish this.
    /// </summary>
    public abstract void Play();

    /// <summary>
    /// Pauses the movement of the object however the concrete implementation
    /// needs to accomplish this.
    /// </summary>
    public abstract void Pause();

    /// <summary>
    /// Checks whether the <see cref="SnapState"/> stack contains elements.
    /// </summary>
    /// <returns></returns>
    public bool HasStates() {
        return states.Count > 0;
    }
}

/// <summary>
/// A representation of a gameObject's current state.
/// </summary>
/// <remarks>
/// Currently contains
/// <list type="bullet">
/// <item>Frame Number</item>
/// <item>Position</item> 
/// <item>Rotation</item>
/// <item>Velocity</item>
/// <item>Angular Velocity</item>
/// <item>is On?</item>
/// </list>
/// </remarks>
public class SnapState {
    public int frameNumber;

    public Vector3 position;
    public Quaternion rotation;

    public Vector3 velocity;
    public Vector3 angularVelocity;

    public bool isOn;
}
