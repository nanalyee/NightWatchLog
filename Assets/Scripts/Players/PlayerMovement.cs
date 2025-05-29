using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 2f;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 moveVelocity;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        // 플레이어 입력 받기
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveInput = new Vector2(moveX, moveY).normalized;
        moveVelocity = moveInput * moveSpeed;

        // 애니메이터에 Speed 값 전달
        //animator.SetFloat("Speed", moveVelocity.magnitude);

        // 좌우 반전 처리
        if (moveX != 0) spriteRenderer.flipX = moveX < 0;    
    }

    private void FixedUpdate()
    {
        // Rigidbody2D를 통한 이동 처리
        rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
    }
}
