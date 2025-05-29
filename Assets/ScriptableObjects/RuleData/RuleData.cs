using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "RuleData", menuName = "NightWatchLog/RuleData", order = 0)]
public class RuleData : ScriptableObject
{
    [Header("기본 정보")]
    public string ruleID;              // "Rule01"
    public string ruleName;            // "복도에서 뛰지 마라"
    [TextArea] public string description;     // 규칙 설명 (Manual UI용)
    public RuleType ruleType;          // 필수 or 히든

    [Header("트리거 조건")]
    public string triggerTag;          // ex) "Trigger_HallwayStart"
    public string triggerConditionNote;  // ex) "복도 진입 후 Shift 키 입력"

    [Header("파훼 조건")]
    public string solveConditionNote;   // ex) "걷기 유지 시 생존"

    [Header("해금 상태")]
    public bool isUnlocked = false;     // 플레이 중 동적 제어
    public bool isRepeatable = false;   // 반복 해금 여부

    [Header("히든 규칙 보상 조건")]
    public List<GameObject> requiredItem;     // 특정 아이템 퍼즐에 필요한 경우
    public string unlockHintText;       // ex) "거울 조각을 모두 모아 전신거울을 보면…"

    [Header("일지 내용")]
    [TextArea] public string successDescription;
    [TextArea] public string failDescription;
}