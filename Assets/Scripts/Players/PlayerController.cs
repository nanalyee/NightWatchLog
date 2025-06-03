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

    private void HandleRuleTriggered(string ruleID, bool isSolved)
    {
        if (isSolved)
        {
            Debug.Log($"Rule {ruleID} 파훼 성공!");

            // 파훼 성공 → RuleManager에 규칙 해금 요청
            RuleManager.Instance?.UnlockRule(ruleID);
            // 이후에 DayManager에서 규칙 해금 상태/파훼 여부 등을 UI 업데이트 등에 활용 가능
        }
        else
        {
            Debug.Log($"Rule {ruleID} 파훼 실패. 사망 처리!");

            if (!(currentState is DeadState))
            {
                SetState(new DeadState());
                Debug.Log("플레이어가 사망했습니다.");
                GameManager.Instance.OnPlayerDied();
                DayManager.Instance?.EndDay(false);
            }
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
