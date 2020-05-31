using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    Inventory()
    {
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
        resourceAmount.Add("matter", 0);
        resourceAmount.Add("smarts", 0);
        resourceAmount.Add("motion", 0);
        resourceAmount.Add("force", 0);
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
