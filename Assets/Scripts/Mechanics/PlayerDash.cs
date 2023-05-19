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

        Vector2 currentDirection = moveAction.action.ReadValue<Vector2>().normalized; // Reads the current movement direction.

        Vector2 dashVelocity = currentDirection * dashSpeed;

        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        RaycastHit2D hit = Physics2D.BoxCast(rb.position, boxCollider.size, 0f, currentDirection, dashSpeed * dashDuration, dashCollisionLayers);

        if (hit.collider != null)
        {
            // If there is a collision in the direction of the dash, end the dash at the collision point
            rb.position = hit.point;
            rb.velocity = Vector2.zero;
        }
        else
        {
            // If there is no collision, perform the dash normally
            rb.velocity = dashVelocity;
            var originalGravity = rb.gravityScale;
            rb.gravityScale = 0f;
            yield return new WaitForSeconds(dashDuration);
            rb.velocity = Vector2.zero;
            rb.gravityScale = originalGravity;

        }

        isDashing = false;
        lastDashTime = Time.time;
    }
}
