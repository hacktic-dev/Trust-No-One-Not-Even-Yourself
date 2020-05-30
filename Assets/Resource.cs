using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public string resourceType;

    // Start is called before the first frame update
    void Start()
    {
        if (resourceType=="matter")
        {
            GetComponent<Renderer>().material.SetColor("_Color", new Color(1.0f,1.0f,1.0f));
        }
        else if (resourceType == "force")
        {
            GetComponent<Renderer>().material.SetColor("_Color", new Color(1.0f, 0.3f, 0.3f));
        }
        else if (resourceType == "smarts")
        {
            GetComponent<Renderer>().material.SetColor("_Color", new Color(0.3f, 0.3f, 1.0f));
        }
        else if (resourceType == "motion")
        {
            GetComponent<Renderer>().material.SetColor("_Color", new Color(0.3f, 0.3f, 0.3f));
        }
        else
        {
            throw new System.Exception("Invalid Resource Type!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
