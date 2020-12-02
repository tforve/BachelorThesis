using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CShapeGenerator
{
    CShapeSettings settings;
    CNoiseFilter noiseFilter;
    public CShapeGenerator(CShapeSettings settings)
    {
        this.settings = settings;
        noiseFilter = new CNoiseFilter(settings.noiseSettings);
    }

    public Vector3 CalculatePointOnPlanet(Vector3 pointOnUnitSphere)
    {
        //float noise = Mathf.PerlinNoise(pointOnUnitSphere.x * settings.noiseRroughness + settings.noiseCentre.x, pointOnUnitSphere.x * settings.noiseRroughness + settings.noiseCentre.y);
        //noise = noise *settings.noiseStrengh;
        float noise = noiseFilter.Evaluate(pointOnUnitSphere);
        return pointOnUnitSphere * settings.planetRadius * (1+noise);
    }
}
