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
    [SerializeField] private TimeTrigger timeTrigger;

    private bool isPlayerInRange = false;

    private void Start()
    {
        timeTrigger = GetComponent<TimeTrigger>();

        if (timeTrigger != null)
        {
            Debug.Log("TimeTrigger를 성공적으로 찾았습니다!");
        }
        else
        {
            Debug.Log("TimeTrigger가 "+gameObject.name+" 오브젝트에 없습니다.");
        }
    }

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
        // 🔥 DayManager에서 이번 Day의 규칙 데이터를 가져옴
        RuleData ruleData = DayManager.Instance.GetRuleDataByID(ruleID);
        if (ruleData == null || ruleData.requiredItemIDs == null || ruleData.requiredItemIDs.Count == 0)
        {
            Debug.Log(ruleID +" 규칙은 필요한 아이템이 없습니다.");
            return;
        }

        // 1️⃣ 필요한 아이템이 모두 있는지 확인
        foreach (string itemID in ruleData.requiredItemIDs)
        {
            if (!InventoryManager.Instance.HasItem(itemID))
            {
                Debug.Log($"아이템이 부족합니다: {itemID}");
                return;
            }
        }

        // 2️⃣ 필요한 아이템을 모두 소모
        foreach (string itemID in ruleData.requiredItemIDs)
        {
            InventoryManager.Instance.UseItem(itemID);
            Debug.Log($"아이템 사용됨: {itemID}");
        }

        // 3️⃣ 규칙 트리거 해금 처리
        Trigger(true);

        // 4️⃣ 이 InteractionTrigger가 가진 모든 RuleTrigger 비활성화
        foreach (var script in GetComponents<RuleTrigger>())
        {
            script.enabled = false;
        }

        Debug.Log($"[InteractionTrigger] {ruleID} 퍼즐 해금 완료!");
        if (timeTrigger) timeTrigger.isVisited = true;
    }
}
