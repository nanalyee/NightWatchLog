using UnityEngine;

/// <summary>
/// 활성화되면 플레이어를 쫓아오는 간단한 적 로직
/// </summary>
public class EnemyChase : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;  // 이동 속도
    private Transform player; // 플레이어 위치 참조

    private void OnEnable()
    {
        // 적이 활성화되면 플레이어를 찾는다
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (player == null) return;

        // 플레이어 방향으로 이동 (Transform 방식)
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;
    }
}
