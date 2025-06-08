using UnityEngine;

public class ManualUIManager : MonoBehaviour
{
    [SerializeField] private GameObject manualUI; // 수첩 UI
    [SerializeField] private GameObject manualPanel; // 매뉴얼 패널
    [SerializeField] private GameObject journalPanel; // 일지 패널 

    private bool isManualOpen = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            ToggleManualUI();
        }
    }

    private void ToggleManualUI()
    {
        isManualOpen = !isManualOpen;
        manualUI.SetActive(isManualOpen);
    }

    public void ShowManualTab()
    {
        manualPanel.SetActive(true);
        journalPanel.SetActive(false);
    }

    public void ShowJournalTab()
    {
        manualPanel.SetActive(false);
        journalPanel.SetActive(true);
    }

    public void CloseManualUI()
    {
        manualUI.SetActive(false);
        manualPanel.SetActive(true);
        journalPanel.SetActive(false);
    }
}
