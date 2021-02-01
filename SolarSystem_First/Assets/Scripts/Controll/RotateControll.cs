using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateControll : MonoBehaviour
{
    public float planetSpeed = 5f;
    public float sunSpeed = 5f;

    public CPlanet planet;
    public GameObject sun;

    private void RotatePlanet()
    {
        this.transform.Rotate(Vector3.down * (planetSpeed * Time.deltaTime));
        sun.transform.Rotate(Vector3.up * (sunSpeed * Time.deltaTime));
        //sun.transform.Rotate(Vector3.left * ((sunSpeed) * Time.deltaTime));

    }
    private void FixedUpdate()
    {
        RotatePlanet();
    }

    private void OnValidate()
    {
        RotatePlanet();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.N))
        {
            planet.RandomizePlanetShape();
            planet.RandomizePlanetColor();
            planet.GeneratePlanet();
        }
    }
}
