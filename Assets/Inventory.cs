using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    Inventory()
    {
        resourceAmount.Add("matter", 10);
        resourceAmount.Add("smarts", 10);
        resourceAmount.Add("motion", 10);
        resourceAmount.Add("force", 10);
        reset();
    }

    public int[] inventoryAmount=new int[2];
    public SortedDictionary<string, int> resourceAmount=new SortedDictionary<string, int>();

    public void reset()
    {
        for (int i = 0; i < 2; i++)
        {
            inventoryAmount[i] = 0;
        }
        resourceAmount["matter"] = 10;
        resourceAmount["smarts"] = 10;
        resourceAmount["motion"] = 10;
        resourceAmount["force"] = 10;
    }

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
