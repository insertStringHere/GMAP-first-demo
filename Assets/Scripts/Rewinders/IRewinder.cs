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
        bool needUpdate = states.TryPeek(out SnapState state) && !(zController.Approximate(state.position, transform.position) && zController.Approximate(state.rotation.eulerAngles, transform.rotation.eulerAngles));
        if (printDebug && needUpdate)
            Debug.Log($"{name} needs movement update");
        return needUpdate;
    }

    /// <summary>
    /// Stores the current position and rotation of the object on the
    /// rewind stack.
    /// </summary>
    public virtual void Store() {
        SnapState s = new SnapState {
            position = transform.position,
            rotation = transform.rotation
        };

        states.Push(s);
    }

    /// <summary>
    /// Rewinds the states from the stack frame one state at a time,
    /// allowing for easier speed-up of rewind. If there are no states left,
    /// the function returns null
    /// </summary>
    /// <returns>The popped <see cref="SnapState"/> or null if there are no states remaining.</returns>
    public virtual SnapState RewindState() {
        SnapState state = null;

        if (states.Count > 0) {
            Pause();
            state = states.Pop();
            transform.SetPositionAndRotation(state.position, state.rotation);

            if (printDebug)
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
/// <item>Position</item> 
/// <item>Rotation</item>
/// <item>Velocity</item>
/// <item>Angular Velocity</item>
/// </list>
/// </remarks>
public class SnapState {
    public Vector3 position;
    public Quaternion rotation;

    public Vector3 velocity;
    public Vector3 angularVelocity;
}
