using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CShapeGenerator
{
    CShapeSettings settings;

    public CShapeGenerator(CShapeSettings settings)
    {
        this.settings = settings;
    }

    public Vector3 CalculatePointOnPlanet(Vector3 pointOnUnitSphere)
    {
        return pointOnUnitSphere * settings.planetRadius;
    }
}
