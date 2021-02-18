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

    private BlueprintPlanet planetToCopy;
    private FinalPlanet[] finalPlanets;

    // --- values to copy ---
    // Color and Shape
    private Face[] faces;
    private int originResolution;
    private ColorSettings originColSettings;
    private ShapeSettings originShapeSettings;

    private void Awake()
    {
        planetToCopy = FindObjectOfType<BlueprintPlanet>();
        finalPlanets = FindObjectsOfType<FinalPlanet>();

        InitPlanets();
        RandomizePlanets();
    }

    void InitPlanets()
    {
        for (int i = 0; i < finalPlanets.Length; i++)
        {
            StoreParameters();
            ApplyParameters(finalPlanets[i]);
            finalPlanets[i].Initialize();
            finalPlanets[i].GeneratePlanet();
        }
    }

    /// <summary>
    /// randomize Planet by randomize blueprintPlanet then store, apply and update finalPlanet
    /// </summary>
    void RandomizePlanets()
    {
        for (int i = 0; i < finalPlanets.Length; i++)
        {
            planetToCopy.RandomizePlanetColor();
            planetToCopy.RandomizePlanetShape();

            StoreParameters();
            ApplyParameters(finalPlanets[i]);
            UpdateParameters(finalPlanets[i]);
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
    }

    /// <summary>
    /// Update Parameters by use Planet.UpdateColors, Initialize and Regenerate
    /// </summary>
    public void UpdateParameters(FinalPlanet planet)
    {
        planet.faces = planetToCopy.GetFaces;
        planet.resolution = planetToCopy.resolution;
        planet.UpdateColors();
        planet.Initialize();
        planet.GeneratePlanet();
    }

}
