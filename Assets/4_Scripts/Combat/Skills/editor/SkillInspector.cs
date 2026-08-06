using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SkillSO))]
public class SkillInspector : Editor
{
    SerializedProperty iconProperty;
    SerializedProperty skillNameProperty;
    SerializedProperty prefabProperty;
    SerializedProperty minFocusCostProperty;
    SerializedProperty damageTypeProperty;

    SerializedProperty onCastEffectsProperty;
    SerializedProperty onDamageEffectsProperty;

    private void OnEnable()
    {
        iconProperty = serializedObject.FindProperty("<icon>k__BackingField");
        skillNameProperty = serializedObject.FindProperty("<skillName>k__BackingField");
        prefabProperty = serializedObject.FindProperty("<prefab>k__BackingField");
        minFocusCostProperty = serializedObject.FindProperty("<minFocusCost>k__BackingField");
        damageTypeProperty = serializedObject.FindProperty("<damageType>k__BackingField");

        onCastEffectsProperty = serializedObject.FindProperty("onCastEffects");
        onDamageEffectsProperty = serializedObject.FindProperty("onDamageEffects");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(iconProperty);
        EditorGUILayout.PropertyField(skillNameProperty);
        EditorGUILayout.PropertyField(prefabProperty);
        EditorGUILayout.PropertyField(minFocusCostProperty);
        EditorGUILayout.PropertyField(damageTypeProperty);

        DrawEffectsList("On Cast Effects", onCastEffectsProperty);
        DrawEffectsList("On Damage Effects", onDamageEffectsProperty);

        serializedObject.ApplyModifiedProperties();
    }

    void DrawEffectsList(string label, SerializedProperty property)
    {
        GUILayout.Space(10);

        EditorGUILayout.LabelField(label, EditorStyles.boldLabel);

        if (GUILayout.Button($"Add {label}"))
            ShowEffectMenu(property);

        EditorGUILayout.PropertyField(property, true);
    }

    void ShowEffectMenu(SerializedProperty property)
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
                    AddEffect(property, (CombatEffect)Activator.CreateInstance(type));
                });
        }

        menu.ShowAsContext();
    }

    void AddEffect(SerializedProperty property, CombatEffect effect)
    {
        serializedObject.Update();

        property.arraySize++;

        SerializedProperty element =
            property.GetArrayElementAtIndex(property.arraySize - 1);

        element.managedReferenceValue = effect;

        serializedObject.ApplyModifiedProperties();
    }
}