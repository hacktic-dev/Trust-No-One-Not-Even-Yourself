using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
    public GameHandler gameHandler;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Collider[] checkForEnemy = Physics.OverlapSphere(transform.position, 3);
        foreach (Collider collision in checkForEnemy)
        {
            if (collision.transform.tag=="Enemy")
            {
                gameHandler.gameState = "lose";
                break;
            }
        }
    }
}
