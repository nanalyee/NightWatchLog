using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance => instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        DayManager.Instance?.StartDay(1);
    }

    public void OnPlayerDied()
    {
        Debug.Log("게임 오버 처리 시작");

        // 게임 오버 패널 표시 (UI 매니저와 연동 가능)
        UIManager.Instance.ShowGameOverPanel();

        // 점수 계산, 랭킹 처리, 광고 호출 등 추가 가능
    }
}
