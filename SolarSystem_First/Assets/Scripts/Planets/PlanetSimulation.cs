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


    public PlanetBody[] planets;            // array to store all celestial bodies

    public bool recalculateStartvelocity = false;

    private void Awake()
    {
        // populate array of planets
        planets = FindObjectsOfType<PlanetBody>();
        // set fixedDeltaTime to own Universe time for more controll
        Time.fixedDeltaTime = Universe.timeSteps;

        // calculate StartVelocity
        for (int i = 0; i < planets.Length; i++)
        {
            planets[i].CalculateStartVelocity(planets);
        }
    }

    private void FixedUpdate()
    {
        // recalculate velocity every timeStep
        for (int i = 0; i < planets.Length; i++)
        {
            planets[i].UpdateVelocity(planets, Universe.timeSteps);
        }
        // use calculated velocity to update Position of planets
        for (int i = 0; i < planets.Length; i++)
        {
            planets[i].UpdatePosition(Universe.timeSteps);
        }
    }

    
    void OnValidate()
    {
        // calculate Startvelocity for debugging in Editor 
        if(recalculateStartvelocity)
        {
            // populate planets array
            planets = FindObjectsOfType<PlanetBody>();
            // set fixedDeltaTime to own Universe time for more controll
            Time.fixedDeltaTime = Universe.timeSteps;

            // calculate StartVelocity
            for (int i = 0; i < planets.Length; i++)
            {
                planets[i].CalculateStartVelocity(planets);
            }
            recalculateStartvelocity = false;
        }
    }
}
