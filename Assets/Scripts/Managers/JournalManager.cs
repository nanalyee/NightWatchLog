using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JournalManager : MonoBehaviour
{
    public static JournalManager Instance { get; private set; }

    private List<JournalEntry> allEntries = new List<JournalEntry>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // 일지 내용은 게임 내내 유지되어야 합니다.
    }

    public void AddLog(int day, bool isSuccess, string logLine)
    {
        JournalEntry entry = allEntries.FirstOrDefault(e => e.day == day);

        // 만약 이번 도전이 '성공'이라면, 기존 일지를 아예 새로 만듭니다.
        if (isSuccess && entry != null)
        {
            allEntries.Remove(entry); // 기존 기록 삭제
            entry = null; // 리셋
        }

        if (entry == null)
        {
            entry = new JournalEntry { day = day };
            allEntries.Add(entry);
        }

        entry.logLines.Add(logLine);
        entry.survived = isSuccess;

        Debug.Log("--- 일지 로그가 추가/갱신되었습니다 ---");
        Debug.Log(entry.ToFormattedString());
    }

    public List<JournalEntry> GetAllEntries()
    {
        return allEntries;
    }
    
    public JournalEntry GetEntryByDay(int day)
    {
        return allEntries.FirstOrDefault(e => e.day == day);
    }

    public void OverwriteEntry(JournalEntry newEntry)
    {
        allEntries.RemoveAll(entry => entry.day == newEntry.day);
        allEntries.Add(newEntry);
        Debug.Log("--- 일지가 완성본으로 갱신되었습니다 ---");
        Debug.Log(newEntry.ToFormattedString());
    }
}