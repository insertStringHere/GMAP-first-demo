using UnityEngine;

/// <summary>
/// An implementation of <see cref="IRewinder"/> made for operating with
/// the water kill plane.
/// </summary>
public class WaterRewinder : IRewinder {

    /// <summary>
    /// Speed of the rising water kill plane
    /// </summary>
    public float waterRiseSpeed = 10;
    /// <summary>
    /// Lockout to prevent the water rising during rewind and pause
    /// </summary>
    public bool waterRiseLockout = false;
    
    public override void Start() {
        base.Start();
    }

    /// <summary>
    /// Updates the plane's y position by waterRiseSpeed / 100 units every frame
    /// </summary>
    public void Update() {
        if (!waterRiseLockout) {
            transform.Translate(new Vector3(0, waterRiseSpeed / 100, 0) * Time.deltaTime);
        }
    }


    /// <summary>
    /// Resume normal water rising
    /// </summary>
    public override void Play() {
        waterRiseLockout = false;
        if (states.Count == 0) {
            if (printDebug)
                Debug.Log($"{name} state at 0");
            Store();
        // Otherwise, that means there's at least one state we can capture
        } else {
            SnapState state = states.Peek();
        }

    }

    /// <summary>
    /// Sets the waterRiseLockout and prevents water from rising
    /// </summary>
    public override void Pause() {
        waterRiseLockout = true;
    }
}