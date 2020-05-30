using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setTransparency : MonoBehaviour
{
    public GameObject currentGameObject;
    float alpha = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        currentGameObject = gameObject;
        Color oldColor = currentGameObject.GetComponent<Renderer>().material.color;
        currentGameObject.GetComponent<Renderer>().material.SetColor("_Color", new Color(oldColor.r, oldColor.g, oldColor.b, alpha));
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
