using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachineCore.Player
{
    public abstract class PlayerBaseState : State
    {
        protected PlayerStateMachine stateMachine;

        public PlayerBaseState(PlayerStateMachine stateMachine)
        {
            this.stateMachine = stateMachine;
        }


        protected readonly int MovXHash = Animator.StringToHash("MovX");
        protected readonly int MovYHash = Animator.StringToHash("MovY");


        protected const float AnimatorDampTime = .1f;



    }
}



