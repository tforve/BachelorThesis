 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CRidgidNoiseFilter : INoiseFilter
{
    CNoiseSettings.StdNoiseSettings settings;
    Noise noise = new Noise();

    public CRidgidNoiseFilter(CNoiseSettings.StdNoiseSettings settings)
    {
        this.settings = settings;
    }

    // calculate noise by using the Noise class
    public float Evaluate(Vector3 point)
    {
        float noiseValue = 0;
        // setting frequency and amplitude to define layers of noise
        float frequency = settings.baseRoughness;
        float amplitude = 1;

        for (int i = 0; i < settings.numberOfLayers; i++)
        {
            // get inverted absolut value of noise for sharp mountains
            float v = 1 - Mathf.Abs(noise.Evaluate(point * frequency + settings.centre));

            noiseValue += v * amplitude;
            frequency *= settings.roughness;    
            amplitude *= settings.persistence;  
        }
        noiseValue = noiseValue - settings.seaLevel;
        return noiseValue * settings.strength;
    }
}
