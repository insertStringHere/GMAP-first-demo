using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;
using TMPro;
using UnityEngine.Rendering;

/// <summary>
/// A controller for the debug settings. Currently contains a manually maintained
/// map of UI elements to the corresponding object value.
/// </summary>
/// <remarks>
/// Not as automated as I'd like but that
/// would be difficult to do without reflection, which is already used slightly in
/// instantiation.
/// </remarks>
public class DebugPanel : MonoBehaviour
{
    /// <summary>
    /// A text field representing gravity's value
    /// </summary>
    public TMP_InputField Gravity;
    /// <summary>
    /// A text field representing how often per second a 
    /// rewind capture is performed
    /// </summary>
    public TMP_InputField RewindSpeed;
    /// <summary>
    /// A text field representing drag
    /// </summary>
    /// <remarks>
    /// Possibly could be removed now
    /// </remarks>
    public TMP_InputField Drag;
    /// <summary>
    /// A list of input fields corresponding to the three
    /// values of a player's movement acceleration
    /// </summary>
    public List<TMP_InputField> Move;
    /// <summary>
    /// A list of input fields corresponding to the three
    /// values of a player's max velocity
    /// </summary>
    public List<TMP_InputField> Speed;
    /// <summary>
    /// A text field representing how long rewind can be held
    /// before time resumes.
    /// </summary>
    public TMP_InputField RLimit;
    /// <summary>
    /// A text field representing how long the player must wait
    /// after burning out the rewind before they can use it again.
    /// </summary>
    public TMP_InputField RCooldown;
    /// <summary>
    /// The leniency for the <see cref="ZController"/>'s check for no motion.
    /// </summary>
    public TMP_InputField Leniency;

    /// <summary>
    /// The <see cref="PhysicsPlayerController"/> the fields are referencing.
    /// </summary>
    public PhysicsPlayerController ppc;
    /// <summary>
    /// The <see cref="ZController"/> the fields are referencing 
    /// </summary>
    public ZController zc;

    /// <summary>
    /// The parent container for all the text fields.
    /// </summary>
    public GameObject panel;

    /// <summary>
    /// Set up all of the elements. If they have not been set in the editor,
    /// they will be extracted from the children of the panel <see cref="GameObject"/>.
    /// For each element, the corresponding value is pulled from game data and populated
    /// into the text fields.
    /// </summary>
    void Start()
    {
        // Set up all elements
        panel ??= GetComponentInChildren<Image>(true).gameObject;

        var components = GetComponentsInChildren<TMPro.TMP_InputField>(true);
        Gravity ??= components.FirstOrDefault(p => p.name == "Gravity");
        RewindSpeed ??= components.FirstOrDefault(p => p.name == "RewindSpeed");
        Drag ??= components.FirstOrDefault(p => p.name == "Drag");
        RLimit ??= components.FirstOrDefault(p => p.name == "RewindLimit");
        RCooldown ??= components.FirstOrDefault(p => p.name == "RewindCooldown");
        Leniency ??= components.FirstOrDefault(p => p.name == "Leniency");
        Move = new List<TMP_InputField>();
        Speed = new List<TMP_InputField>();
         
        foreach(var p in GetComponentsInChildren<TMP_Text>(true).FirstOrDefault(p => p.name == "Speed").GetComponentsInChildren<TMP_InputField>(true)) {
            Speed.Add(p);
        } 
        foreach(var p in GetComponentsInChildren<TMP_Text>(true).FirstOrDefault(p => p.name == "Move").GetComponentsInChildren<TMP_InputField>(true)) {
            Move.Add(p);
        }

        ppc ??= FindObjectOfType<PhysicsPlayerController>();
        zc ??= FindObjectOfType<ZController>();

        // Now, for each object, set their initial values from the game:
        Gravity.text = $"{Physics.gravity.y}";
        RewindSpeed.text = $"{zc.recordInterval}";
        Drag.text = $"{ppc.gameObject.GetComponent<Rigidbody>().drag}";
        RLimit.text = $"{zc.rewindLimit}";
        RCooldown.text = $"{zc.rewindCooldown}";
        Leniency.text = $"{zc.approximateLeniency}";

        foreach(var p in Move) {
            p.text = ($"{new Vector3().GetType().GetField(p.name.ToLower()).GetValue(ppc.playerAcceleration)}"); 
        }
        foreach(var p in Speed) {
            p.text = ($"{new Vector3().GetType().GetField(p.name.ToLower()).GetValue(ppc.maxSpeed)}"); 
        }
        
    }
    
    /// <summary>
    /// Every frame, so long as the panel itself is active, all values in the gui will
    /// be copied into their corresponding fields in the game. This is done through TryParse calls, where
    /// a failure will result in the original value being maintained. If the key (f1) for the debug is called,
    /// the panel will be toggled.
    /// </summary>
    void Update()
    {
        if(panel.activeSelf) {
            Physics.gravity = new Vector3(0, float.TryParse(Gravity.text, out float f) ? f : Physics.gravity.y);
            zc.recordInterval = float.TryParse(RewindSpeed.text, out f) ? f : zc.recordInterval;
            ppc.gameObject.GetComponent<Rigidbody>().drag = float.TryParse(Drag.text, out f) ? f : ppc.gameObject.GetComponent<Rigidbody>().drag;
            zc.rewindLimit = float.TryParse(RLimit.text, out f) ? f : zc.rewindLimit;
            zc.rewindCooldown = float.TryParse(RCooldown.text, out f) ? f : zc.rewindCooldown;
            zc.approximateLeniency = float.TryParse(Leniency.text, out f) ? f : zc.approximateLeniency;

            ppc.maxSpeed = new Vector3(
                float.TryParse(Speed[0].text, out f) ? f : ppc.maxSpeed.x,
                float.TryParse(Speed[1].text, out f) ? f : ppc.maxSpeed.y,
                float.TryParse(Speed[2].text, out f) ? f : ppc.maxSpeed.z
            );
            ppc.playerAcceleration = new Vector3(
                float.TryParse(Move[0].text, out f) ? f : ppc.playerAcceleration.x,
                float.TryParse(Move[1].text, out f) ? f : ppc.playerAcceleration.y,
                float.TryParse(Move[2].text, out f) ? f : ppc.playerAcceleration.z
            );
        }

        if (Input.GetKeyDown(KeyCode.F1)) {
            // Die in a pit, unity.
            if(panel.active = !panel.active) {
                Cursor.lockState = CursorLockMode.Confined;
            } else {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }
}
