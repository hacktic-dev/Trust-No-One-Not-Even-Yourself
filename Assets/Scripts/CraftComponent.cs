using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftComponent : MonoBehaviour
{


    public string componentType;
    public Player player;

    Ray ray;
    RaycastHit hit;

    void Start()
    {
        
    }

    void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit) && hit.collider==gameObject.GetComponent<Collider>() && !player.objectPlaceMode)
        {
            gameObject.GetComponent<Renderer>().material.SetColor("_Color", new Color(4f, 4f,4f));
            if (Input.GetMouseButtonDown(0))
            {
                player.craftItem(componentType);
            }

        }
        else {
            gameObject.GetComponent<Renderer>().material.SetColor("_Color", new Color(2f, 2f, 2f));
        }

    }
}
