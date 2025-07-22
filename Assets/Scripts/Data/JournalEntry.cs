using System.Collections.Generic;

public class JournalEntry
{
    public int day;
    public bool survived;
    public List<string> unlockedRuleIDs;
    public List<string> logLines = new List<string>();

    public string ToFormattedString()
    {
        string status = survived ? "생존" : "진행 중/실패";
        // List에 담긴 모든 로그를 하나의 문자열로 합쳐서 보여줍니다.
        string fullLog = string.Join("\n", logLines);
        return $"[Day {day}: {status}]\n{fullLog}";
    }
}