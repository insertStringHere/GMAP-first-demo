using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PhysicsRewinder : IRewinder {
    public Vector3 StartForce;
    protected bool RigidOff;

    private Vector3 rewindVelocity;
    public Rigidbody rb;
    public ZController myController;


    public override void Start() {
        rb = rb != null ? rb : GetComponent<Rigidbody>();
        myController = myController != null ? myController : GetComponentInParent<ZController>();

        base.Start();
    }

    public override bool NeedUpdate() {
        bool needUpdate = base.NeedUpdate();
        rewindVelocity = rb.velocity;
        return needUpdate;
    }

    public override void Store() {
        base.Store();

        SnapState s = States.Pop();
        s.velocity = rb.velocity;
        s.angularVelocity = rb.angularVelocity;
        States.Push(s);
    }

    public override SnapState RewindState() {
        SnapState state = base.RewindState();
        rewindVelocity = state.velocity;
        return state;
    }

    public void Update() {
        if (rb.velocity != Vector3.zero)
            rewindVelocity = rb.velocity;
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

    public void OnCollisionStay(Collision collision) {
        if (rb.isKinematic && collision.rigidbody != null) {
            // F = ma = m * (dv/dt)
            var force = rb.mass * (
                    (States.TryPeek(out var state) ? Vector3.zero : state.velocity) - rewindVelocity);

            if (printDebug)
                Debug.Log($"Collision with physics object; force = {force}, normal: {collision.contacts[0].normal}");

            collision.rigidbody.AddForce(force, ForceMode.Acceleration);
        }
    }
}