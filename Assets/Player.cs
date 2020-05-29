using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public Inventory inventory;

    Vector3 move;
    public float speed = 12f;
    public CharacterController controller;
    float xRotation;
    public float mouseSense = 100f;
    public Transform cameraMount;

    // Start is called before the first frame update
    void Start()
    {
        LockMouse();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Camera();
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
}
