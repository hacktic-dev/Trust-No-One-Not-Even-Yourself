using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

    public class Player : MonoBehaviour
{

    public AudioSource audioSource;
    public AudioClip craftSuccess;
    public AudioClip craftFail;
    public AudioClip shoot;

    public Inventory inventory;

    //execute objectPlace code when new
    public bool objectPlaceMode;
    //index for selected item
    public int selectedIndex;

    public bool newSelection;

    //the number of items in the game, update this as we add more
    const int totalItemNumber = 2;

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

    float particleTimer;
    public Health health;
    public GameObject level;

    public NavMeshSurface Navmesh;

    public GameHandler gameHandler;

    GameObject objectToPlace;
    int currentObjectAmount;

    public GameObject flag;

    Vector3 fallSpeed;
    float damage = 10f;
    // Start is called before the first frame update
    void Start()
    {
        LockMouse();
        fallSpeed = new Vector3(0, 0, 0);
    }

    public void SetMouseSens(System.Single value)
    {
        mouseSense = 50f + value * 800f;
        Debug.Log(mouseSense);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameHandler.gameState == "active")
        {

            if(health.health<=0)
            { gameHandler.gameState = "lose"; }

            if (gameHandler.newRound)
            {
                shadowPointerBarrier.SetActive(false);
                shadowPointerTurret.SetActive(false);
                Debug.Log("new loc");
                newPlayerLocation();
            }



            if (particleTimer <= 0)
            {
                particleTimer = 0;
                cameraMount.GetComponent<ParticleSystem>().Stop();

            }

            particleTimer -= Time.deltaTime;



            //goes true for a frame when initiating a new selection
            newSelection = false;

            //select inventory
            if (gameHandler.roundType == "defend")
            {
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

                if (objectPlaceMode)
                {
                    ShadowPointer();
                }
            }

            shooting();
            Movement();
            Camera();
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
                Navmesh.BuildNavMesh();

                if (objectToPlace == turret)
                {
                    instantiatedObject.GetComponent<Health>().maxHealth = 80f;
                    instantiatedObject.GetComponent<Turret>().gameHandler = gameHandler;
                }

                Debug.Log(inventory.inventoryAmount[selectedIndex]);
                inventory.inventoryAmount[selectedIndex]--;
                Debug.Log(inventory.inventoryAmount[selectedIndex]);
            }

            if (success)
            {
                audioSource.PlayOneShot(craftSuccess, 0.8f*gameHandler.MasterVolume);
            }
            else
            {
                audioSource.PlayOneShot(craftFail, 0.8f* gameHandler.MasterVolume);
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
                                 4f, LayerMask.GetMask("Solid Object","Turret"));
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
            audioSource.PlayOneShot(shoot, 0.3f * gameHandler.MasterVolume);
            cameraMount.GetComponent<ParticleSystem>().Play();
            particleTimer = 0.2f;
            float closestDistance = 10000;
            bool flagHit = false;
            int flagIndex=0;
            Debug.DrawRay(cameraMount.position, cameraMount.forward * 100f, Color.white, 1f);
            RaycastHit[] raycast = Physics.RaycastAll(cameraMount.position, cameraMount.forward, 100f);
            if (raycast.Length > 0)
            {



                for (int i = 0; i < raycast.Length; i++)
                {
                    float distance = Vector3.Distance(raycast[i].transform.position, transform.position);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        
                    }

                    if (raycast[i].transform.tag == "Turret")
                    {

                        raycast[i].transform.GetComponent<Health>().health -= damage;
                        break;
                    }
                    else if (raycast[i].transform.tag == "Flag")
                    {
                        flagHit = true;
                        flagIndex = i;
                    }
                }

                if(flagHit && closestDistance== Vector3.Distance(raycast[flagIndex].transform.position, transform.position))
                {
                    gameHandler.newRoundF();
                    CharacterController cc = this.GetComponent<CharacterController>();
                    cc.enabled = false;
                    transform.position = flag.transform.position;
                    cc.enabled = true;
                }

            }
        }
    }

    void newPlayerLocation()
    {
        if (gameHandler.roundType=="attack")
        {
            object[] obj = GameObject.FindObjectsOfType(typeof(GameObject));
            List<GameObject> spawnLocs = new List<GameObject>();
            foreach (object o in obj)
            {
                Debug.Log("test");
                GameObject g = (GameObject)o;
                if (g.tag=="EnemySpawn")
                {
                    spawnLocs.Add(g);
                }

            }

            int locationIndex = Random.Range(0, spawnLocs.Count);
            CharacterController cc = this.GetComponent<CharacterController>();
            cc.enabled = false;
            Debug.Log(locationIndex.ToString());
            transform.position = spawnLocs[locationIndex].transform.position;
            Debug.Log(transform.position);
            Debug.Log(spawnLocs[locationIndex].transform.position);
            cc.enabled = true;
        }
        else
        {
            CharacterController cc = this.GetComponent<CharacterController>();
            cc.enabled = false;
            transform.position = flag.transform.position;
            cc.enabled = true;
        }
    }

    public void reset()
    {
        health.health = 50f;
        CharacterController cc = this.GetComponent<CharacterController>();
        cc.enabled = false;
        transform.position = flag.transform.position;
        cc.enabled = true;
        inventory.reset();
    }
}
