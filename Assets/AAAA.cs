using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AAAA : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator animator;
    private float time;

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.Play("New Animation");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A) && time > 0) {
            time -= Time.deltaTime; 
        } else if(time < 1){
            time += Time.deltaTime;
        }

        animator.SetFloat("Time", time);
    }
}
