using UnityEngine;

public enum RuleType
{
    Essential,   // 생존에 필요한 필수 규칙
    Optional,     // 선택적 규칙 (중간 힌트, 튜토리얼용 등)
    Hidden       // 퍼즐 보상형 히든 규칙
}
