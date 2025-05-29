using System.Collections.Generic;
using UnityEngine;

public class RuleManager : MonoBehaviour
{
    [SerializeField] private RuleDatabase ruleDatabase;


    // 런타임 해금 상태 관리용
    Dictionary<string, bool> ruleUnlockStatus = new();

    private void Start()
    {
        InitializeRuleStatus();
        DebugUnlockRule();
    }

    private void OnEnable()
    {
        EventPublisher.OnRuleTriggered += HandleRuleTriggered;
    }

    private void OnDisable()
    {
        EventPublisher.OnRuleTriggered -= HandleRuleTriggered;
    }

    private void InitializeRuleStatus()
{
    foreach (var rule in ruleDatabase.rules)
    {
        // 이미 저장된 값이 있다면 유지 (예: 로드된 상태)
        if (!ruleUnlockStatus.ContainsKey(rule.ruleID))
        {
            ruleUnlockStatus[rule.ruleID] = false;
        }
    }
}

    // 트리거 처리 함수
    private void HandleRuleTriggered(string ruleID)
    {
        RuleData rule = GetRuleByID(ruleID);
        if (rule == null)
        {
            LogWarnNoMatchingID(ruleID);
            return;
        }

        if (!ruleUnlockStatus[ruleID])
        {
            ruleUnlockStatus[ruleID] = true;
            Debug.Log($"Rule Unlocked: {rule.ruleName} - {rule.description}");

            if (rule.ruleType == RuleType.Hidden)
            {
                Debug.Log($"숨겨진 규칙이 발견되었습니다: {rule.ruleName}");
            }

            // UI 갱신, 로그 추가 등 후속 작업
            DebugUnlockRule();
        }
        else
        {
            Debug.Log($"이미 해금된 규칙입니다: {rule.ruleName}");
        }
    }


    // 규칙 로드
    public void LoadSavedData(Dictionary<string, bool> savedStatus)
    {
        if (savedStatus == null)
        {
            Debug.LogWarning("불러올 저장 상태가 없습니다.");
            return;
        }

        ruleUnlockStatus = new Dictionary<string, bool>(savedStatus);
        InitializeRuleStatus(); // 누락 보완
    }

    // 외부에서 규칙의 해금 여부 확인
    public bool IsRuleUnlocked(string ruleID)
    {
        return ruleUnlockStatus.ContainsKey(ruleID) && ruleUnlockStatus[ruleID];
    }

    // 규칙 강제 해금 (개발용, 디버그용)
    public void ForceUnlockRule(string ruleID)
    {
        RuleData rule = GetRuleByID(ruleID);
        if (rule == null)
        {
            LogWarnNoMatchingID(ruleID);
            return;
        }

        ruleUnlockStatus[ruleID] = true;
        Debug.Log($"규칙 강제 해금됨: {rule.ruleName}");
    }

    // Rule ID로 RuleData 검색
    private RuleData GetRuleByID(string ruleID)
    {
        return ruleDatabase.rules.Find(r => r.ruleID == ruleID);
    }

    // 불일치 규칙 로그
    private void LogWarnNoMatchingID(string ruleID)
    {
        Debug.LogWarning($"No matching rule found for ID: {ruleID}");
    }

    // 테스트용 디버그 함수
    public void DebugUnlockRule()
    {
        if (ruleDatabase.rules.Count > 0)
        {
            HandleRuleTriggered(ruleDatabase.rules[0].ruleID);
        }
    }


    // 모든 규칙 초기화 (필요 시)
    public void ResetRules()
    {
        ruleUnlockStatus.Clear();
    }
}
