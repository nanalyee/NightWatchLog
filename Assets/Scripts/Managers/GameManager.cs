using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

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

    private void Start()
    {
        DayManager.Instance?.StartDay(1);
    }

    public void OnPlayerDied()
    {
        Debug.Log("게임 오버 처리 시작");

        // 게임 오버 패널 표시 (UI 매니저와 연동 가능)
        UIManager.Instance.ShowPanel(false);

        // 점수 계산, 랭킹 처리, 광고 호출 등 추가 가능
    }

    public void OnStageClear()
    {
        Debug.Log("스테이지 클리어");
        UIManager.Instance.ShowPanel(true);
    }
}
