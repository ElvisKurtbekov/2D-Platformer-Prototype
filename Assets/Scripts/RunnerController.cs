using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(Animator))]
public class RunnerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;

    [Header("Environment Check")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float wallCheckDistance = 0.1f;

    private Rigidbody2D rb;
    private CapsuleCollider2D col;
    private Animator anim;
    private AudioSource audioSource;

    private bool isGrounded;
    private bool isTouchingWall;
    private bool jumpLocked;

    private Vector2 originalColliderSize;
    private Vector2 crouchColliderSize;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CapsuleCollider2D>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        originalColliderSize = col.size;
        crouchColliderSize = new Vector2(col.size.x, col.size.y / 2f);
    }

    private void Update()
    {
        CheckGround();
        CheckWall();
        HandleJump();
        HandleCrouch();
        UpdateAnimations();
        CheckFall();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void CheckGround()
    {
        bool wasGrounded = isGrounded;

        RaycastHit2D hit = Physics2D.BoxCast(
            col.bounds.center,
            col.bounds.size * 0.95f,
            0f,
            Vector2.down,
            0.1f,
            groundLayer
        );

        isGrounded = hit.collider != null && hit.normal.y > 0.5f;

        if (isGrounded && !wasGrounded)
            jumpLocked = false;
    }

    private void CheckWall()
    {
        Vector2 direction = new Vector2(
            Mathf.Sign(rb.velocity.x == 0 ? transform.localScale.x : rb.velocity.x),
            0
        );

        RaycastHit2D wallHit = Physics2D.BoxCast(
            col.bounds.center,
            col.bounds.size * 0.9f,
            0f,
            direction,
            wallCheckDistance,
            groundLayer
        );

        isTouchingWall =
            wallHit.collider != null &&
            Mathf.Abs(wallHit.normal.x) > 0.5f;
    }

    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !jumpLocked)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpLocked = true;
        }
    }

    private void HandleCrouch()
    {
        bool isCrouching = Input.GetKey(KeyCode.LeftControl);

        col.size = isCrouching ? crouchColliderSize : originalColliderSize;
        col.offset = new Vector2(0, col.size.y / 2f);
    }

    private void HandleMovement()
    {
        float move = 0f;

        if (Input.GetKey(KeyCode.D)) move = 1f;
        else if (Input.GetKey(KeyCode.A)) move = -1f;

        if (!isGrounded && isTouchingWall)
            move = 0f;

        rb.velocity = new Vector2(move * moveSpeed, rb.velocity.y);

        if (move != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * Mathf.Sign(move);
            transform.localScale = scale;
        }
    }

    private void UpdateAnimations()
    {
        bool isRunning = Mathf.Abs(rb.velocity.x) > 0.1f && isGrounded;

        anim.SetBool("isRunning", isRunning);
        anim.SetBool("isGrounded", isGrounded);
    }

    private void CheckFall()
    {
        if (transform.position.y < -6f)
        {
            audioSource?.Stop();
            GameManager.Instance.GameOver();
        }
    }
}