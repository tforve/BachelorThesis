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
    private FinalPlanet[] finalPlanet;

    // values to copy
    private CFace[] faces;
    private string originName;
    private int originResolution;
    private CColorSettings originColSettings;
    private CShapeSettings originShapeSettings;


    private void Awake()
    {

        planetToCopy = FindObjectOfType<CPlanet>();
        StoreParameters();

        finalPlanet = FindObjectsOfType<FinalPlanet>();
        ApplyParameters();
    }


    void StoreParameters()
    {
        faces = planetToCopy.GetFaces;
        originName = planetToCopy.name;
        originResolution = planetToCopy.resolution;
        originColSettings = planetToCopy.colorSettings;
        originShapeSettings = planetToCopy.shapeSettings;
    }

    /// <summary>
    /// apply values to final planet object
    /// </summary>
    void ApplyParameters()
    {
        // now go through all planets - later one by one with different settings !!!!!!!!!!
        for (int i = 0; i < finalPlanet.Length; i++)
        {
            finalPlanet[i].faces = faces;
            finalPlanet[i].name = originName;
            finalPlanet[i].resolution = originResolution;
            finalPlanet[i].colorSettings = originColSettings;
            finalPlanet[i].shapeSettings = originShapeSettings;
        }
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
