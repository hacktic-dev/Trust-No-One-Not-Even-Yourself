using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI
public class Enemy : MonoBehaviour
{
    Enemy()
    { health = maxHealth; }

    public float speed;
    public float attackPower;
    public float health;
    public float maxHealth;
    public NavMeshAgent enemy;
    public Transform flag;

    // Start is called before the first frame update
    void Start()
    { }
    // Update is called once per frame
    void Update()
    {
        
        GoToFlag();
    }

void GoToFlag()
{
    enemy.SetDestination(flag.position);
}
}

