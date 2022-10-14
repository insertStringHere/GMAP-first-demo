using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;
using TMPro;
using UnityEngine.Rendering;

public class DebugPanel : MonoBehaviour
{
    public TMP_InputField Gravity;
    public TMP_InputField RewindSpeed;
    public TMP_InputField Drag;
    public List<TMP_InputField> Move;
    public List<TMP_InputField> Speed;

    public PhysicsPlayerController ppc;
    public ZController zc;

    public GameObject panel;

    void Start()
    {
        panel ??= GetComponentInChildren<Image>(true).gameObject;

        var components = GetComponentsInChildren<TMPro.TMP_InputField>(true);
        Gravity ??= components.FirstOrDefault(p => p.name == "Gravity");
        RewindSpeed ??= components.FirstOrDefault(p => p.name == "RewindSpeed");
        Drag ??= components.FirstOrDefault(p => p.name == "Drag");
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


        Gravity.text = $"{Physics.gravity.y}";
        RewindSpeed.text = $"{zc.RecordInterval}";
        Drag.text = $"{ppc.gameObject.GetComponent<Rigidbody>().drag}";

        foreach(var p in Move) {
            p.text = ($"{new Vector3().GetType().GetField(p.name.ToLower()).GetValue(ppc.playerAcceleration)}"); 
        }
        foreach(var p in Speed) {
            p.text = ($"{new Vector3().GetType().GetField(p.name.ToLower()).GetValue(ppc.maxSpeed)}"); 
        }
        
    }
    
    void Update()
    {
        if(panel.activeSelf) {
            Physics.gravity = new Vector3(0, float.TryParse(Gravity.text, out float f) ? f : Physics.gravity.y);
            zc.RecordInterval = float.TryParse(RewindSpeed.text, out f) ? f : zc.RecordInterval;
            ppc.gameObject.GetComponent<Rigidbody>().drag = float.TryParse(Drag.text, out f) ? f : ppc.gameObject.GetComponent<Rigidbody>().drag;

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
            if(panel.active = !panel.active) {
                Cursor.lockState = CursorLockMode.Confined;
            } else {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }
}
