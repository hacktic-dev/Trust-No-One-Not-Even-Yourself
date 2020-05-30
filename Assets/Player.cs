using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public class Player : MonoBehaviour
{

    public AudioSource audioSource;
    public AudioClip craftSuccess;
    public AudioClip craftFail;

    public Inventory inventory;

    //execute objectPlace code when new
    public bool objectPlaceMode;
    //index for selected item
    public int selectedIndex;

    public bool newSelection;

    //the number of items in the game, update this as we add more
    const int totalItemNumber = 3;

    public GameObject barrier;
    public GameObject turret;

    public Transform selfTransform;
    Vector3 move;
    public float speed = 12f;
    public CharacterController controller;
    float xRotation;
    public int test;
    public float mouseSense = 100f;
    public Transform cameraMount;
    public float maxPlaceDistance = 20f;
    Vector3 hitPosition;

    public Transform shadowPointer;
    public GameObject shadowPointerBarrier;
    public GameObject shadowPointerTurret;
    GameObject currentshadowPointer;


    public bool placeable;
    GameObject objectToPlace;
    int currentObjectAmount;

    // Start is called before the first frame update
    void Start()
    {
        LockMouse();
    }


    // Update is called once per frame
    void Update()
    {
        //goes true for a frame when initiating a new selection
        newSelection = false;

        //select inventory
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            selectUpdate(1);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") <0f)
        {
            selectUpdate(-1);
        }
        if(Input.GetKeyDown("space") && objectPlaceMode)
        {
            shadowPointer.position= new Vector3(0, -100, 0);
            objectPlaceMode = false;
        }
        else if (Input.GetKeyDown("space"))
        {
            objectPlaceMode = true;
        }

        Movement();
        Camera();
        if (objectPlaceMode)
        {
            ShadowPointer();
        }
    }

    void Movement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        move = transform.right * x + transform.forward * z;
        move = Vector3.ClampMagnitude(move, 1);
        controller.Move(move * speed * Time.deltaTime);
    }

    void Camera()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSense * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSense * Time.deltaTime;


        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.Rotate(Vector3.up * mouseX);
        cameraMount.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    void LockMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    //execute this when the player selects a new object
    void selectUpdate(int selectDirection)
    {
        newSelection = true;
        if (selectDirection == 1)
        {
            if (selectedIndex == 0)
            {
                selectedIndex = totalItemNumber - 1;
            }
            else
            {
                selectedIndex--;
            }
        }
        else
        {
            if (selectedIndex == totalItemNumber - 1)
            {
                selectedIndex = 0;
            }
            else
            {
                selectedIndex++;
            }
        }
    }

    void ShadowPointer()
    {
       
        switch (selectedIndex)
        {
            case 0:
                shadowPointerBarrier.SetActive(true);
                shadowPointerTurret.SetActive(false);
                objectToPlace = barrier;
                currentshadowPointer = shadowPointerBarrier;
                break;
            case 1:
                shadowPointerBarrier.SetActive(false);
                shadowPointerTurret.SetActive(true);
                currentshadowPointer = shadowPointerTurret;
                objectToPlace = turret;

                break;
        }

        LayerMask mask = ~LayerMask.GetMask("Hidden Objects");
        if (Physics.Raycast(cameraMount.position, cameraMount.forward, maxPlaceDistance, mask) && inventory.inventoryAmount[selectedIndex]>0)
        {
            RaycastHit hit;
            if (Physics.Raycast(cameraMount.position, cameraMount.forward, out hit, maxPlaceDistance + 1, mask))
            {
                hitPosition = hit.point;
            }
        }
		else
		{
			hitPosition = new Vector3(1000, 0, 0);
        }
        shadowPointer.position = hitPosition;
        shadowPointer.SetPositionAndRotation(new Vector3(shadowPointer.position.x, 0, shadowPointer.position.z),selfTransform.rotation);
        //shadowPointer.rotation = selfTransform.rotation;

        Collider[] checkCollision = Physics.OverlapSphere(shadowPointer.position, 8f, LayerMask.GetMask("Solid Object"));
        ;
        if (checkCollision.Length>0 && checkCollision[0].tag=="SolidObject")
        {
            Debug.Log(checkCollision[0].tag);
            placeable = false;
        }
        else
        {
            placeable = true;
        }

        switch (selectedIndex)
        {
            case 0:
                shadowPointer.Rotate(0.0f, 90.0f, 0.0f);
                break;
            case 1:
                shadowPointer.Rotate(0.0f, 180.0f, 0.0f);
                break;
        }

        if (Input.GetMouseButtonDown(0))
        {
            bool success = false;
            if (inventory.inventoryAmount[selectedIndex] > 0 && placeable)
            {
                success = true;
                GameObject instantiatedObject = Instantiate(objectToPlace, shadowPointer.position, shadowPointer.rotation);
                /*
                if (objectToPlace==turret)
                {
                    Turret playerVariable = instantiatedObject.GetComponent<Turret>();
                    Transform test=this.transform;
                    playerVariable.player = test;
                }
                */
                Debug.Log(inventory.inventoryAmount[selectedIndex]);
                inventory.inventoryAmount[selectedIndex]--;
                Debug.Log(inventory.inventoryAmount[selectedIndex]);
            }

            if (success)
            {
                audioSource.PlayOneShot(craftSuccess, 0.8f);
            }
            else
            {
                audioSource.PlayOneShot(craftFail, 0.8f);
            }
        }

        

    }
	
    public void craftItem(string itemName)
    {
        bool success = false;
        if (itemName=="barrier")
        {
            if(inventory.resourceAmount["matter"]>=2 && inventory.resourceAmount["force"] >= 1)
            {
                inventory.inventoryAmount[0]++;
                inventory.resourceAmount["matter"] -= 2;
                inventory.resourceAmount["force"] -= 1;
                success = true;
            }
            
        }
        else if (itemName=="still turret")
        {
            if (inventory.resourceAmount["smarts"] >= 2 && inventory.resourceAmount["force"] >= 1)
            {
                inventory.inventoryAmount[1]++;
                inventory.resourceAmount["smarts"] -= 2;
                inventory.resourceAmount["force"] -= 1;
                success = true;
            }
        }

        else
        {
            throw new System.Exception("Invalid Item Type!");
        }

        if (success)
        {
            audioSource.PlayOneShot(craftSuccess, 0.8f);
        }
        else
        {
            audioSource.PlayOneShot(craftFail, 0.8f);
        }
    }
}
