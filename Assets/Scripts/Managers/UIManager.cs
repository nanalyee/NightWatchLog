using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private GameObject gamePanel;
    [SerializeField] private TMP_Text gameText;
    [SerializeField] private TMP_Text dayText;

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

    void Start()
    {
        ShowDay();
    }

    public void ShowPanel(bool isClear)
    {
        if (gamePanel != null)
        {
            gamePanel.SetActive(true);
            Debug.Log("게임 패널 표시");
            ShowStatus(isClear);
        }
        else
        {
            Debug.LogWarning("게임 패널이 연결되지 않았습니다.");
        }
    }

    private void ShowStatus(bool isClear)
    {
        if (isClear)
        {
            gameText.text = "Game Clear";
            Debug.Log("게임 클리어 패널 표시");
        }
        else
        {
            gameText.text = "Game Over";
            Debug.Log("게임 오버 패널 표시");
        }
    }

    private void ShowDay()
    {
        Debug.Log(DayManager.Instance.currentDay + "일차를 시작합니다.");
        dayText.text = "Day " + DayManager.Instance.currentDay; 
    }
}
