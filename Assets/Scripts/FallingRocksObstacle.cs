using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingRocksObstacle : MonoBehaviour
{
    public float moveSpeed = 0.05f;
    public float timeout = 2f;
    void Start()
    {
        StartCoroutine(Timeout());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(0, -moveSpeed, 0);
    }
    IEnumerator Timeout()
    {
        yield return new WaitForSeconds(timeout);
        Destroy(this.gameObject);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(collision.gameObject);
        }
    }
}
