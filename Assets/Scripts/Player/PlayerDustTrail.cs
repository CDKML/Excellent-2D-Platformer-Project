using Platformer.Mechanics;
using UnityEngine;

namespace Assets.Scripts
{
    public class PlayerDustTrail : MonoBehaviour
    {
        public ParticleSystem dustTrail;
        public float speedThreshold = 0.1f; // The speed above which the player is considered to be moving

        PlayerController playerController;
        Rigidbody2D rb;

        void Awake()
        {
            playerController = GetComponent<PlayerController>();
            rb = GetComponent<Rigidbody2D>();
        }

        void Update()
        {
            // Emit particles when the player is moving
            //if (playerController.velocity.magnitude > speedThreshold)
            if (rb.velocity.magnitude > speedThreshold)
            {
                if (!dustTrail.isPlaying)
                {
                    dustTrail.Play();
                }
            }
            else
            {
                if (dustTrail.isPlaying)
                {
                    dustTrail.Stop();
                }
            }
        }
    }

}
