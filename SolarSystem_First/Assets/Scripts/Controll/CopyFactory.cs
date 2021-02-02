using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyFactory : MonoBehaviour
{
    #region SINGLETON PATTERN
    private static CopyFactory _instance;

    public static CopyFactory Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<CopyFactory>();

                if (_instance == null)
                {
                    GameObject container = new GameObject("CopyFactory");
                    _instance = container.AddComponent<CopyFactory>();
                }
            }
            return _instance;
        }
    }
    #endregion

    /* getting all Data from Planet
     * store them
     * give them to another Object = final Planet
     * Do it One by One - in between randomize Planet to get n variants of planets
     */

    private CPlanet planetToCopy;
    private FinalPlanet[] finalPlanets;

    // --- values to copy ---
    // Color and Shape
    private CFace[] faces;
    private int originResolution;
    private CColorSettings originColSettings;
    private CShapeSettings originShapeSettings;

    // gravity Simulation related
    //private SolarsystemBody solarsystemBody;


    private void Awake()
    {
        // get values from Blueprintplanet
        planetToCopy = FindObjectOfType<CPlanet>();
        finalPlanets = FindObjectsOfType<FinalPlanet>();

        // must apply parameters to first planet then randomize blueprint planet again then apply on next FinalPlanet
        SetRandomPlanets();
    }

    void SetRandomPlanets()
    {
        for (int i = 0; i < finalPlanets.Length; i++)
        {
            Debug.Log(finalPlanets[i].name);
            // randomize Blueprintplanet  
            //planetToCopy.RandomizePlanetColor();
            //planetToCopy.RandomizePlanetShape();
            StoreParameters();

            // apply to first finalPlanet
            ApplyParameters(finalPlanets[i]);
           // finalPlanets[i].GeneratePlanet();
        }
    }

    void StoreParameters()
    {
        faces = planetToCopy.GetFaces;
        originResolution = planetToCopy.resolution;
        originColSettings = planetToCopy.colorSettings;
        originShapeSettings = planetToCopy.shapeSettings;
    }

    /// <summary>
    /// apply values to final planet object
    /// </summary>
    void ApplyParameters(FinalPlanet planet)
    {
        planet.faces = faces;
        planet.resolution = originResolution;
        planet.colorSettings = originColSettings;
        planet.shapeSettings = originShapeSettings;

        planet.fpMaterial = originColSettings.planetMaterial; //debug
    }

    /// <summary>
    /// if FinalPlanet calls for an Update, called by FinalPlanet itself
    /// </summary>
    /// <param name="planetWhoAsk"></param>
    public void UpdateParameters(FinalPlanet planetWhoAsk)
    {
        planetWhoAsk.faces = planetToCopy.GetFaces;
        planetWhoAsk.resolution = planetToCopy.resolution;
        // redundante if not having own Settings - because we already give planetToCopySettings
        //planetWhoAsk.shapeSettings = planetToCopy.shapeSettings;
        //planetWhoAsk.colorSettings = planetToCopy.colorSettings;
    }

}
