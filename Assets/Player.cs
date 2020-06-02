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
    public float mouseSense = 50f;
    public Transform cameraMount;
    public float maxPlaceDistance = 20f;
    Vector3 hitPosition;

    public GameObject shadowPointer;
    public GameObject shadowPointerBarrier;
    public GameObject shadowPointerTurret;
    GameObject currentshadowPointer;

    public AudioClip hurtSound;

    float particleTimer;
    public Health health;

    public NavMeshSurface Navmesh;

    public GameHandler gameHandler;

    GameObject objectToPlace;
    int currentObjectAmount;

    public GameObject flag;

    Vector3 fallSpeed;

    bool flagHitStartCount;
    float flagCount;

    float damage = 10f;
    // Start is called before the first frame update
    void Start()
    {
        LockMouse();
        fallSpeed = new Vector3(0, 0, 0);
    }

    public void SetMouseSens(System.Single value)
    {
        mouseSense = 20f + value * 400f;
     //   Debug.Log(mouseSense);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameHandler.gameState == "active")
        {

            if(flagCount<0f)

            {
                flagHitStartCount = false;
                flagCount = 1f;
                gameHandler.newRoundF();
                gameHandler.roundType = "defend";
                CharacterController cc = this.GetComponent<CharacterController>();
                cc.enabled = false;
                transform.position = flag.transform.position- new Vector3(0, 6, 0); ;
                cc.enabled = true;
            }

            if(flagHitStartCount)
            { flagCount -= Time.deltaTime; }



            if(health.health<=0)
            { StartCoroutine(ExecuteAfterTime(0.15f)); }



            if (gameHandler.newRound)
            {
                shadowPointerBarrier.SetActive(false);
                shadowPointerTurret.SetActive(false);
              //  Debug.Log("new loc");
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
                    shadowPointer.transform.position = new Vector3(0, -100, 0);
                    objectPlaceMode = false;
                }
                else if (Input.GetKeyDown("e"))
                {
                    objectPlaceMode = true;
                }

                if (objectPlaceMode)
                {
                    shadowPointer.SetActive(true);
                    ShadowPointer();
                }
                else
                {
                    hitPosition = new Vector3(1000, 0, 0);
                    shadowPointer.SetActive(false);
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

        //activate selected shadow pointer
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


        Vector3 hitNormal=new Vector3(0f,0f,0f);

        LayerMask mask = ~LayerMask.GetMask("Shadow","No Collision","RaycastCollider","Player","Turret");

        if (Physics.Raycast(cameraMount.position, cameraMount.forward, maxPlaceDistance, mask) && inventory.inventoryAmount[selectedIndex]>0)
        {
            shadowPointer.SetActive(true);
            RaycastHit hit;
            if (Physics.Raycast(cameraMount.position, cameraMount.forward, out hit, maxPlaceDistance + 1, mask))
            {
                hitPosition = hit.point;
                hitNormal = hit.normal;
                Debug.Log(hit.normal);
            }
        }
		else
		{
            
			hitPosition = new Vector3(1000, 0, 0);

        }

        shadowPointer.transform.position = hitPosition;
        shadowPointer.transform.rotation = selfTransform.rotation;

        //rotate according to which pointer is active
        switch (selectedIndex)
        {
            case 0:
                shadowPointer.transform.Rotate(0.0f, 90.0f, 0.0f);
                break;
            case 1:
                shadowPointer.transform.Rotate(0.0f, 180.0f, 0.0f);
                break;
        }


        bool check = checkIfPlaceable(hitNormal);

      //  Debug.Log("placeable" + check);
        if (Input.GetMouseButtonDown(0))
        {
            bool success = false;
            
            if (inventory.inventoryAmount[selectedIndex] > 0 && check)
            {
                

                success = true;
                GameObject instantiatedObject = Instantiate(objectToPlace, shadowPointer.transform.position, shadowPointer.transform.rotation);

                //Navmesh.BuildNavMesh();

                if (objectToPlace == turret)
                {
                    instantiatedObject.transform.position =new Vector3(instantiatedObject.transform.position.x, instantiatedObject.transform.position.y - 1, instantiatedObject.transform.position.z);
                    instantiatedObject.GetComponent<Health>().maxHealth = 50f;
                    instantiatedObject.GetComponent<Health>().health = 50f;
                    instantiatedObject.GetComponent<Turret>().gameHandler = gameHandler;
                }

               // Debug.Log(inventory.inventoryAmount[selectedIndex]);
                inventory.inventoryAmount[selectedIndex]--;
              //  Debug.Log(inventory.inventoryAmount[selectedIndex]);
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
	

    bool checkIfPlaceable(Vector3 hitNormal)
    {
        if ( hitNormal != new Vector3(0f, 1f, 0f) || Vector3.Distance(currentshadowPointer.transform.position,transform.position)<1.5)
        {
            setTransparency(currentshadowPointer, 0.2f);
            return false;
        }

        Collider[] checkSphere = Physics.OverlapSphere(shadowPointer.transform.position,
                                 4f, LayerMask.GetMask("Solid Object"));
        if (checkSphere.Length > 0)
        {
            
            if (currentshadowPointer.transform.Find("BoundBox").GetComponent<Collider>().bounds.Intersects(checkSphere[0].bounds))
            {
                setTransparency(currentshadowPointer, 0.2f);
                return false;
            }
            else
            {
                setTransparency(currentshadowPointer, 0.8f);
                return true;
            }
        }
        else
        {
            setTransparency(currentshadowPointer, 0.8f);
            return true;
        }
    }

    void setTransparency(GameObject gameObject,float transparency)
    {
        foreach (Transform child in currentshadowPointer.GetComponentsInChildren<Transform>())
        {
            if (child.transform.tag =="Shadow")
            {
                Color tempColor = child.gameObject.GetComponent<Renderer>().material.color;
                tempColor.a = transparency;
                child.gameObject.GetComponent<Renderer>().material.SetColor("_Color", tempColor);
            }
        }
    }

    public void craftItem(string itemName)
    {
        bool success = false;
        if (itemName == "barrier")
        {
            if (inventory.resourceAmount["matter"] >= 2 && inventory.resourceAmount["force"] >= 1)
            {
                inventory.inventoryAmount[0]++;
                inventory.resourceAmount["matter"] -= 2;
                inventory.resourceAmount["force"] -= 1;
                success = true;
            }

        }
        else if (itemName == "still turret")
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

    void Jump()
    {
        float gravity = -19;
        float jumpHeight = 2;
        LayerMask mask = ~LayerMask.GetMask("Hidden Objects","No Collision","Shadow","Player");

        fallSpeed.y += gravity * Time.deltaTime;

        if (controller.isGrounded && fallSpeed.y < -0)
        {
            fallSpeed.y = 0f;
        }

        if (Input.GetButton("Jump") && Physics.CheckSphere(transform.position, 0.4f, mask))
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
                    flagHitStartCount = true;
                    flagCount = 0.3f;
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
                //Debug.Log("test");
                GameObject g = (GameObject)o;
                if (g.tag=="EnemySpawn")
                {
                    spawnLocs.Add(g);
                }

            }

            int locationIndex = Random.Range(0, spawnLocs.Count);
            CharacterController cc = this.GetComponent<CharacterController>();
            cc.enabled = false;
           // Debug.Log(locationIndex.ToString());
            transform.position = spawnLocs[locationIndex].transform.position;
           // Debug.Log(transform.position);
           // Debug.Log(spawnLocs[locationIndex].transform.position);
            cc.enabled = true;
        }
        else
        {
            CharacterController cc = this.GetComponent<CharacterController>();
            cc.enabled = false;
            transform.position = flag.transform.position-new Vector3(0,6,0);
            cc.enabled = true;
        }
    }

    public void reset()
    {
        health.health = 100f;
        CharacterController cc = this.GetComponent<CharacterController>();
        cc.enabled = false;
        transform.position = flag.transform.position- new Vector3(0, 6, 0); ;
        cc.enabled = true;
        inventory.reset();
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        gameHandler.gameState = "lose";
    }

    IEnumerator SoundAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        audioSource.PlayOneShot(hurtSound, 0.8f * gameHandler.MasterVolume);
    }

    public void PlayHurt()
    {
        StartCoroutine(SoundAfterTime(0.0f));


    }

}
