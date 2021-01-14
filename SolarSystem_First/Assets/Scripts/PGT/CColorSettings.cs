using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CPlanet/ColorSettings")]
public class CColorSettings : ScriptableObject
{
    public Material planetMaterial;
    public BiomeColorSettings biomeColorSettings;
    public Gradient oceanColor;

    /// <summary>
    /// Biome settings 
    /// </summary>
    [System.Serializable]
    public class BiomeColorSettings
    {
        public Biome[] biomes;
        // use noise for the Borders of the Biomes
        public CNoiseSettings noise;
        public float noiseOffset;
        public float noiseStrength;
        [Range(0, 1)]
        public float blendStrength;             // blend NoiseBiomes in eachother 


        [System.Serializable]
        public class Biome
        {
            public Gradient gradient;           // set colors
            public Color tint;                  // farbton
            [Range(0, 1)]
            public float startHeight;           // to set poles etc
            [Range(0, 1)]
            public float tintPercent;

        }
    }
}
