using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Planet/ShapeSettings")]
public class ShapeSettings : ScriptableObject
{
    public float planetRadius = 1.0f;
    public CNoiseLayer[] noiseLayers;

    /// <summary>
    /// To get multiple Layers of Noisefunctions we need more then one.
    /// needed to create Mountains etc. for example
    /// </summary>
    [System.Serializable]
    public class CNoiseLayer
    {
        public bool enabled = true;                     // enable layer at all
        public bool useFirstLayerAsMask = true;        // make the firstLayer as mask tho other Terrain only appear on this first layer
        public CNoiseSettings noiseSettings;    
    }
}
