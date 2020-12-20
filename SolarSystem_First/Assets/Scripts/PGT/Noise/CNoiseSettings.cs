using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CNoiseSettings
{
    public enum FilterType { Simple, Ridgid, Hilly, Brain};   // types of Filters to specify how to calculate the Noise
    public FilterType filterType;

    public StdNoiseSettings stdNoiseSettings;
    public RidgidNoiseSettings ridgidNoiseSettings;    

    // Settings used by all Filters
    [System.Serializable]
    public class StdNoiseSettings
    {
        [Range(1, 6)]
        public int numberOfLayers = 1;              // how many different noise layers to lay over each other
        public float strength = 1.0f;               // strength of the noise
        public float baseRoughness = 1.0f;
        public float roughness = 2.0f;
        public float persistence = 0.5f;            // to half the amplitude with each layer
        public Vector3 centre;                      // to move around on the noisemap
        [Tooltip("Sealevel")]
        public float seaLevel;                      // min radius to get flat shapes too - kind of a basic sealevel
    }    

    // additional settings for ridgid Noise
    [System.Serializable]
    public class RidgidNoiseSettings : StdNoiseSettings
    {
        public float weightMultiplier = 0.5f;
    }

    // add more addition Settings
}
