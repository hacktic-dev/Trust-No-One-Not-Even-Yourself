using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPad : MonoBehaviour
{

    public AudioSource audioSource;
    public AudioClip clip;

    float timerToSpawn;

    public float spawnTimeInterval = 1.5f; //DEBUG default 1.5
    const int spawnedMaximum = 5;

    public string resourceType;
    public int currentSpawnedResources;

    public Player player;
    public GameObject Resource;

    public GameHandler gameHandler;

    // Start is called before the first frame update
    void Start()
    {
        timerToSpawn = spawnTimeInterval;
        currentSpawnedResources = 0;



    }

    // Update is called once per frame
    void Update()
    {
        if (gameHandler.gameState == "active" && gameHandler.roundType=="defend")
        {

            if (gameHandler.timeLeftThisRound > gameHandler.fightTimeLength)
            {
                timerToSpawn -= Time.deltaTime;
                if (timerToSpawn <= 0 && currentSpawnedResources < spawnedMaximum)
                {
                    currentSpawnedResources++;
                    timerToSpawn = spawnTimeInterval;
                    SpawnObject(resourceType);

                }
            }

            if (Vector3.Distance(player.transform.position, transform.position) < 3f && currentSpawnedResources > 0)
            {
                audioSource.pitch = (Random.Range(0.9f, 1.1f));
                audioSource.PlayOneShot(clip, 0.5f* gameHandler.MasterVolume);
                player.inventory.resourceAmount[resourceType] += currentSpawnedResources;
                currentSpawnedResources = 0;
                DestroyAllChildren();

                timerToSpawn = spawnTimeInterval;
            }

          
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
