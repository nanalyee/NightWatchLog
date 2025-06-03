using UnityEngine;
using System.Collections;

/// <summary>
/// 시간 기반 트리거
/// </summary>
public class TimeTrigger : RuleTrigger
{
    [SerializeField] private float delay = 5f;
    private bool isTriggered = false;
    private Coroutine deathCoroutine;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isTriggered && collision.CompareTag("Player"))
        {
            isTriggered = true;
            deathCoroutine = StartCoroutine(StartTimer());
        }
    }

    private IEnumerator StartTimer()
    {
        Debug.Log($"Time Trigger will activate in {delay} seconds: {ruleID}");
        yield return new WaitForSeconds(delay);
        Trigger(false);
        isTriggered = false;
        deathCoroutine = null;
        Debug.Log($"Time Trigger activated: {ruleID}");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // 플레이어가 영역을 벗어나면 타이머 초기화
        if (collision.CompareTag("Player") && deathCoroutine != null)
        {
            isTriggered = false;
            StopCoroutine(deathCoroutine);
            deathCoroutine = null;
            Debug.Log($"Time Trigger reset: {ruleID}");
            Trigger(true);
        }
    }
}
