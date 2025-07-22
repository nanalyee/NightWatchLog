using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class ManualUIManager : MonoBehaviour
{
    [Header("UI Panels & Toggles")]
    [SerializeField] private GameObject manualUI; // 수첩 UI
    [SerializeField] private GameObject manualPanel; // 매뉴얼 패널
    [SerializeField] private GameObject journalPanel; // 일지 패널 

    [Header("Manual Content Setup")]
    [SerializeField] private GameObject ruleItemPrefab; // 규칙 항목으로 사용할 프리팹
    [SerializeField] private Transform contentParent; // 규칙 항목들이 생성될 부모 위치 (Scroll View의 Content)

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

        if (isManualOpen)
        {
            UpdateManualDisplay();
            // 기본으로 매뉴얼 탭을 보여줌
            ShowManualTab(); 
        }
    }

    private void UpdateManualDisplay()
    {
        // 1. 기존에 표시된 규칙 항목들을 모두 삭제
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        // 2. RuleManager에서 해금된 규칙 데이터 리스트를 가져옴
        List<RuleData> unlockedRules = RuleManager.Instance.GetUnlockedRuleDataList();

        // 3. 가져온 규칙들을 UI 항목으로 생성
        foreach (RuleData rule in unlockedRules)
        {
            GameObject newItem = Instantiate(ruleItemPrefab, contentParent);
            TextMeshProUGUI ruleText = newItem.GetComponent<TextMeshProUGUI>();
            if (ruleText != null && rule != null)
            {
                ruleText.text = $"- {rule.description}"; // RuleData의 설명을 텍스트로 표시
            }
        }
    }

    public void ShowManualTab()
    {
        manualPanel.SetActive(true);
        journalPanel.SetActive(false);
        UpdateManualDisplay();
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
        isManualOpen = false;
    }
}
