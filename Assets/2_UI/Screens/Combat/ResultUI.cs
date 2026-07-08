using Persistence;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[DefaultExecutionOrder(+1)]
public class ResultUI : UIPanelController
{
    public static ResultUI instance { get; private set; }


    [SerializeField] private ProgressBar progressBar;
    [SerializeField] private TextMeshProUGUI mainText;

    private List<Item> itemRewards = new List<Item>();
    private int goldRewards = 0;

    private ResourceData resourceData;
    private Inventory inventory;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this.gameObject);
        resourceData = SaveLoadSystem.Instance.data.resourceData;
        inventory = SaveLoadSystem.Instance.data.inventory;
    }
    public override void Close()
    {
        base.Close();
        itemRewards.Clear();
        goldRewards = 0;
    }


    private void OnEnable()
    {
        progressBar.onComplete += Close;
    }

    public void ShowDefeat()
    {
        Open();
        mainText.text = "Defeat";
    }

    public void ShowVictory()
    {
        Open();
        mainText.text = "Victory";
    }


}
