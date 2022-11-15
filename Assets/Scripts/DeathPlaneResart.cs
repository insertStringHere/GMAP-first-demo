using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class DeathPlaneCollision : MonoBehaviour
{
    [SerializeField] private LevelLoader l;
    [SerializeField] private GameObject p;
    public void Start()
    {
        l = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();
        p = GameObject.Find("PlayerV2");
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player found");
            //p.transform.parent.rotation = Quaternion.Euler(new Vector3(p.transform.rotation.x, p.transform.rotation.y, -90f));
            //other.transform.rotation = Quaternion.Euler(other.transform.rotation.x, other.transform.rotation.y, -90f);
            l.ReloadScene();
        }
    }
}
