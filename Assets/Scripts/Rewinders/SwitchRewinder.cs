using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An implementation of <see cref="IRewinder"/> made for operating with binary switches.
/// </summary>
public class SwitchRewinder : IRewinder {
    /// <summary>
    /// The GameObject to toggle active when the switch is interacted with.
    /// </summary>
    public GameObject objToToggle;
    /// <summary>
    /// Boolean value. True if the switch is currently on (objToToggle inactive).
    /// </summary>
    public bool isOn;

    /// <summary>
    /// Initializes the state of the switch.
    /// <summary/>
    private void Start() {
        isOn = false;
        base.Start();
    }

    /// <summary>
    /// Store the current state of the switch when recording.
    /// </summary>
    public override void Store() {
        frameNumber++;
        if (states.Count == 0) {
            SnapState n = new SnapState {
                frameNumber = this.frameNumber,
                isOn = this.isOn
            };
            states.Push(n);
        } else {
            SnapState state = states.Peek();
            if (this.isOn != state.isOn) {
                SnapState n = new SnapState {
                    frameNumber = this.frameNumber,
                    isOn = this.isOn
                };

                states.Push(n);
            }
        }
    }

    /// <summary>
    /// Restore the state of the switch when rewinding.
    /// </summary>
    public override SnapState RewindState(int count) {
        SnapState state = null;

        if (states.Count > 0) {
            while (states.Count > 0 && count >= 0) {
                if(states.Peek().frameNumber == frameNumber)
                    state = states.Pop();
                frameNumber--;
                count--;
            }
            
            if(state != null)
                this.isOn = state?.isOn ?? false;
                objToToggle.SetActive(!this.isOn);

            if (printDebug && state != null)
                Debug.Log($"{name} popping and applying state {states.Count + 1}");
        }
        return state;
    }

    /// <summary>
    /// Check for input when player is near switch.
    /// </summary>
    public void OnTriggerStay(Collider other) {
        if (Input.GetKeyDown(KeyCode.E)) {
            Debug.Log("Switch pressed");
            isOn = !isOn;
            objToToggle.SetActive(!this.isOn);
        }
    }

    public override void Play() {
        SnapState state = states.Peek();
        this.isOn = state.isOn;
        objToToggle.SetActive(!this.isOn);
        return;
    }

    public override void Pause() {
        return;
    }
}
