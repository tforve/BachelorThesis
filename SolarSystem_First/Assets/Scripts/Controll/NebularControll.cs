using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NebularControll : MonoBehaviour
{
    public Camera target;
    public Camera orbitTarget;

    public UserControll userControll;

    private void Start()
    {
        userControll = FindObjectOfType<UserControll>();
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (userControll.useOrbitCam)
        {
            this.transform.LookAt(orbitTarget.transform);
        }
        else
        {
            this.transform.LookAt(target.transform);
        }
    }
}
