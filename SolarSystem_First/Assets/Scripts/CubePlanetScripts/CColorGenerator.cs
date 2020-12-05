using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CColorGenerator
{
    CColorSettings settings;
    Texture2D texture;
    const int textureResolution = 50;

    public void UpdateSettings(CColorSettings settings)
    {
        this.settings = settings;
        if(texture == null)
        {
            texture = new Texture2D(textureResolution, 1);
        }
    }

    public void UpdateElevation (CMinMax elevationMinMax)
    {
        settings.planetMaterial.SetVector("_elevationMinMax", new Vector4(elevationMinMax.Min, elevationMinMax.Max));
    }

    public void UpdateColors()
    {
        Color[] colors = new Color[textureResolution];
        for (int i = 0; i < textureResolution; i++)
        {
            colors[i] = settings.planetGradient.Evaluate(i / (textureResolution - 1.0f));
        }
        texture.SetPixels(colors);
        texture.Apply();
        settings.planetMaterial.SetTexture("_texture", texture);
    }
}
