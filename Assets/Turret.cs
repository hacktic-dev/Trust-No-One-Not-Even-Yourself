using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public Transform head;
    public Transform player;
    GameObject closest;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("FindClosestEnemy", 0.5f, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        Look();
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

            int enemyIndex=0;

            for (int i= 0; i < raycast.Length;i++)
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

            if (raycast.Length>0 && raycast[0].transform.tag == "Enemy")
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
