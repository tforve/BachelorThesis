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
            if(_instance == null)
            {
                _instance = GameObject.FindObjectOfType<SolarsystemSimulation>();
            }

            return _instance;
        }
        
    }
    #endregion


    public SolarsystemBody[] planets;            // array to store all celestial bodies

    public bool recalculateStartvelocity = false;

    private void Awake()
    {
        // populate array of planets
        //GameObject[] tmp = GameObject.FindGameObjectsWithTag("Planet");
        //planets = new SolarsystemBody[tmp.Length];
        //for (int i = 0; i < tmp.Length; i++)
        //{
        //    planets[i] = tmp[i].gameObject.GetComponent<SolarsystemBody>();
        //}
        planets = FindObjectsOfType<SolarsystemBody>();

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

            //GameObject[] tmp = GameObject.FindGameObjectsWithTag("Planet");
            //planets = new SolarsystemBody[tmp.Length];
            //for (int i = 0; i < tmp.Length; i++)
            //{
            //    planets[i] = tmp[i].gameObject.GetComponent<SolarsystemBody>();
            //}
            planets = FindObjectsOfType<SolarsystemBody>();
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
