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

    IEnumerator SpawnFalling()
    {
        while (true)
        {
            float randomTime = Random.Range(1f, 2f);
            float setPosition = Random.Range(-50f, -30f);
            Instantiate(rockPrefab, new Vector3(setPosition, transform.position.y, transform.position.z), Quaternion.identity);
            yield return new WaitForSeconds(randomTime);
            Instantiate(rockPrefab, new Vector3(setPosition, transform.position.y, transform.position.z), Quaternion.identity);
            float randomTime2 = Random.Range(1f, 3f);
            float setPosition2 = Random.Range(-30f, -80f);
            Instantiate(rockPrefab, new Vector3(setPosition2, transform.position.y, transform.position.z-2), Quaternion.identity);
            yield return new WaitForSeconds(randomTime2);
            Instantiate(rockPrefab, new Vector3(setPosition2, transform.position.y, transform.position.z+2), Quaternion.identity);
        }
    }
    void Update()
    {
        
    }
}
