using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A controller for textboxes. Part of the textboxes prefab.
/// Textbox appears on canvas layer when collision with player detected.
/// Has support for multiple lines one after another, advanced with the Space key.
/// Does not reappear if trigger is collided with again.
/// </summary>
public class TextboxScript : MonoBehaviour
{
    /// <summary>
    /// A list of strings to display in the textbox.
    /// They are advanced through with the Space key.
    /// </summary>
    public List<string> text;
    /// <summary>
    /// A flag to keep track of if the textbox has been seen.
    /// Prevents the textbox from reoccuring if the collider is triggered again.
    /// </summary>
    public bool hasBeenSeen = false;
    /// <summary>
    /// Function for when the player collides with the trigger.
    /// </summary>
    private void OnTriggerEnter(Collider other) {
        if (other.transform.parent.tag == "Player" && !hasBeenSeen) {
            for (int i = 0; i < text.Count; i++) {
                Debug.Log(text[i]);
            }
            hasBeenSeen = true;
        }
    }
}
