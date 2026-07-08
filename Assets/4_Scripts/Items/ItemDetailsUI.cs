using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDetailsUI : UIPanelController
{
    public static ItemDetailsUI Instance { get; private set; }

    [Header("References")]
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI statsText;
    [SerializeField] private TextMeshProUGUI effectsText;

    private float lastShownTime;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Update()
    {
        if (!IsOpen)
            return;

        if (Time.unscaledTime - lastShownTime < 0.15f)
            return;

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
        {
            if (!IsPointerOverPanel())
            {
                Close();
            }
        }
    }

    public void Show(Item item, Vector2? screenPosition = null)
    {
        if (item == null)
            return;

        titleText.text = item.template<ItemSO>().Name;
        iconImage.sprite = item.Sprite;
        iconImage.color = item.Sprite != null ? Color.white : new Color(1, 1, 1, 0);

        descriptionText.text = item.GetFullDescription();
        PopulateStats(item);
        PopulateEffects(item);

        if (screenPosition.HasValue)
        {
            PositionDetailsUI(screenPosition.Value);
        }

        lastShownTime = Time.unscaledTime;
        Open();
    }

    public override void Close()
    {
        base.Close();
        ClearContent();
    }

    private bool IsPointerOverPanel()
    {
        RectTransform rect = GetComponent<RectTransform>();
        return rect != null && RectTransformUtility.RectangleContainsScreenPoint(rect, Input.mousePosition, null);
    }

    private void ClearContent()
    {
        titleText.text = string.Empty;
        descriptionText.text = string.Empty;
        statsText.text = string.Empty;
        effectsText.text = string.Empty;

        if (iconImage != null)
        {
            iconImage.sprite = null;
            iconImage.color = new Color(1, 1, 1, 0);
        }
    }

    private void PopulateStats(Item item)
    {
        if (item is Equipment equipment)
        {
            EquipmentSO so = equipment.template<EquipmentSO>();
            var lines = new List<string>
            {
                $"Slot: {so.Slot}",
                $"Required Level: {so.level}",
            };

            if (equipment is Weapon weapon)
            {
                WeaponSO weaponSO = weapon.template<WeaponSO>();
                lines.Add($"Damage: {weapon.minAttackValue} - {weapon.maxAttackValue}");
                lines.Add($"Attack Speed: {1 + weaponSO.AttackSpeed:f2}");
                lines.Add($"Weapon Type: {weaponSO.WeaponType}");
            }

            if (equipment.modifiers != null && equipment.modifiers.Count > 0)
            {
                lines.Add(string.Empty);
                lines.Add("Stats:");
                lines.AddRange(equipment.modifiers.Select(mod => $"• {mod.stat}: {mod.value}"));
            }

            statsText.text = string.Join("\n", lines);
        }
        else
        {
            statsText.text = string.Empty;
        }
    }

    private void PopulateEffects(Item item)
    {
        if (item is Equipment equipment && equipment.effects != null && equipment.effects.Count > 0)
        {
            var lines = new List<string> { "Effects:" };
            foreach (ItemEffect effect in equipment.effects)
            {
                lines.Add($"• {GetEffectDescription(effect)}");
            }

            effectsText.text = string.Join("\n", lines);
        }
        else
        {
            effectsText.text = string.Empty;
        }
    }

    private string GetEffectDescription(ItemEffect effect)
    {
        if (effect == null)
            return "Unknown effect";

        string conditionName = effect.condition != null ? PrettyName(effect.condition.GetType().Name.Replace("Condition", "")) : "Always";
        string consequenceName = effect.consequence != null ? PrettyName(effect.consequence.GetType().Name.Replace("Consequence", "")) : "No effect";

        string conditionDetail = GetSerializedFieldValues(effect.condition);
        string consequenceDetail = GetSerializedFieldValues(effect.consequence);

        string description = $"{conditionName} → {consequenceName}";
        if (!string.IsNullOrEmpty(conditionDetail) || !string.IsNullOrEmpty(consequenceDetail))
        {
            var details = new List<string>();
            if (!string.IsNullOrEmpty(conditionDetail)) details.Add(conditionDetail);
            if (!string.IsNullOrEmpty(consequenceDetail)) details.Add(consequenceDetail);
            description += $" ({string.Join("; ", details)})";
        }

        return description;
    }

    private string GetSerializedFieldValues(object target)
    {
        if (target == null)
            return string.Empty;

        FieldInfo[] fields = target.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        var fieldPairs = new List<string>();

        foreach (FieldInfo field in fields)
        {
            if (field.Name == "m_Script")
                continue;

            object value = field.GetValue(target);
            if (value == null)
                continue;

            if (value is string stringValue && string.IsNullOrEmpty(stringValue))
                continue;

            fieldPairs.Add($"{PrettyName(field.Name)}: {value}");
        }

        return string.Join(", ", fieldPairs);
    }

    private string PrettyName(string rawName)
    {
        if (string.IsNullOrEmpty(rawName))
            return rawName;

        rawName = rawName.Replace("m_", string.Empty);
        rawName = rawName.Replace("n", "Count");
        rawName = rawName.Replace("Hits", "Hits");

        return string.Concat(rawName.Select((c, i) => i > 0 && char.IsUpper(c) ? " " + c : c.ToString()));
    }

    private void PositionDetailsUI(Vector2 screenPosition)
    {
        RectTransform rect = GetComponent<RectTransform>();
        if (rect == null)
            return;

        rect.pivot = new Vector2(0, 0);
        rect.anchorMin = new Vector2(0, 0);
        rect.anchorMax = new Vector2(0, 0);
        rect.position = screenPosition;
    }
}
