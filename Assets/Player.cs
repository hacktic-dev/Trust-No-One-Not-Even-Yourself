using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public Inventory inventory;

    //execute objectPlace code when new
    bool objectPlaceMode;
    //index for selected item
    public int selectedIndex;

    public bool newSelection;

    //the number of items in the game, update this as we add more
    const int totalItemNumber = 3;

    Vector3 move;
    public float speed = 12f;
    public CharacterController controller;
    float xRotation;
    public int test;
    public float mouseSense = 100f;
    public Transform cameraMount;
    public float maxPlaceDistance = 10f;
    Vector3 hitPosition;
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


        Movement();
        Camera();
        ShadowPointer();
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
        if (Physics.Raycast(cameraMount.position, cameraMount.forward, maxPlaceDistance))
        {
            RaycastHit hit;
            if (Physics.Raycast(cameraMount.position, cameraMount.forward, out hit))
            {
                hitPosition = hit.point;
            }
        }
		else
		{
			hitPosition = new Vector3(0, 0, 0);
        }
        
	}
	
    //execute this when in object placing mode
    void objectPlaceModeUpdate()
    {
        //TODO
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(hitPosition, 0.1f);
    }

}
