using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setTransparency : MonoBehaviour
{
    public GameObject currentGameObject;
    public float alpha = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        currentGameObject = gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        Color oldColor = currentGameObject.GetComponent<Renderer>().material.color;
        currentGameObject.GetComponent<Renderer>().material.SetColor("_Color", new Color(oldColor.r, oldColor.g, oldColor.b, alpha));
    }
}
