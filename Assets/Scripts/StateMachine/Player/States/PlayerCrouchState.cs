using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace StateMachineCore.Player
{
    public class PlayerCrouchState : PlayerBaseState
    {
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

            BasicCheck(deltaTime);


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

