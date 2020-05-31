using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasUpdate : MonoBehaviour
{
    public Text barrierAmountText;
    public Text stillTurretAmountText;
    public Text movingTurretAmountText;

    public Text matterText;
    public Text smartsText;
    public Text motionText;
    public Text forceText;

    public Text waveTime;
    public Text waveNumber;
    public Text instructionText;

    private const float alpha = 0.8f;

    public int selectedIndex;

    public GameObject stillTurretInventory;
    public GameObject movingTurretInventory;
    public GameObject barrierInventory;

    public GameObject matterBox;
    public GameObject smartsBox;
    public GameObject forceBox;
    public GameObject motionBox;

    public GameHandler gameHandler;

    public Player player;
    public List<GameObject> inventoryBoxSprites;

    public GameObject activeUI;
    public GameObject menuUI;
    public GameObject pauseUI;
    public Button startGame;

    // Start is called before the first frame update
    void Start()
    {
        matterBox.GetComponent<Image>().color = new Color(0, 0, 0, alpha);
        smartsBox.GetComponent<Image>().color = new Color(0, 0, 0, alpha);
        forceBox.GetComponent<Image>().color = new Color(0, 0, 0, alpha);
        motionBox.GetComponent<Image>().color = new Color(0, 0, 0, alpha);

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
        if(gameHandler.gameState=="active")
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            activeUI.SetActive(true);
            menuUI.SetActive(false);
            pauseUI.SetActive(false);

        }
        else if (gameHandler.gameState=="menu")
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            activeUI.SetActive(false);
            menuUI.SetActive(true);
            pauseUI.SetActive(false);
        }
        else if (gameHandler.gameState=="paused")

        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            activeUI.SetActive(false);
            menuUI.SetActive(false);
            pauseUI.SetActive(true);
        }

        //startGame.onClick.AddListener(StartGameOnClick);

        updateTextValues();

        if (player.newSelection == true)
        {
            updateSelection();
        }
    }

    public void ReturnToMenu()
    {
        gameHandler.gameState = "menu";
    }

    public void StartGameOnClick()
    {
        Debug.Log("press");
        gameHandler.gameState = "active";
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

        smartsText.text = player.inventory.resourceAmount["smarts"].ToString();
        motionText.text = player.inventory.resourceAmount["motion"].ToString();
        forceText.text = player.inventory.resourceAmount["force"].ToString();
        matterText.text = player.inventory.resourceAmount["matter"].ToString();

        waveNumber.text = "Wave " + gameHandler.roundNumber.ToString();

        if (gameHandler.timeLeftThisRound < gameHandler.fightTimeLength)
        {
            waveTime.text = "Wave ends in " + Mathf.Ceil(gameHandler.timeLeftThisRound).ToString();
        }
        else
        {
            waveTime.text = "Wave begins in " + Mathf.Ceil(gameHandler.timeLeftThisRound - gameHandler.fightTimeLength).ToString();
        }

        if(gameHandler.gameState=="lose")
        {
            instructionText.text = "You Lose!";
        }

        else if (gameHandler.roundLength - gameHandler.timeLeftThisRound < 3)
        {
            if (gameHandler.roundType == "defend")
            {
                instructionText.text = "Defend The Flag!";
            }
            else
            { instructionText.text = "Destroy The Flag!"; }
        }
   
        else
        {
            instructionText.text = "";
        }
    }

}
