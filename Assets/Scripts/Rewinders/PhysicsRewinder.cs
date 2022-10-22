using UnityEngine;

/// <summary>
/// An implementation of <see cref="IRewinder"/> made for operating with
/// <see cref="Rigidbody"/> physics.
/// </summary>
public class PhysicsRewinder : IRewinder {
    /// <summary>
    /// The initial force that should be applied to the
    /// <see cref="Rigidbody"/> when time starts playing.
    /// </summary>
    public Vector3 startForce = Vector3.zero;

    /// <summary>
    /// A cached state of the previously popped <see cref="SnapState"/>'s velocity; used
    /// for calculating the force imparted onto any non-rewound object while
    /// rewinding time (primarily the plyaer)
    /// </summary>
    private Vector3 rewindVelocity;
    /// <summary>
    /// The attachee <see cref="GameObject"/>'s attached <see cref="Rigidbody"/>.
    /// </summary>
    public Rigidbody rb;

    /// <summary>
    /// Initializes the <see cref="Rigidbody"/> if not set in the editor.
    /// <para/>
    /// <inheritdoc/>
    /// </summary>
    /// 
    public override void Start() {
        rb = rb != null ? rb : GetComponent<Rigidbody>();

        base.Start();
    }

    /// <summary>
    /// <inheritdoc/>
    /// <para/>
    /// Also stores the <see cref="Rigidbody"/>'s current
    /// velocity and current angular velocity.
    /// </summary>
    public override void Store() {
        base.Store();

        // If the state list is empty, p an i c
        SnapState s = states.Peek();
        s.velocity = rb.velocity;
        s.angularVelocity = rb.angularVelocity;
    }

    /// <summary>
    /// <inheritdoc/>
    /// <para/>
    /// Sets the rewind velocity to the popped <see cref="SnapState"/>'s
    /// velocity.
    /// </summary>
    /// <returns><inheritdoc/></returns>
    public override SnapState RewindState() {
        SnapState state = base.RewindState();
        rewindVelocity = state?.velocity ?? Vector3.zero;
        return state;
    }

    /// <summary>
    /// Updates the rewindVelocity every frame, so long as the the
    /// <see cref="Rigidbody"/> is not in kinematic mode.
    /// </summary>
    public void Update() {
        if (!rb.isKinematic)
            rewindVelocity = rb.velocity;
    }


    /// <summary>
    /// Continues physics for the object, and begins by applying the
    /// force if there is no history within the <see cref="SnapState"/> stack.
    /// If there is a state within the stack, attempts to resume movement by applying
    /// the previous state's velocity
    /// </summary>
    public override void Play() {
        if (rb.isKinematic) 
            rb.isKinematic = false;
        // If there are no states, we're at the beginning of movement and should
        // apply the initial force.
        if (states.Count == 0) {
            if (printDebug)
                Debug.Log($"{name} state at 0, applying force");
            rb.AddRelativeForce(startForce, ForceMode.Impulse);
            Store();
        // Otherwise, that means there's at least one state we can capture
        } else {
            SnapState state = states.Peek();
            rb.velocity = state.velocity;
            rb.angularVelocity = state.angularVelocity;
        }

    }

    /// <summary>
    /// Halts physics for the object and sets the <see cref="Rigidbody"/> to kinematic mode.
    /// </summary>
    public override void Pause() {
        if (!rb.isKinematic)
            rb.isKinematic = true;
        
    }

    /// <summary>
    /// In the case of collision while the <see cref="GameObject"/> is rewinding, a force needs to
    /// be applied to the colliding object since the <see cref="Rigidbody"/> cannot do it for itself in
    /// kinematic mode.
    /// </summary>
    /// <remarks>
    /// Using the difference between the next state's velocity and the cached rewind velocity,
    /// the delta is multiplied by the Rigidbody's mass and applied to the colliding Rigidbody
    /// as a force. In this implementation, the change in time is ignored because it produces better results.
    /// </remarks>
    /// <param name="collision">A <see cref="Collision"/> containing data about the colliding body.</param>
    public void OnCollisionStay(Collision collision) {
        if (rb.isKinematic && collision.rigidbody != null) {
            // F = ma = m * (dv/dt)
            var force = rb.mass * (
                    (states.TryPeek(out var state) ? Vector3.zero : state.velocity) - rewindVelocity);

            if (printDebug)
                Debug.Log($"Collision with physics object; force = {force}, normal: {collision.contacts[0].normal}");

            collision.rigidbody.AddForce(force, ForceMode.Force);
        }
    }
}