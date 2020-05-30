using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasUpdate : MonoBehaviour
{
    public Text barrierAmountText;
    public Text stillTurretAmountText;
    public Text movingTurretAmountText;

    public Text smartsText;

    private const float alpha = 0.8f;

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
            inventoryBoxSprites[i].GetComponent<Image>().color = new Color(0,0,0, alpha);
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
        inventoryBoxSprites[selectedIndex].GetComponent<Image>().color = new Color(0, 0, 0, alpha);
        selectedIndex = player.selectedIndex;
        inventoryBoxSprites[selectedIndex].GetComponent<Image>().color = new Color(0.8f, 0.8f, 0.8f, alpha);
    }

    void updateTextValues()
    {
        barrierAmountText.text = player.inventory.inventoryAmount[0].ToString();
        stillTurretAmountText.text = player.inventory.inventoryAmount[1].ToString();
        movingTurretAmountText.text = player.inventory.inventoryAmount[2].ToString();

        smartsText.text= player.inventory.resourceAmount["smarts"].ToString();
    }

}
