using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public float roundLength=60f; //test value, default 180
    public float fightTimeLength=55f;
    public string roundType;
    public float timeLeftThisRound;
    public int roundNumber;
    public string gameState;
    public float MasterVolume;

    // Start is called before the first frame update
    void Start()
    {
        gameState = "menu";
        roundNumber = 1;
        timeLeftThisRound = roundLength;
        roundType = "defend";
    }

    // Update is called once per frame
    void Update()
    {
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
        }
    }

    public void SetVolume(System.Single value)
    {
        MasterVolume = value;
    }
}
