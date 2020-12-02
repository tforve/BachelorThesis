using UnityEngine;
using UnityEditor;


/// <summary>
/// To Customize planets in the Editor, created a customEditor
/// </summary>
[CustomEditor(typeof(CPlanet))]
public class CPlanetEditor : Editor
{
    CPlanet planet;

    public override void OnInspectorGUI()
    {
        using (var check = new EditorGUI.ChangeCheckScope())
        {
            base.OnInspectorGUI();
            if(check.changed)
            {
                planet.GeneratePlanet();
            }
        }
        // generate Planet button
        if(GUILayout.Button("Generate Planet"))
        {
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
        planet = (CPlanet)target;
    }
}
