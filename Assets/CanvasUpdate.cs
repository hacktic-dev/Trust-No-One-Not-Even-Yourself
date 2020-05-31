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

    public Text health;

    public Text waveTime;
    public Text waveNumber;
    public Text instructionText;

    private const float alpha = 0.8f;

    public int selectedIndex;

    public GameObject stillTurretInventory;
    public GameObject movingTurretInventory;
    public GameObject barrierInventory;

    public GameHandler gameHandler;

    public Player player;
    public List<GameObject> inventoryBoxSprites;

    public Button startGame;

    // Start is called before the first frame update
    void Start()
    {

        selectedIndex = 0;

        inventoryBoxSprites.Add(barrierInventory);
        inventoryBoxSprites.Add(stillTurretInventory);
        inventoryBoxSprites.Add(movingTurretInventory);
        
        updateSelection();
    }

    // Update is called once per frame
    void Update()
    {
        if(gameHandler.gameState=="active")
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            if (gameHandler.roundType == "defend")
            {
                GameObject[] allChildren = new GameObject[transform.childCount];
                int i = 0;
                foreach (Transform child in transform)
                {
                    allChildren[i] = child.gameObject;
                    i += 1;
                }

                foreach (GameObject child in allChildren)
                {

                    if (child.tag == "ActiveDefend" || child.tag=="Active" || child.tag == "Always") 
                    {
                        child.SetActive(true);
                    }
                    else
                    { child.SetActive(false); }

                }
            }
            else
            {
                GameObject[] allChildren = new GameObject[transform.childCount];
                int i = 0;
                foreach (Transform child in transform)
                {
                    allChildren[i] = child.gameObject;
                    i += 1;
                }

                foreach (GameObject child in allChildren)
                {

                    if (child.tag == "ActiveAttack" || child.tag == "Active" || child.tag == "Always")
                    {
                        child.SetActive(true);
                    }
                    else
                    { child.SetActive(false); }

                }
            }

        }
        else if (gameHandler.gameState=="menu")
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            GameObject[] allChildren = new GameObject[transform.childCount];
            int i = 0;
            foreach (Transform child in transform)
            {
                allChildren[i] = child.gameObject;
                i += 1;
            }

            foreach (GameObject child in allChildren)
            {
                Debug.Log(child.tag);
                if (child.tag == "Menu")
                {
                    child.SetActive(true);
                }
                else
                { child.SetActive(false); }

            }

        }
        else if (gameHandler.gameState=="paused")

        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            GameObject[] allChildren = new GameObject[transform.childCount];
            int i = 0;
            foreach (Transform child in transform)
            {
                allChildren[i] = child.gameObject;
                i += 1;
            }

            foreach (GameObject child in allChildren)
            {
                if (child.tag == "Paused")
                {
                    child.SetActive(true);
                }
                else
                { child.SetActive(false); }

            }

        }

        else if (gameHandler.gameState == "options")

        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            GameObject[] allChildren = new GameObject[transform.childCount];
            int i = 0;
            foreach (Transform child in transform)
            {
                allChildren[i] = child.gameObject;
                i += 1;
            }

            foreach (GameObject child in allChildren)
            {
                if (child.tag == "Options")
                {
                    child.SetActive(true);
                }
                else
                { child.SetActive(false); }

            }

        }

        else if (gameHandler.gameState == "lose")

        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            GameObject[] allChildren = new GameObject[transform.childCount];
            int i = 0;
            foreach (Transform child in transform)
            {
                allChildren[i] = child.gameObject;
                i += 1;
            }

            foreach (GameObject child in allChildren)
            {
                if (child.tag == "Lose" || child.tag == "Always")
                {
                    child.SetActive(true);
                }
                else
                { child.SetActive(false); }

            }

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

    public void OpenOptions()
    {
        gameHandler.gameState = "options";
    }

    public void ResumeGame()
    {
        gameHandler.gameState = "active";
    }

    public void StartGameOnClick()
    {
        
        gameHandler.gameState = "active";
        gameHandler.newGame();
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
        //movingTurretAmountText.text = player.inventory.inventoryAmount[2].ToString();

        //Debug.Log("health is " + player.health.health.ToString());
        health.text = "Health: "+player.health.health.ToString();

        smartsText.text = player.inventory.resourceAmount["smarts"].ToString();
       // motionText.text = player.inventory.resourceAmount["motion"].ToString();
        forceText.text = player.inventory.resourceAmount["force"].ToString();
        matterText.text = player.inventory.resourceAmount["matter"].ToString();

        waveNumber.text = "Wave " + gameHandler.roundNumber.ToString();

        if (gameHandler.timeLeftThisRound < gameHandler.fightTimeLength && gameHandler.roundType=="defend")
        {
            waveTime.text = "Wave ends in " + Mathf.Ceil(gameHandler.timeLeftThisRound).ToString();
        }
        else if (gameHandler.roundType == "defend")
        {
            waveTime.text = "Wave begins in " + Mathf.Ceil(gameHandler.timeLeftThisRound - gameHandler.fightTimeLength).ToString();
        }
        else
        { waveTime.text = "Time Left: " + Mathf.Ceil(gameHandler.timeLeftThisRound).ToString(); }

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
