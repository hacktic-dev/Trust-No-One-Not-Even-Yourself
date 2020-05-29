using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasUpdate : MonoBehaviour
{
    public Text barrierAmountText;
    public Text stillTurretAmountText;
    public Text movingTurretAmountText;

    public int selectedIndex;

    public GameObject stillTurretInventory;
    public GameObject movingTurretInventory;
    public GameObject barrierInventory;

    public Player player;
    public List<GameObject> inventoryBoxSprites;

    // Start is called before the first frame update
    void Start()
    {
        selectedIndex = 0;

        inventoryBoxSprites.Add(barrierInventory);
        inventoryBoxSprites.Add(stillTurretInventory);
        inventoryBoxSprites.Add(movingTurretInventory);

        for (int i = 0; i < inventoryBoxSprites.Count; i++)
        {
            inventoryBoxSprites[i].GetComponent<Image>().color = new Color(0,0,0,0.5f);
        }

        updateSelection();
    }

    // Update is called once per frame
    void Update()
    {
        updateTextValues();

        if (player.newSelection == true)
        {
            updateSelection();
        }
    }

    void updateSelection()
    {
        inventoryBoxSprites[selectedIndex].GetComponent<Image>().color = new Color(0, 0, 0, 0.5f);
        selectedIndex = player.selectedIndex;
        inventoryBoxSprites[selectedIndex].GetComponent<Image>().color = new Color(100, 100, 100, 0.5f);
    }

    void updateTextValues()
    {
        barrierAmountText.text = player.inventory.barrierAmount.ToString();
        stillTurretAmountText.text = player.inventory.stillTurretAmount.ToString();
        movingTurretAmountText.text = player.inventory.movingTurretAmount.ToString();
    }

}
