using System.Collections.Generic;
using UnityEngine;

public class DayManager : MonoBehaviour
{
    public static DayManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        //DontDestroyOnLoad(gameObject); // 필요하다면
    }

    [SerializeField] private List<DayData> allDays; // Resources에서 로드하거나 인스펙터에서 할당
    private DayData currentDayData;

    public int currentDay { get; private set; } = 0;

    public void StartDay(int dayNumber)
    {
        if (dayNumber <= 0 || dayNumber > allDays.Count)
        {
            Debug.LogError($"Day {dayNumber}는 유효하지 않습니다.");
            return;
        }

        currentDay = dayNumber;
        currentDayData = allDays[dayNumber - 1];

        Debug.Log($"[DayManager] Day {currentDay} 시작. 규칙 개수: {currentDayData.essentialRules.Count}");

        // 여기서 RuleManager 등에 규칙 전달하는 로직 포함 가능
        RuleManager.Instance?.InitializeRules(currentDayData);

        // 추가: 연출, UI 초기화 등
    }

    public void EndDay(bool isSuccess)
    {
        if (isSuccess)
        {
            // 1. 기존에 있던 실패 기록들을 가져옵니다.
            JournalEntry oldEntry = JournalManager.Instance.GetEntryByDay(currentDay); // JournalManager에 GetEntryByDay 함수 추가 필요
            int failureCount = oldEntry?.logLines.Count ?? 0; // 실패 횟수 카운트

            // 2. 새로운 '완성본' 일지를 만듭니다.
            JournalEntry finalEntry = new JournalEntry();
            finalEntry.day = currentDay;
            finalEntry.survived = true;

            // 3. 실패 경험을 요약하는 문장을 추가합니다.
            if (failureCount > 0)
            {
                finalEntry.logLines.Add($"({failureCount}번의 꿈을 반복하고 나서야...)");
            }

            // 4. 해결한 모든 규칙의 성공 로그를 추가합니다.
            List<RuleData> allRulesToday = new List<RuleData>(currentDayData.essentialRules);
            allRulesToday.AddRange(currentDayData.hiddenRules);
            foreach (var rule in allRulesToday)
            {
                if (RuleManager.Instance.WasRuleSolved(rule.ruleID))
                {
                    finalEntry.logLines.Add(rule.successDescription);
                }
            }
            
            // 5. JournalManager의 기록을 이 '완성본'으로 덮어씁니다.
            JournalManager.Instance.OverwriteEntry(finalEntry); // OverwriteEntry 함수 추가 필요
            Debug.Log($"[DayManager] Day {currentDay} 클리어 성공! 일지 출력 준비");
            // 일지 출력 → CalendarScene 이동 등
            GameManager.Instance.OnStageClear();
        }
        else
        {
            // 기존 방식대로 실패 로그를 한 줄 추가합니다.
            string failureLog = "";
            foreach (var rule in currentDayData.essentialRules)
            {
                if (!RuleManager.Instance.WasRuleSolved(rule.ruleID))
                {
                    failureLog = rule.failDescription;
                    break;
                }
            }
            JournalManager.Instance.AddLog(currentDay, isSuccess, failureLog);
            Debug.Log($"[DayManager] Day {currentDay} 실패. 회귀 처리");
            // 회귀 → CalendarScene 이동
        }
    }

    public DayData GetCurrentDayData()
    {
        return currentDayData;
    }

    public RuleData GetRuleDataByID(string ruleID)
    {
        if (currentDayData == null)
        {
            Debug.LogWarning("[DayManager] 현재 DayData가 없습니다.");
            return null;
        }

        // 필수 규칙에서 찾기
        RuleData rule = currentDayData.essentialRules.Find(r => r.ruleID == ruleID);

        // 없으면 히든 규칙에서 찾기
        if (rule == null)
        {
            rule = currentDayData.hiddenRules.Find(r => r.ruleID == ruleID);
        }

        return rule;
    }

}
