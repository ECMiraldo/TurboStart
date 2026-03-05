using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class AutoplayToggle : MonoBehaviour
{
    private Toggle toggle;

    private void Awake()
    {
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(OnToggle);
    }

    private void OnEnable()
    {
        CombatSessionManager.onAutoplayToggled += OnAutoplayToggled;
    }

    private void OnDisable()
    {
        CombatSessionManager.onAutoplayToggled += OnAutoplayToggled;
    }

    public void OnToggle(bool val) => CombatSessionManager.Instance.SetAutoplay(val);

    private void OnAutoplayToggled(bool val) => toggle.isOn = val;

}
