﻿using System.Collections;
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
    }

    public int[] inventoryAmount=new int[3];


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
