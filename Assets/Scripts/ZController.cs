using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SearchService;

public class ZController : MonoBehaviour {
    private IEnumerable<Rewind> Rewinds;

    public float RecordInterval = .2f;
    private float DeltaTime;

    public float RewindScale = 2f;

    public bool Active;

    // Start is called before the first frame update
    void Start() {
        Active = false; 
        DeltaTime = 0;
        if (RewindScale <= 0) RewindScale = 1f;
        Rewinds = GetComponentsInChildren<Rewind>();
    }

    void Update() {
        if (Active) {
            DeltaTime += Time.deltaTime;
            if (DeltaTime >= (RecordInterval / RewindScale) && (Input.GetMouseButton(0) || Input.GetKey(KeyCode.Q))) {
                foreach (Rewind r in Rewinds)
                    r.RewindState();
                DeltaTime = 0;
            } else if (DeltaTime >= RecordInterval && Rewinds.Any(r => r.NeedUpdate())) {
                foreach (Rewind r in Rewinds)
                    r.Store();
                DeltaTime = 0;
            }

            if (Input.GetKeyUp(KeyCode.Q) || Input.GetMouseButtonUp(0))
                foreach (Rewind r in Rewinds)
                    r.Play();
        }
    }

    private void OnTriggerEnter(Collider other) {
        foreach (ZController z in transform.parent?.GetComponentsInChildren<ZController>()) {
            z.Pause();
            z.Active = false;
        }

        foreach (Rewind r in Rewinds)
            r.Play();

        Active = true;
    }

    public void Pause() {
        foreach (Rewind r in Rewinds)
            r.Pause();
    }
}
