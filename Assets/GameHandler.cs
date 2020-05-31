﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public float roundLength=60f; //test value, default 180
    public float fightTimeLength=55f;
    public string roundType="defend";
    public float timeLeftThisRound;
    public int roundNumber;
    public string gameState;
    public float MasterVolume;
    public bool newRound;
    public Player player;

    // Start is called before the first frame update
    void Start()
    {
        gameState = "menu";
        roundNumber = 1;
        timeLeftThisRound = roundLength;
        
    }

    // Update is called once per frame
    void Update()
    {
        newRound = false;

        if (gameState=="paused")
        {

        }

        if (gameState == "active")
        {
            if (Input.GetKeyDown("escape"))
            {
                gameState = "paused";
            }

            timeLeftThisRound -= Time.deltaTime;

            if (timeLeftThisRound <= 0f)
            {
                if (roundType == "defend")
                {
                    newRoundF();
                }
                else
                {
                    gameState = "lose";
                }
            }
        }
    }

    public void SetVolume(System.Single value)
    {
        MasterVolume = value;
    }

    public void newRoundF()
    {
        newRound = true;
        timeLeftThisRound = roundLength;
        roundNumber++;
        if (roundNumber % 2 == 1)
        {
            roundType = "defend";
        }
        else
        {
            roundType = "attack";
        }
    }

    public void newGame()
    {
        object[] obj = GameObject.FindObjectsOfType(typeof(GameObject));
        List<GameObject> spawnLocs = new List<GameObject>();
        foreach (object o in obj)
        {
            Debug.Log("test");
            GameObject g = (GameObject)o;
            if (g.tag == "Enemy" || g.tag=="Turret"||g.tag=="Resource"||g.tag=="Barrier")
            {
                Debug.Log("destroy" + g.tag.ToString());
                Destroy(g);
            }
            if(g.tag=="SpawnPad")
            {
                g.GetComponent<SpawnPad>().currentSpawnedResources = 0;
            }
            

        }
        roundNumber = 1;
        timeLeftThisRound = roundLength;
        roundType = "defend";
        player.reset();
    }
}
