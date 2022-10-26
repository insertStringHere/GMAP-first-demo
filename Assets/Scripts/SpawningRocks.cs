using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningRocks : MonoBehaviour
{
    public GameObject rockPrefab;
    void Start() 
    {
        StartCoroutine(SpawnFalling());
    }

    IEnumerator SpawnFalling() //Put this script on an empty game object to spawn rocks from it
    {
        while (true)
        {
            float randomTime = Random.Range(1f, 2f); //time it waits between spawning new prefabs
            float setPosition = Random.Range(-50f, -30f); //spawns prefabs in range for x position depending on where you place the empty game object
            Instantiate(rockPrefab, new Vector3(setPosition, transform.position.y, transform.position.z), Quaternion.identity);
            yield return new WaitForSeconds(randomTime);
            Instantiate(rockPrefab, new Vector3(setPosition, transform.position.y, transform.position.z), Quaternion.identity); //repeated to spawn multiple at the same time
            float randomTime2 = Random.Range(1f, 3f);
            float setPosition2 = Random.Range(-30f, -80f); //different set of numbers to spawn other rocks at the same time but in a different location
            Instantiate(rockPrefab, new Vector3(setPosition2, transform.position.y, transform.position.z-2), Quaternion.identity);
            yield return new WaitForSeconds(randomTime2);
            Instantiate(rockPrefab, new Vector3(setPosition2, transform.position.y, transform.position.z+2), Quaternion.identity);
        }
    }
}
