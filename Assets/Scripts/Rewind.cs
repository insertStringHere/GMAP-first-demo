using System.Collections.Generic;
using UnityEngine;

public class Rewind : MonoBehaviour {
    protected Vector3 StartPos { get; private set; }
    protected Quaternion StartRot { get; private set; }

    public Vector3 StartForce;

    protected Stack<SnapState> States;
    protected bool RigidOff; 


    [SerializeField]
    private bool printDebug;

    void Start() {
        States = new Stack<SnapState>();

        StartPos = transform.position;
        StartRot = transform.rotation;

        Pause();
    }

    /// <summary>
    /// Checks to see if the object has moved since the last state capture.
    /// </summary>
    /// <returns>True if the object has moved</returns>
    public bool NeedUpdate() {
        bool needUpdate = States.TryPeek(out SnapState state) && !(state.position.Equals(transform.position) || state.rotation.Equals(transform.rotation));
        if (printDebug && needUpdate)
            Debug.Log($"{name} needs movement update");
        return needUpdate;
    }

    /// <summary>
    /// Stores the current state of the object to the rewind stack
    /// </summary>
    public void Store() {
        States.Push(new SnapState {
            position = transform.position,
            rotation = transform.rotation
        });
    } 

    /// <summary>
    /// Continues physics for the object, and begins by applying the
    /// force if there is no history.
    /// </summary>
    public void Play() {
        Rigidbody rb = GetComponent<Rigidbody>();

        if (RigidOff) 
            RigidOff = rb.isKinematic = false;

        if (States.Count == 0) {
            if (printDebug)
                Debug.Log($"{name} state at 0, applying force");
            rb.AddRelativeForce(StartForce, ForceMode.Impulse);
            Store();
        }

    }

    /// <summary>
    /// Halts physics for the object
    /// </summary>
    public void Pause() {
        if (!RigidOff) 
            RigidOff = GetComponent<Rigidbody>().isKinematic = true;
        
    }

    /// <summary>
    /// Rewinds the states from the stack frame one state at a time,
    /// allowing for easier speed-up of rewind. Once the stack is empty,
    /// the physics will resume.
    /// </summary>
    public void RewindState() {
        SnapState state;

        if (States.Count > 0) {
            Pause();
            state = States.Pop();
            transform.SetPositionAndRotation(state.position, state.rotation);

            if (printDebug)
                Debug.Log($"{name} popping and applying state {States.Count + 1}");
        }
    }

    public void Reset() {
        transform.position = StartPos;
        transform.rotation = StartRot;
        States.Clear();
        Pause();
    }

}

public struct SnapState {
    public Vector3 position;
    public Quaternion rotation;
}
