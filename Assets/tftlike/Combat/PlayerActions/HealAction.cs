using UnityEngine;
using UnityEngine.UI;

public class HealAction : MonoBehaviour
{
    [SerializeField] private Button healButton;


    private void OnEnable()
    {
        CombatSessionManager.onStateChanged += HealUnits;
    }

    private void OnDisable()
    {
        CombatSessionManager.onStateChanged -= HealUnits;
    }

    private void HealUnits(CombatState state)
    {
        if (state == CombatState.PostRound)
        {

        }
        if (state == CombatState.PreRound)
        {

        }

    }
}
