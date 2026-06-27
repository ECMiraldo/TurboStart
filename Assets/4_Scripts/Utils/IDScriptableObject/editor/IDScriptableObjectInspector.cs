using UnityEngine;
using System;
using UnityEditor;

[CustomEditor(typeof(IDScriptableObject),true)]
public class IDScriptableObjectInspector : Editor
{
    SerializedProperty id;

    private void OnEnable()
    {
        id = serializedObject.FindProperty("_id");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.LabelField("id", id.stringValue);

        if (GUILayout.Button("Copy", GUILayout.Width(50)))
        {
            EditorGUIUtility.systemCopyBuffer = id.stringValue;
        }
        if (GUILayout.Button("Roll")) {
            Roll();
        }
        
        

        EditorGUILayout.EndHorizontal();

        DrawPropertiesExcluding(serializedObject, "_id");
        serializedObject.ApplyModifiedProperties();
    }

    private void Roll()
    {
        id.stringValue = Guid.NewGuid().ToString();
        EditorUtility.SetDirty(serializedObject.targetObject); // Mark the object dirty so Unity saves the new ID
        AssetDatabase.SaveAssets();   // Ensure it's written to disk
    }
    
}
