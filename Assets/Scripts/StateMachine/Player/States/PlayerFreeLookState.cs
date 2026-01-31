using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace StateMachineCore.Player
{
    public class PlayerFreeLookState : PlayerBaseState
    {
        public PlayerFreeLookState(PlayerStateMachine stateMachine) : base(stateMachine) { }


        public override void Enter() { }


        public override void Tick(float deltaTime)
        {
            BasicCheck(deltaTime);

            if (stateMachine.m_InputHandler.GetCrouchInputDown())
            {
                if (SetCrouchingState(!stateMachine.isCrouching, false))
                {
                    stateMachine.SwitchState(new PlayerCrouchState(stateMachine));
                    return;
                }
            }

            if (stateMachine.m_InputHandler.GetJumpInputDown())
            {
                stateMachine.SwitchState(new PlayerJumpState(stateMachine));
                return;
            }


            UpdateCharacterHeight(false, deltaTime);
            HandleCharacterMovement(deltaTime);
        }



        public override void Exit(){    }

    }

}

