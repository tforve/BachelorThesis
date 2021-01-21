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
        public int seed = 0;

        [Range(1, 4)]
        public int numberOfLayers = 1;              // how many different noise layers to lay over each other
        public float strength = 1.0f;               // strength of the noise
        public float baseRoughness = 1.0f;
        public float roughness = 2.0f;
        public float persistence = 0.5f;            // to half the amplitude with each layer
        public Vector3 centre;                      // to move around on the noisemap
        [Tooltip("Sealevel")]
        public float seaLevel;                      // min radius to get flat shapes too - kind of a basic sealevel

        // ----- saves
        float tmpStr;
        float tmpbRough;
        float tmpRough;
        float tmpPers;
        Vector3 tmpCentre;
        float tmpSeaLvl;

        internal void RandomValue(int multiplier)
        {
            UnityEngine.Random.InitState(seed);
            SaveValues();

            strength = UnityEngine.Random.Range(0.01f * multiplier, 0.02f * multiplier);
            baseRoughness = UnityEngine.Random.Range(1f, 1.5f);
            roughness = UnityEngine.Random.Range(2.5f, 3.8f); ;
            persistence = 0.5f;
            centre = new Vector3(UnityEngine.Random.Range(0f, 10f), UnityEngine.Random.Range(0f, 10f), UnityEngine.Random.Range(0f, 10f));
            seaLevel = UnityEngine.Random.Range(0.45f, 1.5f); ;
        }

        internal void SaveValues()
        {
            tmpStr = strength;
            tmpbRough = baseRoughness;
            tmpRough = roughness;
            tmpPers = persistence;
            tmpCentre = centre;
            tmpSeaLvl = seaLevel;
        }

        internal void ResetValues()
        {
            Debug.Log("Old settings restored");
            strength = tmpStr;
            baseRoughness = tmpbRough;
            roughness = tmpRough;
            persistence = tmpPers;
            centre = tmpCentre;
            seaLevel = tmpSeaLvl;
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
