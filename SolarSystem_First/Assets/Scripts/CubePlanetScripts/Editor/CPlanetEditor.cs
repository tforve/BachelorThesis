using UnityEngine;
using UnityEditor;


/// <summary>
/// To Customize planets in the Editor, created a customEditor
/// </summary>
[CustomEditor(typeof(IcoPlanet))]
public class CPlanetEditor : Editor
{
    //CPlanet planet;
    IcoPlanet planet1;


    public override void OnInspectorGUI()
    {
        using (var check = new EditorGUI.ChangeCheckScope())
        {
            base.OnInspectorGUI();
            if(check.changed)
            {
                planet1.GeneratePlanet();
            }
        }
        // generate Planet button
        if(GUILayout.Button("Generate Planet"))
        {
            planet1.GeneratePlanet();
        }

        // add all Settings to Observers
        DrawSettingsEditor(planet1.shapeSettings, planet1.OnShapeSettingsUpdated);
        DrawSettingsEditor(planet1.colorSettings, planet1.OnColorSettingsUpdate);
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
        planet1 = (IcoPlanet)target;
    }
}
