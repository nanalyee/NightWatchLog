using UnityEngine;

public class DeadState : IPlayerState
{
    public void Enter(PlayerController player)
    {
        Debug.Log("DeadState 진입");
        // 사망 애니메이션 재생, UI 표시 등
    }

    public void Update(PlayerController player)
    {
        // 사망 상태에서는 아무런 입력도 받지 않음
    }

    public void Exit(PlayerController player)
    {
        Debug.Log("DeadState 종료");
    }
}
