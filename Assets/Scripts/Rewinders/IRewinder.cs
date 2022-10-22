using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class IRewinder : MonoBehaviour {
    public Vector3 StartPos { get; protected set; }
    public Quaternion StartRot { get; protected set; }

    public ZController zController;

    protected Stack<SnapState> States;


    [SerializeField]
    protected bool printDebug;

    public virtual void Start() {
        States = new Stack<SnapState>();

        StartPos = transform.position;
        StartRot = transform.rotation;

        zController = zController is null ? GetComponentInParent<ZController>() : zController;

        Pause();
    }

    /// <summary>
    /// Checks to see if the object has moved since the last state capture.
    /// </summary>
    /// <returns>True if the object has moved</returns>
    public virtual bool NeedUpdate() {
        bool needUpdate = States.TryPeek(out SnapState state) && !(zController.Approximate(state.position, transform.position) && zController.Approximate(state.rotation.eulerAngles, transform.rotation.eulerAngles));
        if (printDebug && needUpdate)
            Debug.Log($"{name} needs movement update");
        return needUpdate;
    }

    /// <summary>
    /// Stores the current state of the object to the rewind stack
    /// </summary>
    public virtual void Store() {
        SnapState s = new SnapState {
            position = transform.position,
            rotation = transform.rotation
        };

        States.Push(s);
    }

    /// <summary>
    /// Rewinds the states from the stack frame one state at a time,
    /// allowing for easier speed-up of rewind.
    /// </summary>
    public virtual SnapState RewindState() {
        SnapState state = new SnapState { Null = 1 };

        if (States.Count > 0) {
            Pause();
            state = States.Pop();
            transform.SetPositionAndRotation(state.position, state.rotation);

            if (printDebug)
                Debug.Log($"{name} popping and applying state {States.Count + 1}");
        }

        return state;
    }
    public virtual void Reset() {
        transform.position = StartPos;
        transform.rotation = StartRot;
        States.Clear();
        Pause();
    }
    /// <summary>
    /// Continues time wherever it was left off
    /// </summary>
    public abstract void Play();

    /// <summary>
    /// Pauses the movement of the object
    /// </summary>
    public abstract void Pause();

    public bool HasStates() {
        return States.Count > 0;
    }
}

public struct SnapState {
    public byte Null;

    public Vector3 position;
    public Quaternion rotation;

    public Vector3 velocity;
    public Vector3 angularVelocity;
}
