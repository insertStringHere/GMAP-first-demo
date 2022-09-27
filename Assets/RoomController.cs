using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{   
    private IEnumerable<Animator> animators;
    private float animTime;

    [SerializeField] private float maxTime;

    void Start()
    {
        animators = GetComponentsInChildren<Animator>();
    }
        
    void Update()
    {
        if (Input.GetKey(KeyCode.Q) && animTime > 0) {
            animTime -= Time.deltaTime; 
        } else if(animTime < maxTime){
            animTime += Time.deltaTime;
        }

        foreach(Animator anim in animators){
            anim.SetFloat("Time", animTime);
        }
    }
}
