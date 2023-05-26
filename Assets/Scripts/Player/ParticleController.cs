using Assets.Scripts.Enums;
using Platformer.Mechanics;
using UnityEngine;

namespace Assets.Scripts
{
    public class ParticleController : MonoBehaviour
    {
        [SerializeField] ParticleSystem movementParticle;
        [SerializeField] ParticleSystem fallParticle;
        [SerializeField] ParticleSystem dashParticle;

        //PlayerController playerController;
        [SerializeField] GameObject playerGO;
        PlayerController playerController;
        PlayerDash playerDash;

        bool alreadyDashed;

        [Range(0f, 10f)]
        [SerializeField] float speedThreshold = 0.1f;

        [Range(0f, 0.2f)]
        [SerializeField] float dustFormationPeriod;

        float counter;

        void Awake()
        {
            playerController = playerGO.GetComponent<PlayerController>();
            playerDash = playerGO.GetComponent<PlayerDash>();
        }

        void Update()
        {
            counter += Time.deltaTime;

            // Emit particles when the player is moving
            if (playerController.jumpState == JumpStateEnum.Grounded && Mathf.Abs(playerController.velocity.x) > speedThreshold)
            {
                if (counter > dustFormationPeriod)
                {
                    movementParticle.Play();
                    counter = 0;
                }
            }

            if (playerController.jumpState == JumpStateEnum.Landed)
            {
                fallParticle.Play();
            }

            if (playerDash.isDashing && !alreadyDashed)
            {
                alreadyDashed = true;
                dashParticle.Play();
            }
            else if (!playerDash.isDashing && alreadyDashed)
            {
                alreadyDashed = false;
            }
        }
    }
}
