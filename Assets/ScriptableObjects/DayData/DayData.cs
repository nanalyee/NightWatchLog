using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "DayData", menuName = "NightWatchLog/DayData", order = 1)]
public class DayData : ScriptableObject
{
    [Header("기본 정보")]
    public string dayID; // 예: "Day01"

    [Header("규칙 정보")]
    public List<RuleData> essentialRules;  // 반드시 해금해야 클리어 가능한 규칙
    public List<RuleData> optionalRules;   // 플레이 중 보조 힌트, 조건
    public List<RuleData> hiddenRules;     // 히든 규칙 (보상 or 깊이 추가)

    [Header("일지 텍스트")]
    [TextArea] public string baseJournalText; 
    [TextArea] public string finalJournalText; 
}
