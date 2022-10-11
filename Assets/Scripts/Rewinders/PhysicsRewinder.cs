using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PhysicsRewinder : IRewinder {
    public Vector3 StartForce;
    protected bool RigidOff;

    private Rigidbody rb;
    private Vector3 rewindVelocity;


    public override void Start() {
        rb = GetComponent<Rigidbody>();
        base.Start();
    }

    public override bool NeedUpdate(ZController controller) {
        bool needUpdate = base.NeedUpdate(controller);
        if(!needUpdate)
            rewindVelocity = Vector3.zero;
        return needUpdate;
    }

    public override void Store(ZController controller) {
        base.Store(controller);

        SnapState s = States.Pop();
        s.velocity = rb.velocity;
        s.angularVelocity = rb.angularVelocity;
        States.Push(s);
    }

    public override SnapState RewindState(ZController controller) {
        SnapState state = base.RewindState(controller);
        rewindVelocity = -state.velocity / (controller.RecordInterval / controller.RewindScale);
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
            Store(null);
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
        if (!Approximate(rewindVelocity, Vector3.zero) && collision.rigidbody != null) {
            if (printDebug)
                Debug.Log($"Collision with physics object; launch velocity = {rewindVelocity}, collision normal = {collision.contacts[0].normal}");
            collision.rigidbody?.AddForce((rewindVelocity + -1 * collision.contacts[0].normal), ForceMode.VelocityChange);
        }
    }
}