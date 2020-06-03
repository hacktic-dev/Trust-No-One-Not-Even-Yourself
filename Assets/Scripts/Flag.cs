using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
    public GameHandler gameHandler;
    public AudioSource audioSource;
    public AudioSource audioSource2;
    public AudioClip loseSound;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void PlayLose()
    {
        audioSource2.PlayOneShot(loseSound, 1f * gameHandler.MasterVolume);
    }

    // Update is called once per frame
    void Update()
    {
        

        if (gameHandler.gameState=="active")
        {
            audioSource.volume = gameHandler.MasterVolume;
        }
        else
        {
            audioSource.volume = 0;
        }

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
