using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CHillyNoiseFilter : INoiseFilter
{
    CNoiseSettings.StdNoiseSettings settings;
    Noise noise = new Noise();

    public CHillyNoiseFilter(CNoiseSettings.StdNoiseSettings settings)
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
        // make it more details based on layers. heigher layers have higher weight -------- NO NEED MAYBE

        for (int i = 0; i < settings.numberOfLayers; i++)
        {
            // get inverted absolut value of noise for nice spikes
            float v = 1 + Mathf.Abs(noise.Evaluate(point * frequency + settings.centre));
            v = (v * v)-1;

            noiseValue += v * amplitude;
            frequency *= settings.roughness;    // roughness > 1 = frequency will increase with each layer
            amplitude *= settings.persistence;  // persistence < 1 = amplitude will decrease with each layer
        }
        noiseValue = noiseValue - settings.seaLevel;
        return noiseValue * settings.strength;
    }
}
