using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CColorGenerator
{
    CColorSettings settings;
    Texture2D texture;
    const int textureResolution = 50;
    // Biomes
    INoiseFilter biomeNoiseFilter;

    /// <summary>
    /// Update Colorsettings if texture is null or the number of biomes change
    /// </summary>
    /// <param name="settings"></param>
    public void UpdateSettings(CColorSettings settings)
    {
        this.settings = settings;

        if (texture == null || texture.height != settings.biomeColorSettings.biomes.Length)
        {
            texture = new Texture2D(textureResolution * 2, settings.biomeColorSettings.biomes.Length, TextureFormat.RGBA32, false);
            // set different Texture to water create it here and change some stuff
        }
        // set biome Noise
        biomeNoiseFilter = CNoiseFilterFactory.CreateNoiseFilter(settings.biomeColorSettings.noise);
    }

    public void UpdateElevation(CMinMax elevationMinMax)
    {
        settings.planetMaterial.SetVector("_elevationMinMax", new Vector4(elevationMinMax.Min, elevationMinMax.Max));
    }


    /// <summary>
    /// calculate and set the Biomes, strength, height and
    /// 0 for Southpole, 1 for northpole, 
    /// </summary>
    /// <param name="pointOnUnitSphere"></param>
    /// <returns></returns>
    public float BiomePercentFromtPoint(Vector3 pointOnUnitSphere)
    {
        // set between 0, 1
        float heightPercent = (pointOnUnitSphere.y + 1) / 2.0f;
        // give controll for the noise by using the offset and multipling the strength
        heightPercent += (biomeNoiseFilter.Evaluate(pointOnUnitSphere) - settings.biomeColorSettings.noiseOffset) * settings.biomeColorSettings.noiseStrength;

        float biomeIndex = 0;
        int numberOfBiomes = settings.biomeColorSettings.biomes.Length;
        float blendRange = settings.biomeColorSettings.blendStrength / 2.0f + 0.001f;

        for (int i = 0; i < numberOfBiomes; i++)
        {
            //// return 0 if in first biome
            //// between for other biomes
            //// return 1 if in last biome
            //if (settings.biomeColorSettings.biomes[i].startHeight < heightPercent)
            //{
            //    biomeIndex = i;
            //}
            //else { break; }

            float distanceFromPole = heightPercent - settings.biomeColorSettings.biomes[i].startHeight;
            float weight = Mathf.InverseLerp(-blendRange, blendRange, distanceFromPole);
            // clamp index 
            biomeIndex *= (1 - weight);
            biomeIndex += i * weight;
        }
        // return in in between 0 and 1 - if its 0 its 1 instead
        biomeIndex = biomeIndex / Mathf.Max(1, numberOfBiomes - 1);
        return biomeIndex;
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
                    gradientColor = settings.oceanColor.Evaluate(i / (textureResolution - 1.0f));
                }
                // else biome gradients
                else
                {
                    gradientColor = b.gradient.Evaluate((i - textureResolution) / (textureResolution - 1.0f));
                }
                Color tintColor = b.tint;
                colors[colorIndex] = gradientColor * (1 - b.tintPercent) + tintColor * b.tintPercent;            // -1?
                colorIndex++;
            }
        }

        texture.SetPixels(colors);
        texture.Apply();
        settings.planetMaterial.SetTexture("_texture", texture);
    }
}
