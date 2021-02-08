using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    [Range(2, 256)]
    public int resolution = 10;                 // resolution of each face, 256 is max for mesh in unity
    public bool autoUpdate = true;              // to set autoUpdate   --- DELETE LATER 


    [Header("ScriptableObject")]
    public ShapeSettings shapeSettings;
    public ColorSettings colorSettings;

    ShapeGenerator shapeGenerator = new ShapeGenerator();
    ColorGenerator colorGenerator = new ColorGenerator();
    // --- Seed ----
    public SeedGenerator seedGenerator; //GetComponent<SeedGenerator>();
    public bool useSeed = true;

    [SerializeField, HideInInspector]
    private MeshFilter[] meshFilters;           // array of all 6 meshes
    private Face[] faces;                      // array for all faces

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
        faces = new Face[6];

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

            faces[i] = new Face(shapeGenerator, meshFilters[i].sharedMesh, resolution, directions[i]);
        }
    }

    //called to Generate whole Planet
    public void GeneratePlanet()
    {
        if (useSeed)
        {
            SetSeed();
        }
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
        foreach (Face face in faces)
        {
            face.UpdateUVs(colorGenerator);
        }
    }

    void GenerateMesh()
    {
        foreach (Face face in faces)
        {
            face.ConstructMesh();
        }
        colorGenerator.UpdateElevation(shapeGenerator.elavationMinMax);
    }

    private void OnValidate()
    {
        GeneratePlanet();
    }

    // ------ Randomize Values ---------

    public void RandomizePlanetShape()
    {
        // set planetRadius to mesh in solarsystem.radius
        //shapeSettings.planetRadius = 100;
        // randomize shapesettings in noiseLayers[0].noiseSettings.stdNoiseSettings
        int multiplier = 1;

        for (int i = 0; i < shapeSettings.noiseLayers.Length; i++)
        {
            shapeSettings.noiseLayers[i].enabled = true;
            shapeSettings.noiseLayers[i].useFirstLayerAsMask = true;
            // set rnd NoiseFilterType but keep first at simpleNoise
            for (int j = 1; j < shapeSettings.noiseLayers.Length - 1; j++)
            {
                shapeSettings.noiseLayers[j].noiseSettings.filterType = (CNoiseSettings.FilterType)UnityEngine.Random.Range(0, 3);
            }
            shapeSettings.noiseLayers[i].noiseSettings.stdNoiseSettings.RandomValue(multiplier);
            multiplier += 225; // 125 is nice value for flat planets
        }
    }

    public void RandomizePlanetColor()
    {
        for (int i = 0; i < colorSettings.biomeColorSettings.biomes.Length; i++)
        {
            colorSettings.biomeColorSettings.biomes[i].RandomValue();
        }
        colorSettings.biomeColorSettings.RandomOceanColor();
    }

    private void SetSeed()
    {
        // set seed for generating planetshape
        int seed = seedGenerator.seed;
        UnityEngine.Random.InitState(seed);
    }

    // -------- GETTER ---------

    public string GetPlanetName { get { return GetComponent<Planet>().name; } }
    public Face[] GetFaces { get { return faces; } }
    // get resolution - public 
    // get color settings - public
    // get shape settings - public 
    // public CShapeSettings GetSetShapeSettings { get { return shapeSettings; } set { shapeSettings = value; } }
}
