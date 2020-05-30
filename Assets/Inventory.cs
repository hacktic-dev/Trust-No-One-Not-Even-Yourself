using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    Inventory()
    {
        for (int i=0; i<3;i++)
        {
            inventoryAmount[i] = 10;
        }
        resourceAmount.Add("matter", 0);
        resourceAmount.Add("smarts", 0);
        resourceAmount.Add("motion", 0);
        resourceAmount.Add("force", 0);
    }

    public int[] inventoryAmount=new int[3];
    public SortedDictionary<string, int> resourceAmount=new SortedDictionary<string, int>();


    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
