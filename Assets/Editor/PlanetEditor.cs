using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(Planet))]
public class PlanetEditor : Editor
{

    private Planet planet;
    private Editor shapeEditor;
    private Editor colorEditor;

    private void OnEnable()
    {
        planet = (Planet)target;




    }

    public override void OnInspectorGUI()
    {

        using var check = new EditorGUI.ChangeCheckScope();

        base.OnInspectorGUI();

        if (check.changed) planet.GeneratePlanet();

        if (GUILayout.Button("Generate Planet")) planet.GeneratePlanet();

        DrawSettingsEditor(planet.ShapeSettings, planet.OnShapeSettingsUpdated, ref planet.ShapeSettingsFoldout, ref shapeEditor);
        DrawSettingsEditor(planet.ColorSettings, planet.OnColorSettingsUpdated, ref planet.ColorSettingsFoldout, ref colorEditor);

    }

    private void DrawSettingsEditor(Object settings, System.Action onSettingsUpdated, ref bool foldout, ref Editor editor)
    {

        if (!settings) return;

        foldout = EditorGUILayout.InspectorTitlebar(foldout, settings);

        if (!foldout) return;

        using var check = new EditorGUI.ChangeCheckScope();

        CreateCachedEditor(settings, null, ref editor);
        editor.OnInspectorGUI();

        if (check.changed && onSettingsUpdated != null)
        {
            onSettingsUpdated();
        }




    }
}
