using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DialogueSO))]
public class DialogueInspector : Editor
{
    SerializedProperty dialogueNameProperty;
    SerializedProperty storyProperty;
    SerializedProperty dialogueImageProperty;
    SerializedProperty effectsProperty;

    private void OnEnable()
    {
        dialogueNameProperty = serializedObject.FindProperty("dialogueName");
        storyProperty = serializedObject.FindProperty("story");
        dialogueImageProperty = serializedObject.FindProperty("dialogueImage");
        effectsProperty = serializedObject.FindProperty("dialogueEffects");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(dialogueNameProperty);
        EditorGUILayout.PropertyField(storyProperty);
        EditorGUILayout.PropertyField(dialogueImageProperty);

        if (GUILayout.Button("Add Effect"))
        {
            ShowEffectMenu();
        }

        EditorGUILayout.PropertyField(effectsProperty, true);
        
        serializedObject.ApplyModifiedProperties();
    }

    void ShowEffectMenu()
    {
        GenericMenu menu = new GenericMenu();
        var types = TypeCache.GetTypesDerivedFrom<DialogueEffect>();

        foreach (var type in types)
        {
            if (type.IsAbstract)
                continue;

            string name = ObjectNames.NicifyVariableName(type.Name);

            menu.AddItem(new GUIContent(name), false, () =>
            {
                var effect = (DialogueEffect)Activator.CreateInstance(type);
                AddEffect(effect);
            });
        }
        menu.ShowAsContext();
    }

    void AddEffect(DialogueEffect effect)
    {
        serializedObject.Update();

        effectsProperty.arraySize++;
        SerializedProperty newElement =
            effectsProperty.GetArrayElementAtIndex(effectsProperty.arraySize - 1);

        newElement.managedReferenceValue = effect;

        serializedObject.ApplyModifiedProperties();
    }
}