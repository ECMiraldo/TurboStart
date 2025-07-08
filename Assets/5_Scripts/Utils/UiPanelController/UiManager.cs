using System.Collections.Generic;
using UnityUtils;



public class UiManager : Singleton<UiManager> {


    public List<UIPanelController> openedPannels = new List<UIPanelController>();

    private bool isPaused;
    protected override void Awake()
    {
        base.Awake();
        //playerInputs = new PlayerInputs();
    }

    private void OnEnable()
    {
        UIPanelController.OnPanelOpened += OnPanelOpen;
        UIPanelController.OnPanelClosed += OnPanelClose;
        //playerInputs.UI.Cancel.Enable();
        //playerInputs.UI.Cancel.performed += OnESCPressed;
    }

    private void OnDisable()
    {
        UIPanelController.OnPanelOpened -= OnPanelOpen;
        UIPanelController.OnPanelClosed -= OnPanelClose;

    }

    private void OnPanelOpen(UIPanelController panel) 
    {
        openedPannels.Add(panel);
        panel.transform.SetAsLastSibling();
    }

    private void OnPanelClose(UIPanelController panel)  
    {
        openedPannels.Remove(panel);
    }
}
