using System.Collections.Generic;
using UnityEngine;

public class RuleManager : MonoBehaviour
{
    public static RuleManager Instance { get; private set; }

    [SerializeField] private RuleDatabase ruleDatabase;

    private class RuleStatus
    {
        public bool isUnlocked = false; // 해금 여부
        public bool isSolved = false; // 파훼 여부
    }

    private Dictionary<string, RuleStatus> ruleStatusMap = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        //DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        InitializeRuleStatus();
    }

    private void OnEnable()
    {
        EventPublisher.OnRuleTriggered += HandleRuleTriggered;
    }

    private void OnDisable()
    {
        EventPublisher.OnRuleTriggered -= HandleRuleTriggered;
    }

    /*
    private void HandleRuleTriggered(string ruleID, bool isSolved)
    {
        if (isSolved)
        {
            UnlockRule(ruleID);
        }
    }
    */
    // DayData를 받아서 규칙 해금 상태 초기화
    public void InitializeRules(DayData dayData)
    {
        if (dayData == null)
        {
            Debug.LogWarning("RuleManager: DayData가 null입니다.");
            return;
        }

        // 기존 unlock 상태 초기화
        ruleStatusMap.Clear();
        AddRuleList(dayData.essentialRules);
        AddRuleList(dayData.hiddenRules);

        Debug.Log($"[RuleManager] Day{dayData.dayID} 규칙 {ruleStatusMap.Count}개 초기화 완료!");
    }

    private void AddRuleList(List<RuleData> rules)
    {
        foreach (var rule in rules)
        {
            if (rule != null && !ruleStatusMap.ContainsKey(rule.ruleID))
            {
                ruleStatusMap.Add(rule.ruleID, new RuleStatus());
            }
        }
    }

    // ✅ 트리거 발생 시 해금 처리
    private void HandleRuleTriggered(string ruleID, bool status)
    {
        Debug.Log($"[RuleManager] HandleRuleTriggered 호출됨! ruleID: {ruleID}, status(isSolved): {status}");

        RuleData rule = GetRuleByID(ruleID);
        if (rule == null)
        {
            LogWarnNoMatchingID(ruleID);
            return;
        }

        if (ruleStatusMap.TryGetValue(ruleID, out RuleStatus ruleStatus))
        {
            if (status)
            {
                // 💡 해금되지 않았으면 해금 처리
                if (!ruleStatus.isUnlocked)
                {
                    ruleStatus.isUnlocked = true;
                    Debug.Log($"[RuleManager] 규칙 {rule.ruleName} 해금됨!");
                }

                // 💡 파훼 상태 처리
                if (!ruleStatus.isSolved)
                {
                    ruleStatus.isSolved = true;
                    Debug.Log($"[RuleManager] 규칙 {rule.ruleName} 파훼됨!");
                }

                // 💡 히든 규칙일 경우 알림
                if (rule.ruleType == RuleType.Hidden)
                {
                    Debug.Log($"숨겨진 규칙이 발견되었습니다: {rule.ruleName}");
                }

                // 현재 규칙 상태 전체 디버그 출력
                DebugAllRuleStatus();

                // 모든 필수 규칙 파훼 여부 확인
                CheckClear();
            }
            else
            {
                Debug.Log($"[RuleManager] 규칙 {rule.ruleName} 파훼 실패 (플레이어 사망 루트에서 처리됨)");
                // RuleManager는 파훼 실패 상태는 처리 안 함 (플레이어 쪽에서 처리)
            }
        }
        else
        {
            Debug.LogWarning($"RuleID {ruleID}가 현재 Day의 규칙에 없습니다.");
        }
    }


    // ✅ 모든 필수 규칙이 파훼되었으면 Day 클리어 처리
    private void CheckClear()
    {
        if (AreAllEssentialRulesSolved(DayManager.Instance.GetCurrentDayData()))
        {
            Debug.Log("[RuleManager] 모든 필수 규칙을 파훼! DayManager.EndDay(true) 호출!");
            DayManager.Instance.EndDay(true);
        }
    }

    // ✅ 해금 상태 설정
    public void UnlockRule(string ruleID)
    {
        if (ruleStatusMap.TryGetValue(ruleID, out RuleStatus status))
        {
            if (!status.isUnlocked)
            {
                status.isUnlocked = true;
                Debug.Log($"[RuleManager] 규칙 {ruleID} 해금됨!");
            }
            if (!status.isSolved)
            {
                status.isSolved = true;
                Debug.Log($"[RuleManager] 규칙 {ruleID} 파훼됨!");
            }
        }
        else
        {
            Debug.LogWarning($"[RuleManager] 규칙 ID {ruleID}가 현재 Day에 없습니다.");
        }
    }

    // ✅ 모든 필수 규칙 파훼 여부 확인
    public bool AreAllEssentialRulesSolved(DayData dayData)
    {
        foreach (var rule in dayData.essentialRules)
        {
            if (!ruleStatusMap.TryGetValue(rule.ruleID, out RuleStatus status) || !status.isSolved)
            {
                return false;
            }
        }
        return true;
    }

    // 규칙 로드
    public void LoadSavedData(Dictionary<string, bool> savedStatus)
    {
        if (savedStatus == null)
        {
            Debug.LogWarning("불러올 저장 상태가 없습니다.");
            return;
        }

        foreach (var kvp in savedStatus)
        {
            if (ruleStatusMap.ContainsKey(kvp.Key))
            {
                ruleStatusMap[kvp.Key].isUnlocked = kvp.Value;
            }
        }
    }

    // ✅ 외부에서 규칙의 해금 여부 확인
    public bool IsRuleUnlocked(string ruleID)
    {
        return ruleStatusMap.ContainsKey(ruleID) && ruleStatusMap[ruleID].isUnlocked;
    }

    // ✅ 규칙 강제 해금 (디버그용)
    public void ForceUnlockRule(string ruleID)
    {
        RuleData rule = GetRuleByID(ruleID);
        if (rule == null)
        {
            LogWarnNoMatchingID(ruleID);
            return;
        }
        ruleStatusMap[ruleID].isUnlocked = true;
        Debug.Log($"규칙 강제 해금됨: {rule.ruleName}");
    }

    // ✅ RuleDatabase에서 RuleData 검색
    private RuleData GetRuleByID(string ruleID)
    {
        return ruleDatabase.rules.Find(r => r.ruleID == ruleID);
    }

    // ✅ 불일치 규칙 로그
    private void LogWarnNoMatchingID(string ruleID)
    {
        Debug.LogWarning($"No matching rule found for ID: {ruleID}");
    }

    // ✅ 모든 규칙 상태 리셋
    public void ResetRules()
    {
        ruleStatusMap.Clear();
    }

    // ✅ ruleDatabase 기준으로 unlock 상태를 초기화 (필요 시)
    private void InitializeRuleStatus()
    {
        foreach (var rule in ruleDatabase.rules)
        {
            if (!ruleStatusMap.ContainsKey(rule.ruleID))
            {
                ruleStatusMap[rule.ruleID] = new RuleStatus();
            }
        }
    }

    private void DebugAllRuleStatus()
    {
        Debug.Log("=== [RuleManager Debug] 현재 모든 규칙 상태 ===");
        foreach (var kvp in ruleStatusMap)
        {
            string ruleID = kvp.Key;
            RuleStatus status = kvp.Value;
            Debug.Log($"RuleID: {ruleID} | isUnlocked: {status.isUnlocked}, isSolved: {status.isSolved}");
        }
        Debug.Log("=============================================");
    }

    public List<RuleData> GetUnlockedRuleDataList()
    {
        var unlockedRules = new List<RuleData>();

        // 현재 상태 맵(ruleStatusMap)에 있는 모든 규칙을 확인합니다.
        foreach (var kvp in ruleStatusMap)
        {
            string ruleID = kvp.Key;
            RuleStatus status = kvp.Value;

            // 만약 규칙이 해금(isUnlocked) 상태라면,
            if (status.isUnlocked)
            {
                // ruleID를 이용해 ruleDatabase에서 원본 RuleData를 찾습니다.
                RuleData ruleData = GetRuleByID(ruleID);
                if (ruleData != null)
                {
                    // 찾은 RuleData를 리스트에 추가합니다.
                    unlockedRules.Add(ruleData);
                }
            }
        }

        return unlockedRules; // 해금된 규칙 데이터 목록을 반환합니다.
    }
    
    // 특정 규칙의 성공(solved) 여부를 반환
    public bool WasRuleSolved(string ruleID)
    {
        if (ruleStatusMap.TryGetValue(ruleID, out RuleStatus status))
        {
            return status.isSolved;
        }
        return false;
    }

    // 현재 해금된 모든 규칙의 ID 리스트를 반환
    public List<string> GetUnlockedRuleIDs()
    {
        var unlockedIDs = new List<string>();
        foreach (var kvp in ruleStatusMap)
        {
            if (kvp.Value.isUnlocked)
            {
                unlockedIDs.Add(kvp.Key);
            }
        }
        return unlockedIDs;
    }
}
