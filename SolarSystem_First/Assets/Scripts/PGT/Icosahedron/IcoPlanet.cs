using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class IcoPlanet : MonoBehaviour
{
    public Material material;                   // material to apply
    private GameObject planetMesh;              // mesh holder

    private List<Polygon> polygons;             // list of all Polygons
    private List<Vector3> vertices;             // list of all vertices

    [Header("Subdivition")]
    [Range(0,5)][Tooltip("Unity has a default vertex limit on meshes of  5")]
    public int subdivitions;


    void Start()
    {
        // instantiate Icosahedron 
        CreateIcosahedron();
        // subdivide in n amount of triangles
        Subdivide(subdivitions);
        // generate correct Mesh
        GenerateMesh();
    }


    /// <summary>
    /// generate Icosahedron with 20 faces to subdivide later
    /// </summary>
    public void CreateIcosahedron()
    {
        polygons = new List<Polygon>();
        vertices = new List<Vector3>();
        
        // t = golden Ratio 
        float t = (1.0f + Mathf.Sqrt(5.0f)) / 2.0f;

        // cartesian coordinates, centered at the origin:
        // (  0,+-1,+-t)
        // (+-1,+-t,  0)
        // (+-t,  0,+-1)
        // Icosahedron has 12 vertices ...
        vertices.Add(new Vector3(-1,  t,  0).normalized);
        vertices.Add(new Vector3( 1,  t,  0).normalized);
        vertices.Add(new Vector3(-1, -t,  0).normalized);
        vertices.Add(new Vector3( 1, -t,  0).normalized);

        vertices.Add(new Vector3( 0, -1,  t).normalized);
        vertices.Add(new Vector3( 0,  1,  t).normalized);
        vertices.Add(new Vector3( 0, -1, -t).normalized);
        vertices.Add(new Vector3( 0,  1, -t).normalized);

        vertices.Add(new Vector3( t,  0, -1).normalized);
        vertices.Add(new Vector3( t,  0,  1).normalized);
        vertices.Add(new Vector3(-t,  0, -1).normalized);
        vertices.Add(new Vector3(-t,  0,  1).normalized);

        // ... and 20 faces build out of thos 12 vertices
        // 5 faces around point 0
        polygons.Add(new Polygon( 0, 11,  5));
        polygons.Add(new Polygon( 0,  5,  1));
        polygons.Add(new Polygon( 0,  1,  7));
        polygons.Add(new Polygon( 0,  7, 10));
        polygons.Add(new Polygon( 0, 10, 11));
        // 5 adjacent faces
        polygons.Add(new Polygon( 1,  5,  9));
        polygons.Add(new Polygon( 5, 11,  4));
        polygons.Add(new Polygon(11, 10,  2));
        polygons.Add(new Polygon(10,  7,  6));
        polygons.Add(new Polygon( 7,  1,  8));
        // second half of icosahedron
        // 5 faces around point 3
        polygons.Add(new Polygon( 3,  9,  4));
        polygons.Add(new Polygon( 3,  4,  2));
        polygons.Add(new Polygon( 3,  2,  6));
        polygons.Add(new Polygon( 3,  6,  8));
        polygons.Add(new Polygon( 3,  8,  9));
        // 5 adjacent faces
        polygons.Add(new Polygon( 4,  9,  5));
        polygons.Add(new Polygon( 2,  4, 11));
        polygons.Add(new Polygon( 6,  2, 10));
        polygons.Add(new Polygon( 8,  6,  7));
        polygons.Add(new Polygon( 9,  8,  1));
    }


    /// <summary>
    /// subdivides all Polygons/triangles. 1 triangle = 4 new triangles
    /// </summary>
    /// <param name="recursions"></param>
    public void Subdivide(int recursions)
    {
        var midPointCache = new Dictionary<int, int>();

        if(recursions < 0 || recursions > 5)
        {
            //recursions = Mathf.Clamp(recursions, 0, 5);
            Debug.Log("Unity has a default vertex limit on meshes of 5");
            return;
        }

        for (int i = 0; i < recursions; i++)
        {
            var newPolygons = new List<Polygon>();

            foreach (var poly in polygons)
            {
                int a = poly.vertices[0];
                int b = poly.vertices[1];
                int c = poly.vertices[2];

                // Use GetMidPointIndex to either create a new vertex between two old vertices,
                // or find the one that was already created.
                int ab = GetMidPointIndex(midPointCache, a, b); 
                int bc = GetMidPointIndex(midPointCache, b, c);
                int ca = GetMidPointIndex(midPointCache, c, a);

                // Create the four new polygons using our original three vertices, and the three new midpoints.
                newPolygons.Add(new Polygon(a, ab, ca));
                newPolygons.Add(new Polygon(b, bc, ab));
                newPolygons.Add(new Polygon(c, ca, bc));
                newPolygons.Add(new Polygon(ab, bc, ca));
            }
            // Replace all our old polygons with the new set of subdivided ones.
            polygons = newPolygons;
        }


    }

    public int GetMidPointIndex(Dictionary<int, int> cache, int indexA, int indexB)
    {
        // create a key out of the two original indices
        // by storing the smaller index in the upper two bytes
        // of an integer, and the larger index in the lower two
        // bytes. By sorting them according to whichever is smaller
        // we ensure that this function returns the same result
        // whether you call
        // GetMidPointIndex(cache, 5, 9)
        // or...
        // GetMidPointIndex(cache, 9, 5)

        int smallerIndex = Mathf.Min(indexA, indexB);
        int greaterIndex = Mathf.Max(indexA, indexB);
        int key = (smallerIndex << 16) + greaterIndex;

        // If a midpoint is already defined, just return it.
        int ret;
        if (cache.TryGetValue(key, out ret))
            return ret;

        // If we're here, it's because a midpoint for these two
        // vertices hasn't been created yet.

        Vector3 p1 = vertices[indexA];
        Vector3 p2 = vertices[indexB];
        Vector3 middle = Vector3.Lerp(p1, p2, 0.5f).normalized;

        ret = vertices.Count;
        vertices.Add(middle);

        // Add our new midpoint to the cache so we don't have to do this again.

        cache.Add(key, ret);
        return ret;
    }

    /// <summary>
    /// generate mesh of the Icosahedron Planet
    /// </summary>
    public void GenerateMesh()
    {
        // check if existing. if so destroy and recreate
        if (planetMesh)
            Destroy(planetMesh);

        planetMesh = new GameObject("Planet Mesh");

        // add necessary components
        MeshRenderer surfaceRenderer = planetMesh.AddComponent<MeshRenderer>();
        surfaceRenderer.material = material;
        Mesh terrainMesh = new Mesh();

        // 1 polygon has 3 vertices
        int vertexCount = polygons.Count * 3;

        int[] indices = new int[vertexCount];

        Vector3[] vertices = new Vector3[vertexCount];
        Vector3[] normals  = new Vector3[vertexCount];
        Color32[] colors   = new Color32[vertexCount];

        Color32 green = new Color32(20,  255, 30, 255);
        Color32 brown = new Color32(220, 150, 70, 255);

        // loop through all Polygons
        for (int i = 0; i < polygons.Count; i++)
        {
            // get polygon
            var poly = polygons[i];

            // giving every polygon 3 indices counting up from 0
            indices[i * 3 + 0] = i * 3 + 0;
            indices[i * 3 + 1] = i * 3 + 1;
            indices[i * 3 + 2] = i * 3 + 2;
            
            vertices[i * 3 + 0] = this.vertices[poly.vertices[0]];
            vertices[i * 3 + 1] = this.vertices[poly.vertices[1]];
            vertices[i * 3 + 2] = this.vertices[poly.vertices[2]];

            // setting rnd color for now to debug
            Color32 polyColor = Color32.Lerp(green, brown, Random.Range(0.0f, 1.0f)); 

            colors[i * 3 + 0] = polyColor;
            colors[i * 3 + 1] = polyColor;
            colors[i * 3 + 2] = polyColor;

            // setting normal vectors
            normals[i * 3 + 0] = this.vertices[poly.vertices[0]];
            normals[i * 3 + 1] = this.vertices[poly.vertices[1]];
            normals[i * 3 + 2] = this.vertices[poly.vertices[2]];
        }

        // apply all saves to the mesh
        terrainMesh.vertices = vertices;
        terrainMesh.normals  = normals;
        terrainMesh.colors32 = colors;

        terrainMesh.SetTriangles(indices, 0);

        MeshFilter terrainFilter = planetMesh.AddComponent<MeshFilter>();
        terrainFilter.mesh = terrainMesh;
    }
}
