using UnityEngine;

/// <summary>
/// 위치 기반 트리거
/// </summary>

public class PositionTrigger : RuleTrigger
{
    [Header("Trigger Mode")]
    [SerializeField] private bool isRepeatable; // 반복 모드
    [SerializeField] private bool isTrapMode; // 덫 모드
    [SerializeField] private bool isChaseMode; // 적이 쫓아오는 모드

    [Header("Externer Object")]
    [SerializeField] private GameObject enemy; // 적

    [Header("Trigger Status")]
    [SerializeField] private bool isVisited; // 방문 여부

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        if (isTrapMode)
        {
            // 덫 모드: 규칙 트리거 바로 실행
            Trigger(false);
        }
        else if (isChaseMode && enemy != null && !isVisited)
        {
            // 쫓아오는 모드: 적 활성화
            enemy.SetActive(true);
            isVisited = true;
        }
    }
}
