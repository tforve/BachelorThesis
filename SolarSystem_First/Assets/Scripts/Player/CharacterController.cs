using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController: MonoBehaviour
{
    CharacterController characterController;


    [Header("Move Values")]
    public float flySpeed = 7.5f;
    public float boostedSpeed = 11.5f;

    Vector3 moveDirection = Vector3.zero;
    Camera characterCamera;
    bool useBoost = false;

    [Header("Look")]
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;
    float rotationX = 0;

    private Rigidbody rb;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        characterController = GetComponent<CharacterController>();
        characterCamera = GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalMovement = Input.GetAxisRaw("Horizontal");
        float verticalMovement = Input.GetAxisRaw("Vertical");
        moveDirection = (horizontalMovement * transform.right + verticalMovement * transform.forward).normalized;
        moveDirection = characterCamera.transform.TransformDirection(moveDirection);

        // Press Left Shift to run
        useBoost = Input.GetKey(KeyCode.LeftShift);

        //float curSpeedX = (useBoost ? boostedSpeed : flySpeed) * Input.GetAxis("Vertical");
        //float curSpeedY = (useBoost ? boostedSpeed : flySpeed) * Input.GetAxis("Horizontal");
        //float movementDirectionY = moveDirection.y;
        //moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        //moveDirection.y = movementDirectionY;

        //// Player and Camera rotation

        //rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
        //rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        //characterCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        //transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);

    }
    private void FixedUpdate()
    {
        rb.velocity = moveDirection * (useBoost ? boostedSpeed : flySpeed) * Time.deltaTime;
    }
}
