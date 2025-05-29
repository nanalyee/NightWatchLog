using UnityEngine;

/// <summary>
/// 위치 기반 트리거
/// </summary>
public class PositionTrigger : RuleTrigger
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Trigger();
        }
    }
}
