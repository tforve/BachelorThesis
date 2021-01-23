using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CNoiseSettings
{
    public enum FilterType { Simple, Ridgid, Hilly, Brain };   // types of Filters to specify how to calculate the Noise
    public FilterType filterType;

    public StdNoiseSettings stdNoiseSettings;
    public RidgidNoiseSettings ridgidNoiseSettings;


    // Settings used by all Filters
    [System.Serializable]
    public class StdNoiseSettings
    {

        [Range(1, 4)]
        public int numberOfLayers = 1;              // how many different noise layers to lay over each other
        public float strength = 1.0f;               // strength of the noise
        public float baseRoughness = 1.0f;
        public float roughness = 2.0f;
        public float persistence = 0.5f;            // to half the amplitude with each layer
        public Vector3 centre;                      // to move around on the noisemap
        [Tooltip("Sealevel")]
        public float seaLevel;                      // min radius to get flat shapes too - kind of a basic sealevel

        internal void RandomValue(int multiplier)
        {
            //UnityEngine.Random.InitState(seed);

            strength = UnityEngine.Random.Range(0.01f * multiplier, 0.02f * multiplier);
            baseRoughness = UnityEngine.Random.Range(1f, 1.5f);
            roughness = UnityEngine.Random.Range(2.5f, 3.8f); ;
            persistence = 0.5f;
            centre = new Vector3(UnityEngine.Random.Range(0f, 10f), UnityEngine.Random.Range(0f, 10f), UnityEngine.Random.Range(0f, 10f));
            seaLevel = UnityEngine.Random.Range(0.45f, 1.5f); ;
        }
    }

    // additional settings for ridgid Noise
    [System.Serializable]
    public class RidgidNoiseSettings : StdNoiseSettings
    {
        public float weightMultiplier = 0.5f;
    }

    // add more addition Settings
}
