using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Planet/ColorSettings")]
public class ColorSettings : ScriptableObject
{
    public Material planetMaterial;
    public BiomeColorSettings biomeColorSettings;

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
        [Range(0.001f, 1)]
        public float blendStrength;             // how far the NoiseBiomes blend in eachother 
        public Gradient oceanColor;

        internal void RandomOceanColor()
        {
            Gradient tmpGradient = new Gradient();
            GradientColorKey[] colorKey = new GradientColorKey[2];
            GradientAlphaKey[] alphaKey = new GradientAlphaKey[1];

            for (int i = 0; i < colorKey.Length; i++)
            {
                colorKey[i].color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
            }

            colorKey[0].time = 0.0f;
            colorKey[1].time = 1.0f;
            alphaKey[0].alpha = 1.0f;

            tmpGradient.SetKeys(colorKey, alphaKey);
            oceanColor = tmpGradient;

            // rnd noise
            noiseOffset = Random.Range(0.3f, 0.6f);
            noiseStrength = Random.Range(0.2f, 0.5f);
        }

        [System.Serializable]
        public class Biome
        {
            public Gradient gradient;           // main color gradient
            public Color tint;                  // laying on top of the gradient
            [Range(0, 1)]
            public float startHeight;           
            [Range(0, 1)]
            public float tintPercent;

            internal void RandomValue()
            {
                // randomize Biome Gradient
                Gradient tmpGradient = new Gradient();
                GradientColorKey[] colorKey = new GradientColorKey[5];
                GradientAlphaKey[] alphaKey = new GradientAlphaKey[1];

                for (int i = 0; i < colorKey.Length; i++)
                {
                    colorKey[i].color = Random.ColorHSV();
                    colorKey[i].time = Random.Range(0.0f, 1.0f);
                }
                alphaKey[0].alpha = 1.0f;

                tmpGradient.SetKeys(colorKey, alphaKey);
                gradient = tmpGradient;

                // randomize tint
                tint = new Color(Random.Range(0.3f, 1.0f), Random.Range(0.3f, 1.0f), Random.Range(0.3f, 1.0f));
                tintPercent = Random.Range(0.0f, 0.5f);
            }
        }
    }
}
