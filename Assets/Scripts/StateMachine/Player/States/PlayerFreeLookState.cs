using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace StateMachineCore.Player
{
    public class PlayerFreeLookState : PlayerBaseState
    {
        private readonly int FreeLookBlendTreeHash = Animator.StringToHash("FreeLookBlendTree");
        public PlayerFreeLookState(PlayerStateMachine stateMachine) : base(stateMachine) { }


        public override void Enter()
        {
            // force the crouch state to false when starting
            SetCrouchingState(false, true);
            UpdateCharacterHeight(true, Time.deltaTime);
        }



        public override void Tick(float deltaTime)
        {
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
                SetCrouchingState(!stateMachine.isCrouching, false);
            }

            UpdateCharacterHeight(false, deltaTime);

            HandleCharacterMovement(deltaTime);
        }



        public override void Exit()
        {
            //stateMachine.InputReader.TargetEvent -= OnTarget;
            //stateMachine.InputReader.JumpEvent -= OnJump;
            //stateMachine.InputReader.DancePanelEvent -= OnDancePanel;
        }



        private void OnJump()
        {
            //if (stateMachine.ForceReceiver.MySlopeAngle > stateMachine.Controller.slopeLimit) return;
            //stateMachine.SwitchState(new PlayerJumpingState(stateMachine));
        }


        private void OnFalling()
        {
            //stateMachine.SwitchState(new PlayerFallingState(stateMachine));
        }

    }

}

