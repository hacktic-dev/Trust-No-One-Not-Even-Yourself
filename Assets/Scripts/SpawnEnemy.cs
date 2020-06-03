using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class SpawnEnemy : MonoBehaviour
{
    float timeToNextSpawn;
    float timeBetweenSpawns;
    int currentRound;

    public GameHandler gameHandler;
    public GameObject enemy;
    public GameObject flag;

    // Start is called before the first frame update
    void Start()
    {
        currentRound = 0;
        timeBetweenSpawns = 7f;
        timeToNextSpawn = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (gameHandler.gameState == "active")
        {
            if (gameHandler.timeLeftThisRound < gameHandler.fightTimeLength && gameHandler.roundType == "defend")
            {
                timeToNextSpawn -= Time.deltaTime;
            }

            if (timeToNextSpawn <= 0f && gameHandler.timeLeftThisRound < gameHandler.fightTimeLength && gameHandler.roundType == "defend")
            {
                timeToNextSpawn = timeBetweenSpawns;
                if (gameHandler.timeLeftThisRound < gameHandler.fightTimeLength && gameHandler.roundType == "defend")
                {
                    Spawn();
                }
            }

            if (currentRound != gameHandler.roundNumber)
            {
                currentRound = gameHandler.roundNumber;
                
                timeBetweenSpawns -= timeBetweenSpawns/14;
                timeToNextSpawn = 0f;
            }
        }

    }

    void Spawn()
    {
        Vector3 newPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        GameObject instantiatedObject = Instantiate(enemy, newPosition, transform.rotation);

        Enemy EnemyObject = instantiatedObject.GetComponent<Enemy>();

        //TODO test values, update later
        EnemyObject.GetComponent<NavMeshAgent>().speed = 1.5f + currentRound*0.5f; //default 1.5
        EnemyObject.GetComponent<Health>().maxHealth = 30 + currentRound * 10;
        EnemyObject.gameHandler = gameHandler;
        EnemyObject.flag = flag.transform;

    }
}
