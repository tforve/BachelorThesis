using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CNoiseSettings
{
    [Range(1, 6)]
    public int numberOfLayers = 1;              // how many different noise layers to lay over each other

    public float strength = 1.0f;               // strength of the noise
    public float baseRoughness = 1.0f;          
    public float roughness = 2.0f;              
    public float persistence = 0.5f;            // to half the amplitude with each layer
    public Vector3 centre;                      // to move around on the noisemap
    [Tooltip("Sealevel")]
    public float minValue;                      // min radius to get flat shapes too - kind of a basic sealevel
}
