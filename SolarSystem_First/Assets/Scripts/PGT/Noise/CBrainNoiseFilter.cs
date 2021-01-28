using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CBrainNoiseFilter : INoiseFilter
{
    CNoiseSettings.StdNoiseSettings settings;
    Noise noise = new Noise();

    public CBrainNoiseFilter(CNoiseSettings.StdNoiseSettings settings)
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
            // get inverted absolut value of noise for hilly mountains
            float v = 0.5f - Mathf.Abs(noise.Evaluate(point * frequency + settings.centre));
            v = v * v;

            noiseValue += v * amplitude;
            frequency *= settings.roughness;
            amplitude *= settings.persistence;

        }
        noiseValue = noiseValue - settings.seaLevel;
        return noiseValue * settings.strength;
    }
}
