using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    Inventory()
    {
        resourceAmount.Add("matter", 0);
        resourceAmount.Add("smarts", 0);
        resourceAmount.Add("motion", 0);
        resourceAmount.Add("force", 0);
        reset();
    }

    public int[] inventoryAmount=new int[2];
    public SortedDictionary<string, int> resourceAmount=new SortedDictionary<string, int>();
    int resourceStartAmount;
    public bool debug;

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
       if (debug)
        {
            resourceStartAmount = 100;
        }
       else
        {
            resourceStartAmount = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
