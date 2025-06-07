using UnityEngine;
using System.Collections.Generic;


public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    private List<string> items = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        //DontDestroyOnLoad(gameObject);
    }

    public void AddItem(string itemID)
    {
        if (!items.Contains(itemID))
        {
            items.Add(itemID);
            Debug.Log($"아이템 추가됨: {itemID}");
        }
    }

    public bool UseItem(string itemID)
    {
        if (items.Contains(itemID))
        {
            items.Remove(itemID);
            Debug.Log($"아이템 사용됨: {itemID}");
            return true;
        }

        Debug.LogWarning($"아이템이 존재하지 않아 사용 불가: {itemID}");
        return false;
    }

    public bool HasItem(string itemID)
    {
        return items.Contains(itemID);
    }

    public List<string> GetAllItems()
    {
        return new List<string>(items); // 복사본 반환
    }
}
