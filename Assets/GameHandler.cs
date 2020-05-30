using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public const float roundLength=10f; //test value, default 180
    public const float setupTimeLength=2f;
    public string roundType;
    public float timeLeftThisRound;
    public int roundNumber;
 


    // Start is called before the first frame update
    void Start()
    {
        roundNumber = 0;
        timeLeftThisRound = roundLength;
        roundType = "defend";
    }

    // Update is called once per frame
    void Update()
    {
        timeLeftThisRound -= Time.deltaTime;

        if (timeLeftThisRound<=0f)
        {
            timeLeftThisRound = roundLength;
            roundNumber++;
            if(roundNumber%2==0)
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
