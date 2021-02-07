using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// By creating every Face on its owne we can later easily change the LOD 
/// </summary>
public class Face
{
    private ShapeGenerator shapeGenerator;     // shapesettings 
    private Mesh mesh;
    private int resolution;                     // LOD of the Face
    private Vector3 localUp;                    // facing direction, normal vector
    private Vector3 axisA, axisB;               // other 2 dir vectors

    public Face(ShapeGenerator shapeGenerator, Mesh mesh, int resolution, Vector3 localUp)
    {
        this.shapeGenerator = shapeGenerator;
        this.mesh = mesh;
        this.resolution = resolution;
        this.localUp = localUp;

        axisA = new Vector3(localUp.y, localUp.z, localUp.x);
        axisB = Vector3.Cross(localUp, axisA);
    }

    public void ConstructMesh()
    {
        Vector3[] vertices = new Vector3[resolution * resolution];
        // getting nb of vertices; nmb of faces = (r-1)^2; *2 for triangles; *3 to get all vertices for every triangle 
        int[] triangles = new int[(resolution - 1) * (resolution - 1) * 6];

        // index of triangle 
        int triangleIndex = 0;
        // save UVs
        Vector2[] uv;
        if (mesh.uv.Length == vertices.Length)
        { uv = mesh.uv; }
        else { uv = new Vector2[vertices.Length]; }

        for (int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++)
            {
                // index of vertices
                int i = x + y * resolution;
                Vector2 percent = new Vector2(x, y) / (resolution - 1); // why -1
                Vector3 pointOnUnitCube = localUp + (percent.x - 0.5f) * 2 * axisA + (percent.y - 0.5f) * 2 * axisB;
                // get vertices same distance to center of cube
                Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;
                // calculate elevation and set them to the vertices
                float unscaledElevation = shapeGenerator.CalculateUnscaledElevation(pointOnUnitSphere);
                vertices[i] = pointOnUnitSphere * shapeGenerator.GetScaledElevation(unscaledElevation);
                // set UVs
                uv[i].y = unscaledElevation;

                // check if index is not outside the face
                if (x != resolution - 1 && y != resolution - 1)
                {
                    // first triangle, clockwise
                    triangles[triangleIndex] = i;
                    triangles[triangleIndex + 1] = i + resolution + 1;
                    triangles[triangleIndex + 2] = i + resolution;
                    // second triangle, clockwise
                    triangles[triangleIndex + 3] = i;
                    triangles[triangleIndex + 4] = i + 1;
                    triangles[triangleIndex + 5] = i + resolution + 1;
                    // update triangleIndex by 6 because we created 6 triangle
                    triangleIndex += 6;
                }
            }
        }
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.uv = uv;
    }


    public void UpdateUVs(ColorGenerator colorGenerator)
    {
        Vector2[] uv = mesh.uv;

        for (int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++)
            {
                // index of vertices
                int i = x + y * resolution;
                Vector2 percent = new Vector2(x, y) / (resolution - 1); // why -1
                Vector3 pointOnUnitCube = localUp + (percent.x - 0.5f) * 2 * axisA + (percent.y - 0.5f) * 2 * axisB;
                // get vertices same distance to center of cube
                Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;
                //store uvs , x coords are used for biomes, y for water
                uv[i].x = colorGenerator.BiomePercentFromtPoint(pointOnUnitSphere);
            }
        }
        mesh.uv = uv;
    }
}
