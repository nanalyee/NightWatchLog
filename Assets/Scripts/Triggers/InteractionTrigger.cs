using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 상호작용 트리거
/// </summary>
public class InteractionTrigger : RuleTrigger
{
    [Header("Trigger Mode")]
    [SerializeField] private bool isChaseMode; // 적이 쫓아오는 모드
    [SerializeField] private bool isEquipMode; // 소지품 모드
    [SerializeField] private bool isSolveMode; // 파훼 모드

    [Header("EquipMode Object name")]
    [SerializeField] private string objectName;

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
        if (!isPlayerInRange || !Keyboard.current.eKey.wasPressedThisFrame)
            return;

        Interact();
    }

    private void Interact()
    {
        if (isChaseMode)
            PerformChaseMode();
        else if (isEquipMode)
            PerformEquipMode();
        else if (isSolveMode)
            PerformSolveMode();
    }

    private void PerformChaseMode()
    {
        Trigger(true);
        Debug.Log($"Interaction Trigger activated: {ruleID}");

        if (enemy != null)
            enemy.SetActive(false);
    }

    private void PerformEquipMode()
    {
        if (string.IsNullOrEmpty(objectName))
        {
            Debug.LogWarning("획득할 아이템 이름이 없습니다.");
            return;
        }

        InventoryManager.Instance.AddItem(objectName);
        Debug.Log($"아이템 획득: {objectName}");
        gameObject.SetActive(false);
    }

    private void PerformSolveMode()
    {
        if (InventoryManager.Instance.HasItem("e_MirroShard_A")
        && InventoryManager.Instance.HasItem("e_MirroShard_B")
        && InventoryManager.Instance.HasItem("e_MirroShard_C"))
        {
            InventoryManager.Instance.UseItem("e_MirroShard_A");
            InventoryManager.Instance.UseItem("e_MirroShard_B");
            InventoryManager.Instance.UseItem("e_MirroShard_C");

            Trigger(true);

            foreach (var script in GetComponents<RuleTrigger>())
            {
                script.enabled = false;
            }
        }
    }
}
