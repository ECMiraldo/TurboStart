using UnityEngine;
using UnityEngine.EventSystems;

public class PanelBackground  : MonoBehaviour, IPointerClickHandler 
{
    [SerializeField] UIPanelController panelController;

    public void OnPointerClick(PointerEventData eventData)
    {
        panelController.Close();
    }
}
