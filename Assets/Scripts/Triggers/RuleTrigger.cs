using UnityEngine;

/// <summary>
/// 트리거 기본 컴포넌트
/// </summary>
public class RuleTrigger : MonoBehaviour
{
    [SerializeField] protected string ruleID;

    protected virtual void Trigger()
    {
        EventPublisher.TriggerRule(ruleID);
    }

    protected virtual void Awake()
    {
        if (string.IsNullOrEmpty(ruleID))
            Debug.LogWarning($"{gameObject.name}의 RuleTrigger에 ruleID가 설정되지 않았습니다.");
    }
}
