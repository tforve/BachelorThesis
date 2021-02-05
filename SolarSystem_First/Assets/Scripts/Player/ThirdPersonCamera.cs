using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public bool lockCursor;
    public float mouseSensitivity = 10.0f;
    public SolarsystemBody[] targets;
    public float distanceFromTarget = 200.0f;
    public float scrollspeed = 25.0f;

    public Vector2 pitchMinMax = new Vector2(-40.0f, 85.0f);        // to clamp Rotation in y

    public float rotationSmoothTime = 0.12f;                        // smoothing out the Camera
    Vector3 rotationSmoothVelocity;
    Vector3 currentRotation;

    private float yaw;
    private float pitch;

    private int index = 0;

    void Start()
    {
        if(lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        targets = FindObjectsOfType<SolarsystemBody>();
    }

    void LateUpdate()
    {
        yaw += Input.GetAxis("Mouse X");
        pitch -= Input.GetAxis("Mouse Y");
        pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);

        currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3 (pitch, yaw), ref rotationSmoothVelocity, rotationSmoothTime);
        this.transform.eulerAngles = currentRotation;
        this.transform.position = targets[index].transform.position - this.transform.forward * distanceFromTarget;

        // Zoom
        distanceFromTarget -= Input.mouseScrollDelta.y * scrollspeed;
        distanceFromTarget = Mathf.Clamp(distanceFromTarget, targets[index].radius + 20, 1500);

        // Switch Target
        if(Input.GetMouseButtonDown(0))
        {
            index = mod(index+1, 3);
        }
        if (Input.GetMouseButtonDown(1))
        {
            index = mod(index-1, 3);
        }

    }
    /// <summary>
    /// modulo helper function
    /// </summary>
    int mod(int x, int m)
    {
        int r = x % m;
        return r < 0 ? r + m : r;
    }
}
