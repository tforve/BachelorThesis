using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPlanet : MonoBehaviour
{
    [Range(2, 256)]
    public int resolution = 10;                 // resolution of each face, 256 is max for mesh in unity
    public bool autoUpdate = true;              // to set autoUpdate 

    [Header("ScriptableObject")]
    public CShapeSettings shapeSettings;
    public CColorSettings colorSettings;

    CShapeGenerator shapeGenerator;

    [SerializeField, HideInInspector]
    private MeshFilter[] meshFilters;           // array of all 6 meshes
    private CFace[] faces;                      // array for all faces

    void Initialize()
    {
        // set all Shapesettings 
        shapeGenerator = new CShapeGenerator(shapeSettings);

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

                meshObject.AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Standard"));
                meshFilters[i] = meshObject.AddComponent<MeshFilter>();
                meshFilters[i].sharedMesh = new Mesh();
            }

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
        foreach (MeshFilter mesh in meshFilters)
        {
            mesh.GetComponent<MeshRenderer>().sharedMaterial.color = colorSettings.planetColor;
        }
    }

    void GenerateMesh()
    {
        foreach (CFace face in faces)
        {
            face.ConstructMesh();
        }
    }
}
