﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeGenerator
{
    ShapeSettings settings;                
    INoiseFilter[] noiseFilters;

    public CMinMax elavationMinMax;            // height to calculate hightest point of Planet and set Colors right

    public void UpdateSettings(ShapeSettings settings)
    {
        this.settings = settings;
        noiseFilters = new INoiseFilter[settings.noiseLayers.Length];

        for (int i = 0; i < noiseFilters.Length; i++)
        {
            noiseFilters[i] = CNoiseFilterFactory.CreateNoiseFilter(settings.noiseLayers[i].noiseSettings);
        }

        elavationMinMax = new CMinMax();
    }


    public float CalculateUnscaledElevation(Vector3 pointOnSphere)
    {
        //float noise = Mathf.PerlinNoise(pointOnUnitSphere.x * settings.noiseRroughness + settings.noiseCentre.x, pointOnUnitSphere.x * settings.noiseRroughness + settings.noiseCentre.y);
        //noise = noise *settings.noiseStrengh;

        float firstLayerValyue = 0;         // used to set noise Value of first layer to the rest
        float noise = 0;

        if (noiseFilters.Length > 0)
        {
            // set value to first Layer value
            firstLayerValyue = noiseFilters[0].Evaluate(pointOnSphere);
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
                noise += noiseFilters[i].Evaluate(pointOnSphere) * mask;
            }
        }

        //noise = settings.planetRadius * (1 + noise);
        //storing lowest and highest elevation of all vertices
        elavationMinMax.AddValue(noise);
        return noise;
    }

    /// <summary>
    /// get back correct elevation with clamp and * by PlanetRadius
    /// </summary>
    /// <param name="unscaledElevation"></param>
    /// <returns></returns>
    public float GetScaledElevation(float unscaledElevation)
    {
        float elevtaion = Mathf.Max(0, unscaledElevation);
        elevtaion = settings.planetRadius * (1 + elevtaion);
        return elevtaion;
    }
}
