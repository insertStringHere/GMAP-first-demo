using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;

/// <summary>
/// A controller for textboxes. Part of the textboxes prefab.
/// Textbox appears on canvas layer when collision with player detected.
/// Has support for multiple lines one after another, advanced with the Space key.
/// Does not reappear if trigger is collided with again.
/// </summary>
public class TextboxScript : MonoBehaviour
{
    /// <summary>
    /// The GameObject containing the Textbox Canvas conponent
    /// </summary>
    private GameObject TextboxLayer;
    /// <summary>
    /// The Textbox in the Canvas UI Layer to write the text to
    /// </summary>
    private TextMeshProUGUI Textbox;
    /// <summary>
    /// A list of strings to display in the textbox.
    /// They are advanced through with the Space key.
    /// </summary>
    public List<string> text;
    /// <summary>
    /// Time in seconds before the textbox auto-advances
    /// </summary>
    public float timeToAdvance = 5;
    /// <summary>
    /// A flag to keep track of if the textbox has been seen.
    /// Prevents the textbox from reoccuring if the collider is triggered again.
    /// </summary>
    public bool hasBeenSeen = false;

    void Start() {
        TextboxLayer = GameObject.FindGameObjectsWithTag("TextboxLayer")[0];
        Textbox = TextboxLayer.GetComponentInChildren<TextMeshProUGUI>();
        TextboxLayer.GetComponent<Canvas>().enabled = false;
    }

    /// <summary>
    /// Function for when the player collides with the trigger.
    /// </summary>
    private void OnTriggerEnter(Collider other) {
        if (other.transform.parent.tag == "Player" && !hasBeenSeen) {
            StartCoroutine(DisplayTextCoroutine());
        }
        hasBeenSeen = true;
    }

    IEnumerator DisplayTextCoroutine() {
        for (int i = 0; i < text.Count; i++) {
            Textbox.text = text[i];
            TextboxLayer.GetComponent<Canvas>().enabled = true;
            yield return new WaitForSeconds(timeToAdvance);
            TextboxLayer.GetComponent<Canvas>().enabled = false;
        }
    }
}
