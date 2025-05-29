using UnityEngine;

public class RuleManager : MonoBehaviour
{
    [SerializeField] private RuleDatabase ruleDatabase;

    private void OnEnable()
    {
        EventPublisher.OnRuleTriggered += HandleRuleTriggered;
    }

    private void OnDisable()
    {
        EventPublisher.OnRuleTriggered -= HandleRuleTriggered;
    }

    private void HandleRuleTriggered(string ruleID)
    {
        RuleData rule = ruleDatabase.rules.Find(r => r.ruleID == ruleID);
        if (rule != null)
        {
            if (!rule.isUnlocked)
            {
                rule.isUnlocked = true;
                Debug.Log($"Rule Unlocked: {rule.ruleName} - {rule.description}");

                // UI 갱신 또는 로그 추가 가능
                if (rule.ruleType == RuleType.Hidden)
                {
                    Debug.Log($"숨겨진 규칙이 발견되었습니다: {rule.ruleName}");
                }
            }
            else
            {
                Debug.Log($"이미 해금된 규칙입니다: {rule.ruleName}");
            }
        }
        else
        {
            Debug.LogWarning($"No matching rule found for ID: {ruleID}");
        }
    }


    // 외부에서 규칙의 해금 여부 확인
    public bool IsRuleUnlocked(string ruleID)
    {
        RuleData rule = ruleDatabase.rules.Find(r => r.ruleID == ruleID);
        return rule != null && rule.isUnlocked;
    }

    // 규칙 수동 해금 (필요 시)
    public void ForceUnlockRule(string ruleID)
    {
        RuleData rule = ruleDatabase.rules.Find(r => r.ruleID == ruleID);
        if (rule != null)
        {
            rule.isUnlocked = true;
            Debug.Log($"규칙 강제 해금됨: {rule.ruleName}");
        }
    }
}
