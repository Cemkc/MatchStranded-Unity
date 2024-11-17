using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GridBlueprint))]
public class GridBlueprintEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Draw the default Inspector UI
        DrawDefaultInspector();

        // Add a button to the Inspector
        GridBlueprint myScriptableObject = (GridBlueprint)target;
        if (GUILayout.Button("Draw Blueprint"))
        {
            myScriptableObject.Draw();
        }
        
        if (GUILayout.Button("Initialize Blueprint - WARNING (Resets Blueprint)"))
        {
            myScriptableObject.Init();
        }

    }
}