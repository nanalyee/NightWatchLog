using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 상호작용 트리거
/// </summary>
public class InteractionTrigger : RuleTrigger
{
    [Header("Trigger Mode")]
    [SerializeField] private bool isChaseMode; // 적이 쫓아오는 모드

    [Header("Externer Object")]
    [SerializeField] private GameObject enemy; // 적

    private bool isPlayerInRange = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = true;
            Debug.Log($"Player entered interaction zone: {ruleID}");
            // 이펙트 켜기
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = false;
            Debug.Log($"Player exited interaction zone: {ruleID}");
            // 이펙트 끄기
        }
    }

    private void Update()
    {
        if (isPlayerInRange && Keyboard.current.eKey.wasPressedThisFrame)
        {
            Trigger(true);
            Debug.Log($"Interaction Trigger activated: {ruleID}");
            enemy.SetActive(false);
        }
    }
}
