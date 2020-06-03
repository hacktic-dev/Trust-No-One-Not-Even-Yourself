using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{


    public int[] inventoryAmount=new int[2];
    public SortedDictionary<string, int> resourceAmount=new SortedDictionary<string, int>();
    public int resourceStartAmount;
    public GameHandler gameHandler;

    public void reset()
    {
        for (int i = 0; i < 2; i++)
        {
            inventoryAmount[i] = 0;
        }
        resourceAmount["matter"] = resourceStartAmount;
        resourceAmount["smarts"] = resourceStartAmount;
        resourceAmount["motion"] = resourceStartAmount;
        resourceAmount["force"] = resourceStartAmount;
    }

    // Start is called before the first frame update
    void Start()
    {
        resourceAmount["matter"] = resourceStartAmount;
        resourceAmount["smarts"] = resourceStartAmount;
        resourceAmount["motion"] = resourceStartAmount;
        resourceAmount["force"] = resourceStartAmount;
    }

    // Update is called once per frame
    void Update()
    {
    }


}
