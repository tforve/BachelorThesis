using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController: MonoBehaviour
{
    CharacterController characterController;


    [Header("Move Values")]
    public float flySpeed = 10000.0f;
    public float boostedSpeed = 100000.0f;

    private Vector3 moveDirection = Vector3.zero;
    //private Camera characterCamera;
    private bool useBoost = false;

    [Header("Look")]
    public  float lookSpeed = 2.0f;
    private float rotationX = 0;
    private float rotationY = 0;

    private Rigidbody rb;

    // ---- ADD Gravitation to Player later --------

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        characterController = GetComponent<CharacterController>();
        Cursor.visible = false;
    }

    void Update()
    {
        float horizontalMovement = Input.GetAxisRaw("Horizontal");
        float verticalMovement = Input.GetAxisRaw("Vertical");
        moveDirection = (horizontalMovement * transform.right + verticalMovement * transform.forward).normalized;

        // Press Left Shift to use boost
        useBoost = Input.GetKey(KeyCode.LeftShift);

        // Player and Camera rotation
        rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
        rotationY += Input.GetAxis("Mouse X") * lookSpeed;
        this.transform.localRotation = Quaternion.Euler(rotationX, rotationY, 0);

    }
    private void FixedUpdate()
    {
        rb.velocity = moveDirection * (useBoost ? boostedSpeed : flySpeed) * Time.deltaTime;
    }
}
