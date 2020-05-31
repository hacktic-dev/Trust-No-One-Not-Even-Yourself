using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
public class Enemy : MonoBehaviour
{

    Enemy() { gameObject.GetComponent<ParticleSystem>().Stop(); }

    //ublic float speed;
    // public float attackPower;
    public Health health;
     float lastFrameHealth;
    public Transform flag;
    float particleTimer;
    bool firstFrame = true;
    public GameHandler gameHandler;

    // Start is called before the first frame update
    void Start()
    {
        
        gameObject.GetComponent<AIPath>().destination = flag.transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        if (particleTimer<=0)
        { particleTimer = 0;
            gameObject.GetComponent<ParticleSystem>().Stop();

        }

        particleTimer -= Time.deltaTime;

        if (health.health != lastFrameHealth && !firstFrame)
        {
            gameObject.GetComponent<ParticleSystem>().Play();
            particleTimer = 0.2f;
        }
        lastFrameHealth = health.health;

        if (health.health <= 0 || gameHandler.roundType=="attack")
        {
            Destroy(gameObject);
        }

        if (firstFrame)
        {
            firstFrame = false;
        }
    }
}
