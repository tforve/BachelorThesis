using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// By creating every Face on its owne we can later easily change the LOD 
/// </summary>
public class Face
{
    private ShapeGenerator shapeGenerator;     
    private Mesh mesh;
    private int resolution;                    
    private Vector3 normalVector;               // facing up/ used as local up
    private Vector3 axisX, axisY;               // other 2 dir vectors to build cartesian system

    public Face(ShapeGenerator shapeGenerator, Mesh mesh, int resolution, Vector3 normalVec)
    {
        this.shapeGenerator = shapeGenerator;
        this.mesh = mesh;
        this.resolution = resolution;
        this.normalVector = normalVec;

        //flip normalVector aside and then use crossproduct to get third vector
        axisX = new Vector3(normalVec.y, normalVec.z, normalVec.x); 
        axisY = Vector3.Cross(normalVec, axisX);
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
                // calculate how complete every loop/face is 
                Vector2 percent = new Vector2(x, y) / (resolution-1);
                Vector3 pointOnCube = normalVector + (percent.x - 0.5f) * 2 * axisX + (percent.y - 0.5f) * 2 * axisY;
                // get vertices same distance to center of cube
                Vector3 pointOnSphere = pointOnCube.normalized;
                // calculate elevation and set them to the vertices
                float unscaledElevation = shapeGenerator.CalculateUnscaledElevation(pointOnSphere);
                vertices[i] = pointOnSphere * shapeGenerator.GetScaledElevation(unscaledElevation);
                // set UVs
                uv[i].y = unscaledElevation;

                // check if index is not outside the face on rightside or bottom
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
        // store UVs
        Vector2[] uv = mesh.uv;

        for (int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++)
            {
                // index of vertices
                int i = x + y * resolution;
                Vector2 percent = new Vector2(x, y) / (resolution - 1);
                Vector3 pointOnUnitCube = normalVector + (percent.x - 0.5f) * 2 * axisX + (percent.y - 0.5f) * 2 * axisY;
                Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;
                //store uvs , x coords are used for biomes, y for water
                uv[i].x = colorGenerator.BiomePercentFromtPoint(pointOnUnitSphere);
            }
        }
        mesh.uv = uv;
    }
}
