using TMPro;
using UnityEngine;

public class ResultUI : UIPanelController
{
    [SerializeField] private ProgressBar progressBar;
    [SerializeField] private TextMeshProUGUI mainText;

    private void OnEnable()
    {
        progressBar.onComplete += Close;
    }

    public void ShowDefeat()
    {
        Open();
        mainText.text = "Defeat";
        HandleProgressBar();
    }

    public void ShowVictory()
    {
        Open();
        mainText.text = "Victory";
        HandleProgressBar();
    }

    private void HandleProgressBar()
    {
        if (CombatSessionManager.Instance.isAutoplay)
        {
            progressBar.gameObject.SetActive(true);
            progressBar.StartFill(CombatSessionManager.Instance.resultScreenTime);
        }
        else
        {
            progressBar.gameObject.SetActive(false);
        }
    }
    public void OnLeaveClicked()
    {
        CombatSessionManager.Instance.PlayerClickedLeave();
        Close();
    }

    public void OnRestartClicked()
    {
        CombatSessionManager.Instance.PlayerClickedRestart();
        Close();
    }
}
