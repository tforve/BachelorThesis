using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSimpleNoiseFilter : CINoiseFilter
{
    CNoiseSettings.StdNoiseSettings settings;
    Noise noise = new Noise();

    public CSimpleNoiseFilter(CNoiseSettings.StdNoiseSettings settings)
    {
        this.settings = settings;
    }

    // calculate noise by using the Noise class
    public float Evaluate(Vector3 point)
    {
        // final noiseValue
        float noiseValue = 0;
        // setting frequency and amplitude to define layers of noise
        float frequency = settings.baseRoughness;
        float amplitude = 1;

        for (int i = 0; i < settings.numberOfLayers; i++)
        {
            float v = noise.Evaluate(point * frequency + settings.centre);
            // change range from -1/1 to 0/1
            noiseValue += (v + 1) * 0.5f * amplitude;
            frequency *= settings.roughness;    // roughness > 1 = frequency will increase with each layer
            amplitude *= settings.persistence;  // persistence < 1 = amplitude will decrease with each layer
        }
        noiseValue = Mathf.Max(0, noiseValue - settings.minValue);
        return noiseValue * settings.strength;
    }
}
