using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPad : MonoBehaviour
{

    float timerToSpawn;
    const float spawnTimeInterval = 3.0f;
    public string resourceType;

    public GameObject Resource;

    // Start is called before the first frame update
    void Start()
    {
        timerToSpawn = spawnTimeInterval;
    }

    // Update is called once per frame
    void Update()
    {
        timerToSpawn -= Time.deltaTime;
        if (timerToSpawn<=0)
        {
        timerToSpawn = spawnTimeInterval;
            SpawnObject(resourceType);

        }
    }

    void SpawnObject(string resourceType)
    {
        float randomX = Random.Range(-1.0f, 1.0f);
        float randomZ = Random.Range(-1.0f, 1.0f);
        Vector3 newPosition = new Vector3(transform.position.x + randomX, transform.position.y + 1.0f, transform.position.z + randomZ);

        GameObject instantiatedObject = Instantiate(Resource, newPosition, transform.rotation);
        Resource resourceObject= instantiatedObject.GetComponent<Resource>();
        resourceObject.resourceType = resourceType;
    }
}
