using System.Collections;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Movement Values")]
    public float speed;
    public float jumpPower;
    public float groundCheckDistance = 0.05f;
    public bool isGround;

    [Header("Dash Values")]
    public float dashSpeed = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    private bool canDash = true;
    private bool isDashing;

    [Header("Components")]
    public Rigidbody2D rb;
    private Transform player;

    private bool jumpInput;
    private float moveInput;
    private Vector2 dashDirection;
    private bool isFacingRight = true;

    void Awake()
    {
        player = transform;
    }

    void Update()
    {
        if (IsGround() && jumpInput)
        {
            Jump(jumpPower);
        }

        isGround = IsGround();
        HandleInput();

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && moveInput != 0)
        {
            StartCoroutine(Dash());
        }

        // Flip the character based on movement direction
        if (moveInput > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (moveInput < 0 && isFacingRight)
        {
            Flip();
        }
    }

    void FixedUpdate()
    {
        if (!isDashing)
        {
            Walk(moveInput);
        }
    }

    private void Walk(float moveInput)
    {
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
    }

    private void Jump(float jumpPower)
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpPower);
    }

    private bool IsGround()
    {
        float playerHeight = player.GetComponent<BoxCollider2D>().size.y;
        Vector2 origin = player.position;
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, groundCheckDistance + playerHeight / 2, LayerMask.GetMask("Ground"));
        return hit.collider != null;
    }

    private void HandleInput()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        jumpInput = Input.GetButtonDown("Jump");

        if (moveInput != 0)
        {
            dashDirection = new Vector2(moveInput, 0).normalized;
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;

        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = dashDirection * dashSpeed;

        yield return new WaitForSeconds(dashDuration);

        rb.gravityScale = originalGravity;
        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    private void OnDrawGizmos()
    {
        if (player == null) return;

        Gizmos.color = Color.red;
        Vector2 origin = player.position;
        Vector2 direction = Vector2.down;
        float playerHeight = GetComponent<BoxCollider2D>() != null ? GetComponent<BoxCollider2D>().size.y : 1f;

        Gizmos.DrawLine(origin, origin + direction * (groundCheckDistance + playerHeight / 2));
    }
}
