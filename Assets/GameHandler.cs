using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public float roundLength=10f; //test value, default 180
    public float fightTimeLength=2f;
    public string roundType;
    public float timeLeftThisRound;
    public int roundNumber;
    public string gameState;


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

        if (gameState == "active")
        {
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
}
