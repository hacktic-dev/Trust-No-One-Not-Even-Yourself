using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Enemy : MonoBehaviour
{

    Enemy() {}

    public AudioSource audioSource;
    public AudioClip hurtSound1;
    public AudioClip hurtSound2;
    public AudioClip hurtSound3;
    public AudioClip hurtSound4;

    List<AudioClip> hurtSounds = new List<AudioClip>();

    //ublic float speed;
    // public float attackPower;
    public Health health;
     float lastFrameHealth;
    public Transform flag;
    float particleTimer;
    bool firstFrame = true;
    public GameHandler gameHandler;
    public NavMeshAgent enemy;

    // Start is called before the first frame update
    void Start()
    {
        
        hurtSounds.Add(hurtSound1);
        hurtSounds.Add(hurtSound2);
        hurtSounds.Add(hurtSound3);
        hurtSounds.Add(hurtSound4);
    }
    // Update is called once per frame
    void Update()
    {

        if(Vector3.Distance(transform.position,flag.transform.position)<6.25)
            {
            flag.GetComponent<Flag>().PlayLose();
            gameHandler.gameState = "lose";
        }

        if (gameHandler.gameState=="lose")
        { Destroy(gameObject); }

        if (gameHandler.gameState != "active")
        {
            enemy.speed = 0;
        }
        else
        {
            enemy.speed = 1.5f + gameHandler.roundNumber * 0.5f;
        }

            if (gameHandler.gameState == "active")
        {
            enemy.SetDestination(flag.position);
            // gameObject.GetComponent<AIPath>().enabled = true;
            if (particleTimer <= 0)
            {
                particleTimer = 0;
                gameObject.GetComponent<ParticleSystem>().Stop();

            }

            particleTimer -= Time.deltaTime;

            if (health.health != lastFrameHealth && !firstFrame)
            {
                int index = Random.Range(0, 3);

                audioSource.PlayOneShot(hurtSounds[index], 0.8f * gameHandler.MasterVolume);
                //SoundAfterTime(0.2f);
                gameObject.GetComponent<ParticleSystem>().Play();
                particleTimer = 0.4f;
            }
            lastFrameHealth = health.health;

            if (health.health <= 0 || gameHandler.roundType == "attack")
            {
                Destroy(gameObject,0.4f);
            }

            if (firstFrame)
            {
                firstFrame = false;
            }
        }
        else
        {
          //  gameObject.GetComponent<AIPath>().enabled = false;
        }
    }

    IEnumerator SoundAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        int index = Random.Range(0, 3);

        audioSource.PlayOneShot(hurtSounds[index], 0.8f * gameHandler.MasterVolume);
    }


}
