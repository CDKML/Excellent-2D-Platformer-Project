using Platformer.Gameplay;
using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static Platformer.Mechanics.PlayerController;

public class ColliderController : MonoBehaviour
{
    [SerializeField] private BoxCollider2D _rigidBody2d;
    [SerializeField] private PlayerController _playerController;

    private Vector2 colliderJumpingUp = new Vector2(0.2f, 1);
    private Vector2 colliderMoving = new Vector2(1f, 0.6f);
    private Vector2 colliderFalling = new Vector2(1f, 1f);
    private Vector2 colliderGrounded = new Vector2(1f, 1f);
    BoxCollider2D boxCollider2D;
    public JumpState jumpState;
    float timeElapsed = 0f;
    public float interpolationDuration = 0.25f;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider2D = _rigidBody2d.GetComponent<BoxCollider2D>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        jumpState = _playerController.GetJumpState();
        switch (jumpState)
        {
            case JumpState.Jumping:
                if (!_playerController.IsGrounded)
                {
                    boxCollider2D.size = colliderJumpingUp;
                    LerpCollider(colliderGrounded, colliderJumpingUp);
                }
                break;
            case JumpState.Falling:
                //timeElapsed = 0f;
                boxCollider2D.size = colliderFalling;
                LerpCollider(colliderJumpingUp, colliderFalling);

                break;
            case JumpState.Landed:
                //timeElapsed = 0f;
                boxCollider2D.size = colliderGrounded;
                LerpCollider(colliderFalling, colliderGrounded);
                break;
        }
    }

    private void LerpCollider(Vector2 startCollider, Vector2 endCollider)
    {

        float perc = timeElapsed / interpolationDuration; // Calculate the percentage of the jump duration that has passed
        perc = Mathf.Clamp01(perc); // Clamp the value to ensure it stays between 0 and 1
            
        Vector3 targetSize = startCollider * endCollider; // Calculate the target collider size
        transform.localScale = Vector3.Lerp(startCollider, endCollider, perc); // Interpolate between the current size and the target size
        boxCollider2D.size = Vector2.Lerp(startCollider, endCollider, perc);

        //// Increment the elapsed time by the time since the last frame
        timeElapsed += Time.deltaTime;

        //// Calculate the normalized time between 0 and 1 based on the elapsed time and the duration
        //float t = Mathf.Clamp01(timeElapsed / interpolationDuration);

        //// Interpolate the size of the collider based on the start and end colliders and the normalized time
        //boxCollider2D.size = Vector2.Lerp(startCollider, endCollider, t);

        // Check if the interpolation is complete and disable this script if it is
        if (timeElapsed >= interpolationDuration)
        {
            timeElapsed = 0f;
        }
    }
}
