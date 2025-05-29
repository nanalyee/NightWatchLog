using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "AllRules", menuName = "NightWatchLog/RuleDatabase")]
public class RuleDatabase : ScriptableObject
{
    public List<RuleData> rules = new List<RuleData>();
}
