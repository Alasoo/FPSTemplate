using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace StateMachineCore.Player
{
    public class PlayerCrouchState : PlayerBaseState
    {
        private readonly int FreeLookBlendTreeHash = Animator.StringToHash("FreeLookBlendTree");
        public PlayerCrouchState(PlayerStateMachine stateMachine) : base(stateMachine) { }


        public override void Enter(){}



        public override void Tick(float deltaTime)
        {
            bool isSprinting = stateMachine.m_InputHandler.GetSprintInputHeld();
            if (isSprinting)
            {
                stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
                return;
            }

            CheckZKill();
            stateMachine.HasJumpedThisFrame = false;

            bool wasGrounded = stateMachine.isGrounded;
            GroundCheck(deltaTime);

            if (stateMachine.isGrounded && !wasGrounded)
            {
                // Fall damage
                float fallSpeed = -Mathf.Min(stateMachine.CharacterVelocity.y, stateMachine.m_LatestImpactSpeed.y);
                float fallSpeedRatio = (fallSpeed - stateMachine.MinSpeedForFallDamage) /
                                       (stateMachine.MaxSpeedForFallDamage - stateMachine.MinSpeedForFallDamage);
                if (stateMachine.RecievesFallDamage && fallSpeedRatio > 0f)
                {
                    float dmgFromFall = Mathf.Lerp(stateMachine.FallDamageAtMinSpeed, stateMachine.FallDamageAtMaxSpeed, fallSpeedRatio);
                    stateMachine.m_Health.TakeDamage(dmgFromFall, null);

                    // fall damage SFX
                    stateMachine.AudioSource.PlayOneShot(stateMachine.FallDamageSfx);
                }
                else
                {
                    // land SFX
                    stateMachine.AudioSource.PlayOneShot(stateMachine.LandSfx);
                }
            }

            if (stateMachine.m_InputHandler.GetCrouchInputDown())
            {
                if (SetCrouchingState(!stateMachine.isCrouching, false))
                {
                    stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
                    return;
                }
            }

            UpdateCharacterHeight(false, deltaTime);
            HandleCharacterMovement(deltaTime);
        }



        public override void Exit(){}


    }

}

