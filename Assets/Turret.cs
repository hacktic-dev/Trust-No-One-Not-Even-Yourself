using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public Transform head;
    public Transform player;
    GameObject closest;
    const float timeBetweenShoot = 1f;
    float timeToNextShoot;
    float damage = 10f;
    float gunOffset = 0.2f;
    Vector3 gun1Position;
    Vector3 gun2Position;

    public AudioSource audioSource;
    public AudioClip shoot;
    float particleTimer;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("FindClosestEnemy", 0.5f, 0.5f);
        timeToNextShoot = timeBetweenShoot;

        foreach (Transform child in head.transform)
        {
            Debug.Log(gunOffset);
            if (child.tag == "Gun")
            {
                gun1Position = child.position;

            }
            else if (child.tag == "Gun2")
            {
                gun2Position = child.position;
            }
        }
    }

        // Update is called once per frame
        void Update()
        {
        if (particleTimer < 0)
        {
            head.GetComponent<ParticleSystem>().Stop();
            particleTimer = 0;
        }
        particleTimer -= Time.deltaTime;
        if (gameObject.tag != "Shadow")
            {

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

                    if (closest != null)
                    {
                        audioSource.PlayOneShot(shoot, 0.8f);
                        head.GetComponent<ParticleSystem>().Play();
                        particleTimer = 0.2f;


                    foreach (Transform child in head.transform)
                        {
                            gunOffset = 0.2f;
                        }

                        timeToNextShoot = timeBetweenShoot;

                        Debug.DrawRay(head.position, head.forward * 10f, Color.white, 0.5f);
                        RaycastHit[] raycast = Physics.RaycastAll(head.position, head.forward, 100f);
                        if (raycast.Length > 0)
                        {

                            for (int i = 0; i < raycast.Length; i++)
                            {
                                if (raycast[i].transform.tag == "Enemy")
                                {
                                    raycast[i].transform.GetComponent<Health>().health -= damage;
                                    break;
                                }
                            }

                        }

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
                head.rotation = Quaternion.Lerp(head.rotation, target, Time.deltaTime * 5);
            }
        }

        void FindClosestEnemy()
        {
            GameObject[] enemies;
            enemies = GameObject.FindGameObjectsWithTag("Enemy");
            float closestDistance = 10000;
            Vector3 position = transform.position;
            closest = null;
            foreach (GameObject enemy in enemies)
            {
                RaycastHit[] raycast = Physics.RaycastAll(transform.position, -transform.position + enemy.transform.position, 100f);

                int enemyIndex = 0;

                for (int i = 0; i < raycast.Length; i++)
                {
                    if (raycast[i].transform.tag == "Enemy") ;
                    enemyIndex = i;
                    break;
                }

                for (int i = 0; i < raycast.Length; i++)
                {
                    if (raycast[i].transform.tag == "SolidObject" && raycast[i].distance <= raycast[enemyIndex].distance)
                    {
                        goto nested_break;
                    }

                }

                if (raycast.Length > 0 && raycast[0].transform.tag == "Enemy")
                {
                    float distance = Vector3.Distance(enemy.transform.position, transform.position);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closest = enemy;
                    }
                }

            nested_break: continue;

            }
        }
    }
