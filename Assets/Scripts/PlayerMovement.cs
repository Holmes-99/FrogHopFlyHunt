using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 6f;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private int maxJumps = 2;
    [SerializeField] private LayerMask floorLayer;

    private float inputDelay = 0.5f;
    private float inputTimer = 0f;
    private bool inputEnabled = false;

    private Rigidbody2D rb;
    private Collider2D col;
    private SpriteRenderer sr;
    private Animator anim;

    private float horizontalInput = 0f;
    private bool jumpRequested = false;
    private int jumpsRemaining = 0;
    private bool isOnGround = false;
    private bool facingRight = true;
    private bool isAlive = true;

    private static readonly int AnimIsRunning = Animator.StringToHash("isRunning");
    private static readonly int AnimIsJumping = Animator.StringToHash("isJumping");

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private void Update()
    {
        if (!inputEnabled)
        {
            inputTimer += Time.deltaTime;
            if (inputTimer >= inputDelay)
                inputEnabled = true;
            return;
        }

        if (!isAlive) return;
        ReadInput();
        UpdateGroundCheck();
        UpdateAnimator();
    }

    private void FixedUpdate()
    {
        if (!isAlive) return;
        ApplyHorizontalMovement();
        ApplyJump();
    }

    private void ReadInput()
    {
        // Movement
        if (Keyboard.current.leftArrowKey.isPressed || Keyboard.current.aKey.isPressed)
            horizontalInput = -1f;
        else if (Keyboard.current.rightArrowKey.isPressed || Keyboard.current.dKey.isPressed)
            horizontalInput = 1f;
        else
            horizontalInput = 0f;

        // Jump
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
            if (jumpsRemaining > 0)
                jumpRequested = true;

        // Flip sprite
        if (horizontalInput > 0f) { facingRight = true; sr.flipX = false; }
        else if (horizontalInput < 0f) { facingRight = false; sr.flipX = true; }
    }

    private void ApplyHorizontalMovement()
    {
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
    }

    private void ApplyJump()
    {
        if (!jumpRequested) return;
        jumpRequested = false;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        jumpsRemaining--;

        // Play jump sound
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayJump();
    }

    private void UpdateGroundCheck()
    {
        bool wasOnGround = isOnGround;
        isOnGround = col.IsTouchingLayers(floorLayer);

        if (isOnGround && !wasOnGround)
            jumpsRemaining = maxJumps;

        if (!isOnGround && wasOnGround && jumpsRemaining == maxJumps)
            jumpsRemaining = maxJumps - 1;
    }

    private void UpdateAnimator()
    {
        anim.SetBool(AnimIsRunning, Mathf.Abs(horizontalInput) > 0.01f && isOnGround);
        anim.SetBool(AnimIsJumping, !isOnGround);
    }

    public void DisablePlayer()
    {
        isAlive = false;
        rb.linearVelocity = Vector2.zero;
        anim.SetBool(AnimIsJumping, false);
        anim.SetBool(AnimIsRunning, false);
        anim.SetBool("isDead", true);
    }
}