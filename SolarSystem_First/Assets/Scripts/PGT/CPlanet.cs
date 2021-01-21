using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPlanet : MonoBehaviour
{
    [Range(2, 256)]
    public int resolution = 10;                 // resolution of each face, 256 is max for mesh in unity
    public bool autoUpdate = true;              // to set autoUpdate   --- DELETE LATER 

    [Header("ScriptableObject")]
    public CShapeSettings shapeSettings;        
    public CColorSettings colorSettings;

    CShapeGenerator shapeGenerator = new CShapeGenerator();
    CColorGenerator colorGenerator = new CColorGenerator();

    [SerializeField, HideInInspector]
    private MeshFilter[] meshFilters;           // array of all 6 meshes
    private CFace[] faces;                      // array for all faces

    void Initialize()
    {
        // set all settings
        shapeGenerator.UpdateSettings(shapeSettings);
        colorGenerator.UpdateSettings(colorSettings);

        // check if null
        if (meshFilters == null || meshFilters.Length == 0)
        {
            meshFilters = new MeshFilter[6];
        }
        // init faces
        faces = new CFace[6];

        Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

        for (int i = 0; i < 6; i++)
        {
            // check if already existing
            if (meshFilters[i] == null)
            {
                GameObject meshObject = new GameObject("mesh");
                meshObject.transform.parent = transform;

                meshObject.AddComponent<MeshRenderer>();
                meshFilters[i] = meshObject.AddComponent<MeshFilter>();
                meshFilters[i].sharedMesh = new Mesh();
            }
            // set material to MeshRenderer ( in this case right now its an PBR Shader)
            meshFilters[i].GetComponent<MeshRenderer>().sharedMaterial = colorSettings.planetMaterial;

            faces[i] = new CFace(shapeGenerator, meshFilters[i].sharedMesh, resolution, directions[i]);
        }
    }




    //called to Generate whole Planet
    public void GeneratePlanet()
    {
        Initialize();
        GenerateMesh();
        GenerateColors();
    }

    // called if ShapeSettings changed
    public void OnShapeSettingsUpdated()
    {
        if (autoUpdate)
        {
            Initialize();
            GenerateMesh();
        }
    }

    // called wenn Colorsettings changed
    public void OnColorSettingsUpdate()
    {
        if (autoUpdate)
        {
            Initialize();
            GenerateColors();
        }
    }

    // loop through meshes and set colors
    void GenerateColors()
    {
        colorGenerator.UpdateColors();
        foreach (CFace face in faces)
        {
            face.UpdateUVs(colorGenerator);
        }
    }

    void GenerateMesh()
    {
        foreach (CFace face in faces)
        {
            face.ConstructMesh();
        }
        colorGenerator.UpdateElevation(shapeGenerator.elavationMinMax);
    }

    private void OnValidate()
    {
        GeneratePlanet();
    }

    // -------- GETTER ---------

    public string GetPlanetName { get { return GetComponent<CPlanet>().name; } }
    public CFace[] GetFaces { get { return faces; } }
    // get resolution - public 
    // get color settings - public
    // get shape settings - public 
    public CShapeSettings GetSetShapeSettings { get { return shapeSettings; } set { shapeSettings = value; } }

    // -------- Randomize Values ---------

    public void RandomizePlanetShape()
    {
        // set planetRadius to mesh in solarsystem.radius
        shapeSettings.planetRadius = 100;

        // randomize shapesettings in noiseLayers[0].noiseSettings.stdNoiseSettings
        int multiplier = 1;

        for (int i = 0; i < shapeSettings.noiseLayers.Length; i++)
        {
            shapeSettings.noiseLayers[i].noiseSettings.stdNoiseSettings.RandomValue(multiplier);
            multiplier += 1000;
        }
    }

    public void ResetShape()
    {

        for (int i = 0; i < shapeSettings.noiseLayers.Length; i++)
        {
            shapeSettings.noiseLayers[i].noiseSettings.stdNoiseSettings.ResetValues();
        }
    }

}
