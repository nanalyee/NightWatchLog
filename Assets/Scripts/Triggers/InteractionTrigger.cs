using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 상호작용 트리거
/// </summary>
public class InteractionTrigger : RuleTrigger
{
    private bool isPlayerInRange = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = true;
            Debug.Log($"Player entered interaction zone: {ruleID}");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = false;
            Debug.Log($"Player exited interaction zone: {ruleID}");
        }
    }

    private void Update()
    {
        if (isPlayerInRange && Keyboard.current.eKey.wasPressedThisFrame)
        {
            Trigger(true);
            Debug.Log($"Interaction Trigger activated: {ruleID}");
        }
    }
}
