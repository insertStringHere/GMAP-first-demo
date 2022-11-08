using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingRocksObstacle : MonoBehaviour
{
    public float moveSpeed = 0.05f;
    public float timeout = 2f;
    void Start() ///place script on the prefab that SpawningRocks script will spawn.
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
        yield return new WaitForSeconds(timeout); ///destroys obstacles after they have already fallen, after they already 
        Destroy(this.gameObject);
    }

    }
    ///<summary>
    /// Place this script on the prefab that SpawningRocks script will spawn
    /// This destroys the game object after it has been spawned and is no longer moving, so once its fallen, and it will also kill the player upon collision
    ///</summary>

