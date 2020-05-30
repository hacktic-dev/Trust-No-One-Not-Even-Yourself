using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    float timeToNextSpawn;
    float timeBetweenSpawns;
    int currentRound;

    public GameHandler gameHandler;
    public GameObject enemy;

    // Start is called before the first frame update
    void Start()
    {
        currentRound = 0;
        timeBetweenSpawns = 10f;
        timeToNextSpawn = timeBetweenSpawns;
    }

    // Update is called once per frame
    void Update()
    {
        timeToNextSpawn -= Time.deltaTime;

        if(timeToNextSpawn<=0f)
        {
            timeToNextSpawn = timeBetweenSpawns;
            Spawn();
        }

        if (currentRound!=gameHandler.roundNumber)
        {
            currentRound = gameHandler.roundNumber;
            //TODO change value
            timeBetweenSpawns -= 1f;
        }

    }

    void Spawn()
    {
        Vector3 newPosition = new Vector3(transform.position.x, transform.position.y -0.1f, transform.position.z);

        GameObject instantiatedObject = Instantiate(enemy, newPosition, transform.rotation);

        Enemy EnemyObject = instantiatedObject.GetComponent<Enemy>();

        //TODO test values, update later
        EnemyObject.speed = 1f+currentRound;
        EnemyObject.attackPower = 1f + currentRound;
    }
}
