using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private IPlayerState currentState;
    private Animator animator;

    // 1. Animator.StringToHash()
    // 문자열을 정수형 해시 값으로 변환
    // 애니메이터의 파라미터를 문자열 대신 정수로 참조하여 성능을 최적화
    // "StateID"라는 문자열을 정수로 변환하여 Animator의 파라미터로 사용
    // 2. static readonly
    // static readonly로 선언하여 런타임에 한 번만 계산하도록 설정 (최적화)
    // 동일한 문자열을 여러 번 참조할 때 메모리 사용량 감소
    private static readonly int StateID = Animator.StringToHash("StateID");


    private void OnEnable()
    {
        EventPublisher.OnRuleTriggered += HandleRuleTriggered;
    }

    private void OnDisable()
    {
        EventPublisher.OnRuleTriggered -= HandleRuleTriggered;
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        SetState(new IdleState());
    }

    private void Update()
    {
        currentState?.Update(this);
    }

    private void HandleRuleTriggered(string ruleID)
    {
        if (ruleID == "Hidden01" && !(currentState is DeadState))
        {
            SetState(new DeadState());
            Debug.Log("플레이어가 사망했습니다.");
            GameManager.Instance.OnPlayerDied();
            DayManager.Instance?.EndDay(false);
        }
    }
    public void SetState(IPlayerState newState)
    {
        currentState?.Exit(this);
        currentState = newState;
        currentState.Enter(this);

        // 상태를 Int 파라미터로 전달
        int stateID;
        switch (newState.GetType().Name)
        {
            case "IdleState":
                stateID = 0;
                break;
            case "WalkingState":
                stateID = 1;
                break;
            case "DeadState":
                stateID = 2;
                Debug.Log($"Player State changed to: {newState.GetType().Name}");
                break;
            default:
                stateID = 0;
                break;
        }

        animator.SetInteger(StateID, stateID);
        
    }
}
