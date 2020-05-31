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

    public GameObject level;

    public GameHandler gameHandler;

    GameObject objectToPlace;
    int currentObjectAmount;

    Vector3 fallSpeed;
    float damage = 10f;
    // Start is called before the first frame update
    void Start()
    {
        LockMouse();
        fallSpeed = new Vector3(0, 0, 0);
    }


    // Update is called once per frame
    void Update()
    {
        if (gameHandler.gameState == "active")
        {
            //goes true for a frame when initiating a new selection
            newSelection = false;

            //select inventory
            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                selectUpdate(1);
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                selectUpdate(-1);
            }
            if (Input.GetKeyDown("e") && objectPlaceMode)
            {
                shadowPointer.position = new Vector3(0, -100, 0);
                objectPlaceMode = false;
            }
            else if (Input.GetKeyDown("e"))
            {
                objectPlaceMode = true;
            }

            Movement();
            Camera();
            if (objectPlaceMode)
            {
                ShadowPointer();
            }

            Jump();

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

        LayerMask mask = ~LayerMask.GetMask("Hidden Objects","RaycastCollider","Grayscale");
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

        switch (selectedIndex)
        {
            case 0:
                shadowPointer.Rotate(0.0f, 90.0f, 0.0f);
                break;
            case 1:
                shadowPointer.Rotate(0.0f, 180.0f, 0.0f);
                break;
        }

        bool check = checkIfPlaceable();
        if (Input.GetMouseButtonDown(0))
        {
            bool success = false;
            
            if (inventory.inventoryAmount[selectedIndex] > 0 && check)
            {
                Pathfinding.GridGraph graphToScan = AstarPath.active.data.gridGraph;
                AstarPath.active.Scan(graphToScan);

                success = true;
                GameObject instantiatedObject = Instantiate(objectToPlace, shadowPointer.position, shadowPointer.rotation);
                instantiatedObject.GetComponent<Turret>().gameHandler = gameHandler;
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

    bool checkIfPlaceable()
    {
        Collider[] checkSphere = Physics.OverlapSphere(shadowPointer.position,
                                 4f, LayerMask.GetMask("Solid Object"));
        if (checkSphere.Length > 0)
        {
            if (currentshadowPointer.transform.Find("BoundBox").GetComponent<Collider>().bounds.Intersects(checkSphere[0].bounds))
            {
                foreach (Transform child in currentshadowPointer.transform)
                {

                    child.gameObject.layer = LayerMask.NameToLayer("Grayscale");
                }
                return false;
            }
            else
            {
                foreach (Transform child in currentshadowPointer.transform)
                {

                    child.gameObject.layer = LayerMask.NameToLayer("Hidden Objects");
                }
                return true;
            }
        }
        else
        {
            foreach (Transform child in currentshadowPointer.transform)
            {

                child.gameObject.layer = LayerMask.NameToLayer("Hidden Objects");
            }
            return true;
        }
    }

    void Jump()
    {
        float gravity = -19;
        float jumpHeight = 2;

        fallSpeed.y += gravity * Time.deltaTime;

        if (controller.isGrounded && fallSpeed.y < -0)
        {
            fallSpeed.y = 0f;
        }

        if (Input.GetButton("Jump") && Physics.CheckSphere(transform.position, 0.4f))
        {
            fallSpeed.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        controller.Move(fallSpeed * Time.deltaTime);
    }

    void shooting()
    {
        if(Input.GetMouseButtonDown(0) && gameHandler.roundType == "attack")
        {
            RaycastHit hit;
            GameObject hitObject;
            if(Physics.Raycast(cameraMount.position, cameraMount.forward, out hit))
            {
                hitObject = hit.transform.gameObject;
                hitObject.GetComponent<Health>().health -= damage;
            }
        }
    }
}
