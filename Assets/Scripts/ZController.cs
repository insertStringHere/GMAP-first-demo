using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SearchService;

public class ZController : MonoBehaviour {
    private IEnumerable<IRewinder> Rewinds;

    public float RecordInterval = .2f;
    [SerializeField] private float DeltaTime;

    public float RewindScale = 2f;
    public float RewindLimit = -1f;

    public float RewindCooldown = 2f;
    [SerializeField] private float cooldown = 0f;

    [SerializeField] private float approximateLeniency = .01f;    

    public bool Active;

    // Start is called before the first frame update
    void Start() {
        DeltaTime = 0;
        if (RewindScale <= 0) RewindScale = 1f;
        Rewinds = GetComponentsInChildren<PhysicsRewinder>();
    }

    void Update() {
        if (Active) {
            DeltaTime += Time.deltaTime;
                
            if(cooldown > 0) {
                cooldown -= Time.deltaTime;
            }

            if (DeltaTime >= (RecordInterval / RewindScale) && (Input.GetMouseButton(0) || Input.GetKey(KeyCode.Q)) && cooldown <= 0) {
                if (Rewinds.FirstOrDefault()?.HasStates() ?? false) {
                    foreach (IRewinder r in Rewinds) if (r.isActiveAndEnabled)
                            r.RewindState();
                    DeltaTime = 0;
                } else if(DeltaTime > RewindLimit && RewindLimit >= 0) {
                    cooldown = RewindCooldown;
                    foreach (IRewinder r in Rewinds) if (r.isActiveAndEnabled)
                            r.Play();
                    DeltaTime = 0;
                }
                
            } else if (DeltaTime >= RecordInterval && Rewinds.Any(r => r.NeedUpdate())) {
                foreach (IRewinder r in Rewinds) if(r.isActiveAndEnabled)
                    r.Store();
                DeltaTime = 0;
            }

            if (Input.GetKeyUp(KeyCode.Q) || Input.GetMouseButtonUp(0))
                foreach (IRewinder r in Rewinds) if(r.isActiveAndEnabled)
                    r.Play();
        }
    }

    private void OnTriggerEnter(Collider other) {
        foreach (ZController z in transform.parent?.GetComponentsInChildren<ZController>()) {
            z.Pause();
            z.Active = false;
        }

        foreach (IRewinder r in Rewinds) {
            r.enabled = true;
            r.Play();
        }

        Active = true;
    }

    public void Pause() {
        foreach (IRewinder r in Rewinds)
            r.Pause();
    }


    public bool Approximate(Vector3 a, Vector3 b)
    {
        return Mathf.Abs(a.x - b.x) < approximateLeniency &&
               Mathf.Abs(a.y - b.y) < approximateLeniency &&
               Mathf.Abs(a.z - b.z) < approximateLeniency;
    }
}
