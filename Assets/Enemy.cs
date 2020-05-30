using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
public class Enemy : MonoBehaviour
{
    Enemy()
    { health = maxHealth; }

    public float speed;
    public float attackPower;
    public float health;
    public float maxHealth;
    public Transform flag;

    // Start is called before the first frame update
    void Start()
    {
        //gameObject.GetComponent<AIPath>().destination = flag.transform.position;
    }
    // Update is called once per frame
    void Update()
    {

        GoToFlag();
    }

void GoToFlag()
{
       
}
}

