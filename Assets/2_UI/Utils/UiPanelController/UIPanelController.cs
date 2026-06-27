using UnityEngine;
using System;
public abstract class UIPanelController : MonoBehaviour
{
    public static event Action<UIPanelController> OnPanelOpened;
    public static event Action<UIPanelController> OnPanelClosed;
    [SerializeField] protected bool startActive = false;
    [SerializeField] protected GameObject child;
    public bool IsOpen { get; protected set; }
    protected virtual void Start()
    {
        if (child == null && transform.childCount == 1) child = transform.GetChild(0).gameObject;
        if (!startActive) Close();
    }


    public virtual void OnToggle()
    {
        if (IsOpen) Close();
        else Open();
    }

    public virtual void Open()
    {
        IsOpen = true;
        child.SetActive(true);
        OnPanelOpened?.Invoke(this);
    }

    public virtual void Close()
    {
        IsOpen = false;
        child.SetActive(false);
        OnPanelClosed?.Invoke(this);
    }

}
