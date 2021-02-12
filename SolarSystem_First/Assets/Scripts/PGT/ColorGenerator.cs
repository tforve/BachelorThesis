using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorGenerator
{
    private ColorSettings settings;
    private Texture2D texture;
    private const int textureResolution = 50;
    // Biomes
    private INoiseFilter biomeNoiseFilter;

    public void UpdateElevation(Elevation elevation)
    {
        settings.planetMaterial.SetVector("_elevation", new Vector4(elevation.minElevation, elevation.maxElevation));
    }

    /// <summary>
    /// calculate and set the Biomes strength, height and how much they blend in each other. 
    /// 0 for Southpole, 1 for Northpole, or first and last biome
    /// </summary>
    public float BiomePercentFromtPoint(Vector3 pointOnSphere)
    {
        // set between 0, 1
        float heightPercent = (pointOnSphere.y + 1) / 2.0f;
        // use noise on heightPercent to disturb the boundries
        heightPercent += (biomeNoiseFilter.Evaluate(pointOnSphere) - settings.biomeColorSettings.noiseOffset) * settings.biomeColorSettings.noiseStrength;

        float biomeLevel = 0;
        int numberOfBiomes = settings.biomeColorSettings.biomes.Length;
        float blendStrength = settings.biomeColorSettings.blendStrength / 2.0f; 

        for (int i = 0; i < numberOfBiomes; i++)
        {
            float distanceFromPole = heightPercent - settings.biomeColorSettings.biomes[i].startHeight;
            // get if the distanceFromPole is within the range of the blendRange
            float weight = Mathf.InverseLerp(-blendStrength, blendStrength, distanceFromPole);
            // clamp index so he didnt get too large
            biomeLevel *= (1 - weight);
            biomeLevel += i * weight;
        }
        // return in between 0 and 1. use Max to not divice by 0 - its 0 instead
        biomeLevel = biomeLevel / Mathf.Max(1, numberOfBiomes - 1);
        return biomeLevel;
    }

    /// <summary>
    /// Update Colorsettings if texture is null or the number of biomes change
    /// </summary>
    public void UpdateSettings(ColorSettings settings)
    {
        this.settings = settings;

        if (texture == null || texture.height != settings.biomeColorSettings.biomes.Length)
        {
            texture = new Texture2D(textureResolution * 2, settings.biomeColorSettings.biomes.Length, TextureFormat.RGBA32, false);
        }
        // set biome Noise
        biomeNoiseFilter = CNoiseFilterFactory.CreateNoiseFilter(settings.biomeColorSettings.noise);
    }

    public void UpdateColors()
    {
        Color[] colors = new Color[texture.width * texture.height];

        int colorIndex = 0;

        foreach (var b in settings.biomeColorSettings.biomes)
        {
            for (int i = 0; i < textureResolution * 2; i++)
            {
                Color gradientColor;
                // ocean Gradient
                if (i < textureResolution)
                {
                    gradientColor = settings.biomeColorSettings.oceanColor.Evaluate(i / (textureResolution - 1.0f));
                }
                // biome gradients
                else
                {
                    gradientColor = b.gradient.Evaluate((i - textureResolution) / (textureResolution - 1.0f));
                }
                Color tintColor = b.tint;
                // if no tintpercent just based on gradientColor
                colors[colorIndex] = gradientColor * (1 - b.tintPercent) + tintColor * b.tintPercent;            
                colorIndex++;
            }
        }

        texture.SetPixels(colors);
        texture.Apply();
        settings.planetMaterial.SetTexture("_texture", texture);
    }
}
