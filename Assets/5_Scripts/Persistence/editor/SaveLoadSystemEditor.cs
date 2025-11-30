using Persistence;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(SaveLoadSystem))]
public class SaveLoadSystemEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Draw the default inspector first
        DrawDefaultInspector();

        GUILayout.Space(10);

        SaveLoadSystem saveLoadSystem = (SaveLoadSystem)target;

        if (GUILayout.Button("Delete Local Save"))
        {
            if (EditorUtility.DisplayDialog(
                "Delete Local Save",
                "Are you sure you want to delete the local save file?",
                "Yes", "Cancel"))
            {
                saveLoadSystem.Delete();
                Debug.Log("<color=red>Local save deleted.</color>");
            }
        }

        if (GUILayout.Button("Save Now"))
        {
            saveLoadSystem.SaveProfile();
            Debug.Log("<color=green>Saved manually.</color>");
        }

        if (GUILayout.Button("Load Save"))
        {
            try
            {
                saveLoadSystem.LoadProfile();
                Debug.Log("<color=cyan>Save Loaded.</color>");
            }
            catch
            {
                Debug.LogWarning("No save file to load.");
            }
        }
    }
}
