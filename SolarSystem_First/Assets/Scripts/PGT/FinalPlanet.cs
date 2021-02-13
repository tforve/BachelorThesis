using System.Collections;
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
    public Face[] faces;
    public int resolution = 2;
    public Transform placeHolder;

    // necessaries
    [SerializeField, HideInInspector]
    private MeshFilter[] meshFilters;

    // Scriptable Objects
    [Header("Settings")]
    public ShapeSettings shapeSettings;
    public ColorSettings colorSettings;

    ShapeGenerator shapeGenerator = new ShapeGenerator();
    ColorGenerator colorGenerator = new ColorGenerator();

    public void Initialize()
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
                GameObject meshObj = new GameObject("facemesh"+this.name);
                meshObj.transform.SetParent(this.transform);

                meshObj.AddComponent<MeshRenderer>();
                meshFilters[i] = meshObj.AddComponent<MeshFilter>();
                meshFilters[i].sharedMesh = new Mesh();
            }
            // generate new Material based on colorSettings.planetMaterial
            meshFilters[i].GetComponent<MeshRenderer>().sharedMaterial = new Material(colorSettings.planetMaterial);
            // generate Faces
            faces[i] = new Face(shapeGenerator, meshFilters[i].sharedMesh, resolution, directions[i]);
        }

        shapeSettings.planetRadius = placeHolder.GetComponent<SolarsystemBody>().radius;
        this.transform.position = placeHolder.position;

    }

    public void GeneratePlanet()
    {
        foreach (Face f in faces)
        {
            f.ConstructMesh();
        }
        UpdateColors();
    }

    public void UpdateColors()
    {
        colorGenerator.UpdateColors();

        foreach (Face face in faces)
        {
            face.UpdateUVs(colorGenerator);
        }
    }
}
