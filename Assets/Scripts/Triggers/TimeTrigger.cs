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

    [Header("Trigger Status")]
    [SerializeField] public bool isVisited = false; // 방문 여부

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isVisited) return;
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
        //if (isVisited) return;
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

    private void OnDisable()
    {
        Debug.Log("규칙이 파훼되어 타이머가 중단됩니다다");
        Debug.Log($"Time Trigger Stopped: {ruleID}");
        StopCoroutine(deathCoroutine);
    }
}
