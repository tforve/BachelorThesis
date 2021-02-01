﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalPlanet : MonoBehaviour
{
    /* apply all parameters from Planet to himself
    * create himselft 
    * instantiate himself on placeHolder.transform
    * end
    */

    // copied parameters
    public CFace[] faces;
    public int resolution = 2;
    public Transform placeHolder;

    // necessaries
    [SerializeField, HideInInspector]
    private MeshFilter[] meshFilters;
    private CopyFactory copyFactory;

    // Scriptable Objects
    [Header("Settings")]
    public CShapeSettings shapeSettings;
    public CColorSettings colorSettings;

    CShapeGenerator shapeGenerator = new CShapeGenerator();
    CColorGenerator colorGenerator = new CColorGenerator();

    // debug Only
    public bool isUpdated = false;             // update Planet Values to check if different variants are possible

    private void Start()
    {
        copyFactory = FindObjectOfType<CopyFactory>();
        Initialize();
        GeneratePlanet();
        this.transform.position = placeHolder.position;
    }

    private void Update()
    {
        if (isUpdated)
        {
            UpdatePlanet();
            isUpdated = false;
        }
    }

    void Initialize()
    {
        shapeGenerator.UpdateSettings(shapeSettings);
        colorGenerator.UpdateSettings(colorSettings);

        if (meshFilters == null || meshFilters.Length == 0)
        {
            meshFilters = new MeshFilter[6];
        }

        // all cardinal directions
        Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

        for (int i = 0; i < 6; i++)
        {
            if (meshFilters[i] == null)
            {
                GameObject meshObj = new GameObject("facemesh");
                meshObj.transform.SetParent(this.transform);

                meshObj.AddComponent<MeshRenderer>();
                meshFilters[i] = meshObj.AddComponent<MeshFilter>();
                meshFilters[i].sharedMesh = new Mesh();
            }

            meshFilters[i].GetComponent<MeshRenderer>().sharedMaterial = colorSettings.planetMaterial;
            // generate Faces
            faces[i] = new CFace(shapeGenerator, meshFilters[i].sharedMesh, resolution, directions[i]);
        }

        shapeSettings.planetRadius = placeHolder.GetComponent<SolarsystemBody>().radius;
    }

    void GeneratePlanet()
    {
        foreach (CFace f in faces)
        {
            f.ConstructMesh();
        }
        UpdateColors();
    }

    void UpdateColors()
    {
        colorGenerator.UpdateColors();
        foreach (CFace face in faces)
        {
            face.UpdateUVs(colorGenerator);
        }
    }

    /// <summary>
    /// Need to be changed later. Not Call the Copyfactory - NOT NEEDED AT ALL?
    /// </summary>
    void UpdatePlanet()
    {
        // ask CopyFactory for updated Values and apply them to finalPlanet
        copyFactory.UpdateParameters(this);
        // Update Shape based on ShapeSettings
        // Update Color based on ColorSettings
        UpdateColors();
        // reinitialize finalPlanet
        Initialize();
        GeneratePlanet();
    }


}
