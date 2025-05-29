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
        DontDestroyOnLoad(gameObject); // 필요하다면
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
        //RuleManager.Instance?.InitializeRules(currentDayData);

        // 추가: 연출, UI 초기화 등
    }

    public void EndDay(bool isSuccess)
    {
        if (isSuccess)
        {
            Debug.Log($"[DayManager] Day {currentDay} 클리어 성공! 일지 출력 준비");
            // 일지 출력 → CalendarScene 이동 등
        }
        else
        {
            Debug.Log($"[DayManager] Day {currentDay} 실패. 회귀 처리");
            // 회귀 → CalendarScene 이동
        }
    }

    public DayData GetCurrentDayData()
    {
        return currentDayData;
    }
}
