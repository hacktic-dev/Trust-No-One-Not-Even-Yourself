using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPad : MonoBehaviour
{

    float timerToSpawn;

    const float spawnTimeInterval = 0.5f;
    const int spawnedMaximum = 10;

    public string resourceType;
    public int currentSpawnedResources;

    public Player player;
    public GameObject Resource;

    // Start is called before the first frame update
    void Start()
    {
        timerToSpawn = spawnTimeInterval;
        currentSpawnedResources = 0;



    }

    // Update is called once per frame
    void Update()
    {
        timerToSpawn -= Time.deltaTime;
        if (timerToSpawn<=0 && currentSpawnedResources<spawnedMaximum)
        {
            currentSpawnedResources++;
            timerToSpawn = spawnTimeInterval;
            SpawnObject(resourceType);

        }

        if (Vector3.Distance(player.transform.position, transform.position)<3f)
        {
            player.inventory.resourceAmount[resourceType] += currentSpawnedResources;
            currentSpawnedResources = 0;
            DestroyAllChildren();
        }
    }





        //Spawn a resource of resourceType type
        void SpawnObject(string resourceType)
    {
        float randomX = Random.Range(-2.0f, 2.0f);
        float randomZ = Random.Range(-2.0f, 2.0f);
        Vector3 newPosition = new Vector3(transform.position.x + randomX, transform.position.y + 0.4f, transform.position.z + randomZ);

        GameObject instantiatedObject = Instantiate(Resource, newPosition, transform.rotation);
        instantiatedObject.transform.parent = gameObject.transform;
        Resource resourceObject= instantiatedObject.GetComponent<Resource>();
        resourceObject.resourceType=resourceType;
    }

    //Destory all spawned resources
    void DestroyAllChildren()
    {

        foreach (Transform child in gameObject.transform)
        {
            if (child.tag == "Resource")
            {
                GameObject.Destroy(child.gameObject);
            }
        }
    }
}
