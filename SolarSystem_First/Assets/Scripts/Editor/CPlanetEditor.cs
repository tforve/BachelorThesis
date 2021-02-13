using UnityEngine;
using UnityEditor;

/// <summary>
/// To Customize planets in the Editor, created a customEditor
/// </summary>
[CustomEditor(typeof(BlueprintPlanet))]
public class CPlanetEditor : Editor
{
    BlueprintPlanet planet;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        // generate Planet button
        if(GUILayout.Button("Generate Planet"))
        {            
            if(planet.useSeed)
            {
                // change methode 
                planet.RandomizePlanetShape();
            }
            planet.GeneratePlanet();
        }

        if (GUILayout.Button("Randomize Shape"))
        {
            // randomize Shape
            planet.RandomizePlanetShape();
            // give new rnd settings to planet.Generate
            planet.GeneratePlanet();
        }        

        if (GUILayout.Button("Randomize Color"))
        {
            planet.RandomizePlanetColor();
            planet.GeneratePlanet();

        }
        if (GUILayout.Button("Randomize Both"))
        {
            planet.RandomizePlanetShape();
            planet.RandomizePlanetColor();
            planet.GeneratePlanet();
        }

        // add all Settings to Observers
        DrawSettingsEditor(planet.shapeSettings, planet.OnShapeSettingsUpdated);
        DrawSettingsEditor(planet.colorSettings, planet.OnColorSettingsUpdate);
    }

    void DrawSettingsEditor(Object settings, System.Action onSettingsUpdated)
    {
        // check if settings in GUI changed
        using (var check = new EditorGUI.ChangeCheckScope())
        {
            // draw TitleBars
            EditorGUILayout.InspectorTitlebar(true, settings);
            Editor editor = CreateEditor(settings);
            editor.OnInspectorGUI();

            if(check.changed)
            {
                // invoke if != null
                onSettingsUpdated?.Invoke();
            }
        }
    }

    private void OnEnable()
    {
        planet = (BlueprintPlanet)target;
    }
}
