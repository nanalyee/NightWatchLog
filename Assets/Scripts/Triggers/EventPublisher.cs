using System;
using UnityEngine;

/// <summary>
/// Rule Trigger가 발생했을 때 다른 컴포넌트로 전달하는 중앙 허브
/// RuleTrigger와 RuleManager가 직접 연결되지 않고도 이벤트 처리를 위함
/// </summary>
public class EventPublisher : MonoBehaviour
{
    // 이벤트 선언
    public static event Action<string> OnRuleTriggered;

    // 이벤트 발송
    public static void TriggerRule(string ruleID)
    {
        // 이벤트 구독 중일 때 호출
        OnRuleTriggered?.Invoke(ruleID);
        Debug.Log($"Rule Triggered: {ruleID}");
    }
}
