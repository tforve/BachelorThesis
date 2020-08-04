﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowTarget : MonoBehaviour
{
    public Transform targetObj;


    public float followDistance = 5.0f;
    public float followHeight = 2.0f;

    public bool smoothedFollow = false;         //toggle this for hard or smoothed follow
    public float smoothSpeed = 5f;
    public bool useFixedLookDirection = false;      //optional different camera mode... fixed look direction
    public Vector3 fixedLookDirection = Vector3.one;

    // Use this for initialization
    void Start()
    {
        //do something when game object is activated .. if you want to

    }

    // Update is called once per frame
    void Update()
    {
        //get a vector pointing from camera towards the ball

        Vector3 lookToward = targetObj.position - transform.position;
        if (useFixedLookDirection)
            lookToward = fixedLookDirection;


        //make it stay a fixed distance behind ball
        Vector3 newPos;
        newPos = targetObj.position - lookToward.normalized * followDistance;
        newPos.y = targetObj.position.y + followHeight;

        if (!smoothedFollow)
        {
            transform.position = newPos;    //move exactly to follow target
        }
        else   //  smoothed / soft follow
        {
            transform.position += (newPos - transform.position) * Time.deltaTime * smoothSpeed;
        }

        //re- calculate look direction (dont' do this line if you want to lag the look a little
        lookToward = targetObj.position - transform.position;

        //make this camera look at target
        transform.forward = lookToward.normalized;
    }
}
