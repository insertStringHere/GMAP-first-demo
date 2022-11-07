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
            float randomTime = Random.Range(1f, 3f); //time it waits between spawning new prefabs
            float setPositionZ = Random.Range(-152f, -140f); //spawns prefabs in range for z position depending on where you place the empty game object
            float setPositionX = Random.Range(-164f, -160f);
            Instantiate(rockPrefab, new Vector3(setPositionX, transform.position.y, setPositionZ), Quaternion.identity);
            yield return new WaitForSeconds(randomTime);
            float randomTime2 = Random.Range(1f, 2f);
            float setPositionZ2 = Random.Range(-135f, -123f); //different set of numbers to spawn other rocks at the same time but in a different location
            Instantiate(rockPrefab, new Vector3(setPositionX, transform.position.y, setPositionZ2), Quaternion.identity);
            yield return new WaitForSeconds(randomTime2);
        }
    }
}
