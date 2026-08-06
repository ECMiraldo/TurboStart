using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ProjectileSO))]
public class ProjectileInspector : Editor
{
    SerializedProperty projectilePrefabProperty;
    SerializedProperty speedProperty;
    SerializedProperty effectsProperty;

    private void OnEnable()
    {
        projectilePrefabProperty = serializedObject.FindProperty("<projectilePrefab>k__BackingField");
        speedProperty = serializedObject.FindProperty("<speed>k__BackingField");
        
        effectsProperty = serializedObject.FindProperty("onDamageEffects");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(projectilePrefabProperty);
        EditorGUILayout.PropertyField(speedProperty);

        GUILayout.Space(5);

        if (GUILayout.Button("Add Effect"))
            ShowEffectMenu();

        EditorGUILayout.PropertyField(effectsProperty, true);

        serializedObject.ApplyModifiedProperties();
    }

    void ShowEffectMenu()
    {
        GenericMenu menu = new();

        foreach (Type type in TypeCache.GetTypesDerivedFrom<CombatEffect>())
        {
            if (type.IsAbstract)
                continue;

            menu.AddItem(
                new GUIContent(ObjectNames.NicifyVariableName(type.Name)),
                false,
                () =>
                {
                    AddEffect((CombatEffect)Activator.CreateInstance(type));
                });
        }

        menu.ShowAsContext();
    }

    void AddEffect(CombatEffect effect)
    {
        serializedObject.Update();

        effectsProperty.arraySize++;

        SerializedProperty element =
            effectsProperty.GetArrayElementAtIndex(effectsProperty.arraySize - 1);

        element.managedReferenceValue = effect;

        serializedObject.ApplyModifiedProperties();
    }
}