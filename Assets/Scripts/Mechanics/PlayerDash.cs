using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDash : MonoBehaviour
{
    public float dashSpeed = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;

    private Rigidbody2D rb;
    private float lastDashTime = -Mathf.Infinity;
    public bool isDashing;

    [SerializeField] InputActionReference dashAction;
    [SerializeField] InputActionReference moveAction;

    public LayerMask dashCollisionLayers; // Assign this in the Inspector

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        // Hook up Dash action
        dashAction.action.performed += _ => { if (Time.time >= lastDashTime + dashCooldown) StartCoroutine(PerformDash()); };
    }

    private IEnumerator PerformDash()
    {
        isDashing = true;

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Vector2 dashDirection = spriteRenderer.flipX ? Vector2.left : Vector2.right;

        Vector2 dashVelocity = dashDirection * dashSpeed;

        rb.velocity = dashVelocity;
        var originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        yield return new WaitForSeconds(dashDuration);
        rb.velocity = Vector2.zero;
        rb.gravityScale = originalGravity;

        isDashing = false;
        lastDashTime = Time.time;
    }
}
