using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class Turret : MonoBehaviour
{
    public Transform head;
    public Transform player;
    public Transform hurtParticle;
    GameObject closest;
    const float timeBetweenShoot = 1f;
    float timeToNextShoot;
    float damage = 10f;
    const float maxDistance = 50f;
    //float gunOffset = 0.2f;
    // Vector3 gun1Position;
    //Vector3 gun2Position;

    public Health health;

    public AudioSource audioSource;
    public AudioClip TakeDamage;
    public AudioClip shoot;
    public AudioClip deathSound;

    float particleTimer, particleTimer2;
    float lastFrameHealth;
    public GameHandler gameHandler;
    bool firstFrame = true;

    // Start is called before the first frame update
    void Start()
    {
        //InvokeRepeating("FindClosestEnemy", 0.5f, 0.5f);
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

    if (transform.tag != "Shadow")
    {


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
                    audioSource.pitch = (Random.Range(0.9f, 1.1f));
                    //AudioSource.PlayClipAtPoint(deathSound,transform.position ,0.8f * gameHandler.MasterVolume);
                    Destroy(gameObject, 0.4f);
                }

                if (particleTimer < 0)
                {
                    // Debug.Log("end");
                    head.GetComponent<ParticleSystem>().Stop();
                    particleTimer = 0;
                }
                particleTimer -= Time.deltaTime;

                //Unused code
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
                }

                timeToNextShoot -= Time.deltaTime;
                Look();
                if (timeToNextShoot <= 0f)
                {
                    Shoot();

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
                head.rotation = Quaternion.Lerp(head.rotation, target, Time.deltaTime * 10); //ACCURACY
            }
        }

    void Shoot()
    {
        if (closest != null)
        {
            audioSource.pitch = (Random.Range(0.9f, 1.1f));
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
            Profiler.BeginSample("Shoot raycast Sample");
            RaycastHit[] raycast = Physics.RaycastAll(head.position, head.forward, 100f);
            Profiler.EndSample();
            if (raycast.Length > 0)
            {

                for (int i = 0; i < raycast.Length; i++)
                {
                    if (raycast[i].transform.tag == "Enemy" || (gameHandler.roundType == "attack" && raycast[i].transform.tag == "Player"))
                    {
                        if (raycast[i].transform.tag == "Player")
                        {
                            raycast[i].transform.GetComponent<Player>().StageHurt(damage,Vector3.Distance(raycast[i].transform.position,transform.position));
                        }
                        else
                        {
                            //todo put this inside turret and make taking damage a part of the health component
                            raycast[i].transform.GetComponent<Health>().health -= damage; }
                        
                        
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
        List<GameObject> list = new List<GameObject>();
            for (int i= 0; i < enemies.Length;i++)
            {
                if(Vector3.Distance(enemies[i].transform.position,transform.position)<=maxDistance)
                {list.Add(enemies[i]); }
            }
            enemies = list.ToArray();

            if (gameHandler.roundType == "attack")
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            List<GameObject> list2 = new List<GameObject>();
            list2.AddRange(enemies);
            list2.AddRange(players);
            enemies = list2.ToArray();
        }

        float closestDistance = 10000;
        Vector3 position = transform.position;
        closest = null;

        foreach (GameObject enemy in enemies)
        {
            RaycastHit[] raycast = Physics.RaycastAll(transform.position, -transform.position + enemy.transform.position, maxDistance);
            
            int enemyIndex = 0;
            
            //get index of the player/enemy in the raycast array
            for (int i = 0; i < raycast.Length; i++)
            {
                if (raycast[i].transform.tag == "Enemy" ||  raycast[i].transform.tag == "Player")
                {
                    enemyIndex = i;
                    //Debug.Log(enemyIndex);
                    break;
                }
            }

            //check to make sure there isn't any walls in the way
            bool nestedBreak = false;
            for (int i = 0; i < raycast.Length; i++)
            {
                if (((raycast[i].transform.tag == "SolidObject" || raycast[i].transform.tag == "Barrier" || raycast[i].transform.tag == "RaycastCollider")
                        && raycast[i].distance <= raycast[enemyIndex].distance))
                {
                    nestedBreak = true;
                }

            }
            if (nestedBreak == true)
            {
                continue;
            }

            //check to see if its closer than all enemies checked before
            if ((raycast[enemyIndex].transform.tag == "Enemy" || raycast[enemyIndex].transform.tag == "Player"))
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

    public void StageHurt(float damage, float distance)
    {
        StartCoroutine(Hurt(damage, distance * 0.01f));
    }

    IEnumerator Hurt(float damage, float time)
    {
        yield return new WaitForSeconds(time);
        audioSource.pitch = (Random.Range(0.9f, 1.1f));
        audioSource.PlayOneShot(TakeDamage, 0.8f * gameHandler.MasterVolume);
        health.health -= damage;
        Debug.Log(damage);
    }

}
