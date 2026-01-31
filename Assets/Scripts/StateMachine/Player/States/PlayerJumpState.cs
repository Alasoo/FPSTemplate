using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace StateMachineCore.Player
{
    public class PlayerJumpState : PlayerBaseState
    {
        public PlayerJumpState(PlayerStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            Jump();
        }


        public override void Tick(float deltaTime)
        {
            BasicCheck(deltaTime);

            if (stateMachine.isGrounded)
            {
                stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
                return;
            }

            UpdateCharacterHeight(false, deltaTime);
            HandleCharacterMovement(deltaTime);
        }


        public override void Exit(){    }
    }

}

