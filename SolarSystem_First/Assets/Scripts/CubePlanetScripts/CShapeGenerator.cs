using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CShapeGenerator
{
    CShapeSettings settings;                
    CNoiseFilter[] noiseFilters;            

    public CShapeGenerator(CShapeSettings settings)
    {
        this.settings = settings;
        noiseFilters = new CNoiseFilter[settings.noiseLayers.Length];

        for (int i = 0; i < noiseFilters.Length; i++)
        {
            noiseFilters[i] = new CNoiseFilter(settings.noiseLayers[i].noiseSettings);
        }
    }


    public Vector3 CalculatePointOnPlanet(Vector3 pointOnUnitSphere)
    {
        //float noise = Mathf.PerlinNoise(pointOnUnitSphere.x * settings.noiseRroughness + settings.noiseCentre.x, pointOnUnitSphere.x * settings.noiseRroughness + settings.noiseCentre.y);
        //noise = noise *settings.noiseStrengh;

        float firstLayerValyue = 0;         // used to set noise Value of first layer to the rest
        float noise = 0;

        if (noiseFilters.Length > 0)
        {
            // set value to first Layer value
            firstLayerValyue = noiseFilters[0].Evaluate(pointOnUnitSphere);
            if(settings.noiseLayers[0].enabled)
            {
                noise = firstLayerValyue;
            }
        }


        for (int i = 1; i < noiseFilters.Length; i++)
        {
            if(settings.noiseLayers[i].enabled)
            {
                // check if firstLayer is used as mask if so, set value if not set to 1
                float mask = (settings.noiseLayers[i].useFirstLayerAsMask) ? firstLayerValyue : 1;
                noise += noiseFilters[i].Evaluate(pointOnUnitSphere) * mask;
            }
        }

        return pointOnUnitSphere * settings.planetRadius * (1+noise);
    }
}
