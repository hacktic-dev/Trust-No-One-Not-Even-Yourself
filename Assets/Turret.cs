using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public Transform head;
    public Transform player;
    public Transform hurtParticle;
    GameObject closest;
    const float timeBetweenShoot = 1f;
    float timeToNextShoot;
    float damage = 10f;
    //float gunOffset = 0.2f;
    Vector3 gun1Position;
    Vector3 gun2Position;
    public Health health;
    public AudioSource audioSource;
    public AudioClip shoot;
    float particleTimer, particleTimer2;
    float lastFrameHealth;
    public GameHandler gameHandler;
    bool firstFrame = true;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("FindClosestEnemy", 0.5f, 0.5f);
        timeToNextShoot = timeBetweenShoot;

        /*
        foreach (Transform child in head.transform)
        {
            //Debug.Log(gunOffset);
            if (child.tag == "Gun")
            {
                gun1Position = child.position;

            }
            else if (child.tag == "Gun2")
            {
                gun2Position = child.position;
            }
        }
        */
    }

        // Update is called once per frame
        void Update()
        {
        FindClosestEnemy();
        //Debug.Log(head.GetComponent<ParticleSystem>().isPlaying.ToString());
        //Debug.Log(particleTimer.ToString());
        if (health.health != lastFrameHealth && !firstFrame)
        {
           hurtParticle.GetComponent<ParticleSystem>().Play();
            particleTimer2 = 0.4f;
        }
        lastFrameHealth = health.health;

        if (firstFrame)
        {
            firstFrame = false;
        }

        if (particleTimer2 < 0)
        {
            hurtParticle.GetComponent<ParticleSystem>().Stop();
            particleTimer2 = 0;
        }

        particleTimer2 -= Time.deltaTime;

        if (gameObject.tag != "Shadow")
        {
            if (gameHandler.gameState == "active")
            {

                if (health.health <= 0)
                {
                    Destroy(gameObject, 0.4f);
                }

                if (particleTimer < 0)
                {
                    Debug.Log("end");
                    head.GetComponent<ParticleSystem>().Stop();
                    particleTimer = 0;
                }
            particleTimer -= Time.deltaTime;
            

                //please do not judge this code  

                //TODO fix if there is time
                /*
                foreach (Transform child in head.transform)
                {
                    Debug.Log(gunOffset);
                    if (child.tag == "Gun" && gunOffset > 0)
                    {
                        if (gunOffset - Time.deltaTime/5 > 0)
                        {
                            float time = Time.deltaTime/5;

                            child.position = head.position - child.up * gunOffset;
                            gunOffset -= time;

                        }
                        else
                        {
                            
                            child.position = gun1Position - child.up * gunOffset;
                            gunOffset = 0;

                    }
                    }
                    if (child.tag == "Gun2" && gunOffset > 0)
                    {
                        if (gunOffset - Time.deltaTime/5 > 0)
                        {
                            float time = Time.deltaTime/5;

                            child.position = gun2Position - child.up * gunOffset;
                            gunOffset -= time;
                            Debug.DrawRay(child.position, child.up * 10f, Color.white, 0.5f);
                    }
                        else
                        {
                           
                            child.position = gun2Position - child.up * gunOffset;
                            gunOffset = 0;
                    }
                    }

                }
                */
                timeToNextShoot -= Time.deltaTime;
                Look();
                if (timeToNextShoot <= 0f)
                {
                    Shoot();
                    
                }
            }
        }
        }

    void Look()
        {
            //DEBUG
            FindClosestEnemy();

            if (closest != null)
            {
                Vector3 test = head.position;
                Quaternion target = Quaternion.LookRotation(closest.transform.position - test);
                head.rotation = Quaternion.Lerp(head.rotation, target, Time.deltaTime * 10);
            }
        }

    void Shoot()
    {
        if (closest != null)
        {
            audioSource.PlayOneShot(shoot, 0.3f * gameHandler.MasterVolume);
            head.GetComponent<ParticleSystem>().Play();
            particleTimer = 0.2f;

            /*
            foreach (Transform child in head.transform)
            {
                gunOffset = 0.2f;
            }
            */

            timeToNextShoot = timeBetweenShoot;

            //Debug.DrawRay(head.position, head.forward * 10f, Color.white, 0.5f);
            RaycastHit[] raycast = Physics.RaycastAll(head.position, head.forward, 100f);
            if (raycast.Length > 0)
            {

                for (int i = 0; i < raycast.Length; i++)
                {
                    if (raycast[i].transform.tag == "Enemy" || (gameHandler.roundType == "attack" && raycast[i].transform.tag == "Player"))
                    {
                        raycast[i].transform.GetComponent<Health>().health -= damage;
                        if(raycast[i].transform.tag == "Player")
                        {
                            raycast[i].transform.GetComponent<Player>().PlayHurt();
                        }
                        break;
                    }
                }

            }

        }
    }

    void FindClosestEnemy()
    {
        if (gameObject.tag != "Shadow")
        {
            GameObject[] enemies;

            enemies = GameObject.FindGameObjectsWithTag("Enemy");
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

            List<GameObject> list = new List<GameObject>();
            list.AddRange(enemies);
            list.AddRange(players);
            enemies = list.ToArray();

            float closestDistance = 10000;
            Vector3 position = transform.position;
            closest = null;

            foreach (GameObject enemy in enemies)
            {
                RaycastHit[] raycast = Physics.RaycastAll(transform.position, -transform.position + enemy.transform.position, 100f);

                int enemyIndex = 0;

                for (int i = 0; i < raycast.Length; i++)
                {
                    if (raycast[i].transform.tag == "Enemy" || (gameHandler.roundType == "attack" && raycast[i].transform.tag == "Player"))
                        enemyIndex = i;
                    break;
                }

                bool nestedBreak = false;

                for (int i = 0; i < raycast.Length; i++)
                {
                    if ((raycast[i].transform.tag == "SolidObject" && raycast[i].distance <= raycast[enemyIndex].distance) || raycast[enemyIndex].transform.position.y<transform.position.y)
                    {
                        nestedBreak = true;
                    }

                }
                if (nestedBreak == true)
                {
                    continue;
                }

                if ((raycast.Length > 0 && raycast[enemyIndex].transform.tag == "Enemy" )||
                    ( raycast.Length > 0  && gameHandler.roundType == "attack" && raycast[enemyIndex].transform.tag == "Player"))
                {
                    float distance = Vector3.Distance(enemy.transform.position, transform.position);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closest = enemy;
                    }
                }

   

            }
        }
    }
    }
