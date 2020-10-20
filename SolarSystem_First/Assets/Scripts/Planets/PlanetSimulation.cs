using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetSimulation : MonoBehaviour
{
    #region SINGLETON PATTERN
    private static PlanetSimulation instance;
    public static PlanetSimulation Instance
    {
        get
        {
            if(instance == null)
            {
                instance = GameObject.FindObjectOfType<PlanetSimulation>();
            }

            return instance;
        }
        
    }
    #endregion


    public PlanetBody[] planets;

    private void Awake()
    {
        planets = FindObjectsOfType<PlanetBody>();
        Time.fixedDeltaTime = Universe.timeSteps;

        // calculate StartVelocity
        for (int i = 0; i < planets.Length; i++)
        {
            planets[i].CalculateStartVelocity(planets);
        }
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < planets.Length; i++)
        {
            planets[i].UpdateVelocity(planets, Universe.timeSteps);
        }

        for (int i = 0; i < planets.Length; i++)
        {
            planets[i].UpdatePosition(Universe.timeSteps);
        }
    }
}
