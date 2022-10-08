using System.Collections.Generic;
using UnityEngine;

public class Rewind : IRewinder {
    public Vector3 StartForce;
    protected bool RigidOff;

    private Rigidbody rb;


    public override void Start() {
        rb = GetComponent<Rigidbody>();
        base.Start();
    }

    public override void Store() {
        base.Store();

        SnapState s = States.Pop();
        s.velocity = rb.velocity;
        s.angularVelocity = rb.angularVelocity;
        States.Push(s);
    }

    /// <summary>
    /// Continues physics for the object, and begins by applying the
    /// force if there is no history.
    /// </summary>
    public override void Play() {
        if (RigidOff) 
            RigidOff = rb.isKinematic = false;

        if (States.Count == 0) {
            if (printDebug)
                Debug.Log($"{name} state at 0, applying force");
            rb.AddRelativeForce(StartForce, ForceMode.Impulse);
            Store();
        } else {
            SnapState state = States.Peek();
            rb.velocity = state.velocity;
            rb.angularVelocity = state.angularVelocity;
        }

    }

    /// <summary>
    /// Halts physics for the object
    /// </summary>
    public override void Pause() {
        if (!RigidOff) 
            RigidOff = GetComponent<Rigidbody>().isKinematic = true;
        
    }
}