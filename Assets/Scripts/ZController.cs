using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ZController : MonoBehaviour {
    private IEnumerable<Rewind> Rewinds;

    public float RecordInterval = .2f;
    private float DeltaTime;

    public float RewindScale = 2f;

    // Start is called before the first frame update
    void Start() {
        DeltaTime = 0;
        if (RewindScale <= 0) RewindScale = 1f;
        Rewinds = GetComponentsInChildren<Rewind>();
    }

    void FixedUpdate() {
        DeltaTime += Time.fixedDeltaTime;
        if (DeltaTime >= (RecordInterval / RewindScale) && Input.GetMouseButton(0)) {
            foreach (Rewind r in Rewinds)
                r.RewindState();
            DeltaTime = 0;
        } else if (DeltaTime >= RecordInterval && Rewinds.Any(r => r.NeedUpdate())) {
            foreach (Rewind r in Rewinds)
                r.Store();
            DeltaTime = 0;
        }

        if (Input.GetMouseButtonUp(0))
            foreach (Rewind r in Rewinds)
                r.Play();
    }
}
