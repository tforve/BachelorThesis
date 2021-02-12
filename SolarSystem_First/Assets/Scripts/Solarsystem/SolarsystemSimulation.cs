using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarsystemSimulation : MonoBehaviour
{
    #region SINGLETON PATTERN
    private static SolarsystemSimulation _instance;
    public static SolarsystemSimulation Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<SolarsystemSimulation>();

                if (_instance == null)
                {
                    GameObject container = new GameObject("SolarsystemSimulation");
                    _instance = container.AddComponent<SolarsystemSimulation>();
                }
            }


            return _instance;
        }

    }
    #endregion


    public SolarsystemBody[] planets;               // array to store all celestial bodies

    public bool recalculateStartvelocity = false;
    public bool useFixStartvelocity = false;

    private void Awake()
    {
        // populate array of planets
        planets = FindObjectsOfType<SolarsystemBody>();

        // set fixedDeltaTime to own Universe time for more controll
        Time.fixedDeltaTime = Universe.timeSteps;

        // calculate StartVelocity
        if (useFixStartvelocity)
        {
            for (int i = 0; i < planets.Length; i++)
            {
                planets[i].CalculateStartVelocity(planets);
            }
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

    /// <summary>
    /// calculate Startvelocity for debugging by using recalculateStartvelocity boolean 
    /// </summary>
    void OnValidate()
    {
        if (recalculateStartvelocity && useFixStartvelocity)
        {
            planets = FindObjectsOfType<SolarsystemBody>();
            Time.fixedDeltaTime = Universe.timeSteps;
            for (int i = 0; i < planets.Length; i++)
            {
                planets[i].CalculateStartVelocity(planets);
            }
            recalculateStartvelocity = false;
        }
    }
}
