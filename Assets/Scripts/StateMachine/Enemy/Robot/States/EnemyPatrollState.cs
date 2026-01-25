using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace StateMachineCore.Enemy.Robot
{
    public class EnemyPatrollState : EnemyBaseState
    {
        public EnemyPatrollState(EnemyStateMachine stateMachine) : base(stateMachine) { }


        public override void Enter()
        {
            stateMachine.m_EnemyController.onDetectedTarget += OnDetectedTarget;
            stateMachine.m_EnemyController.onDamaged += OnDamaged;
        }



        public override void Tick(float deltaTime)
        {
            stateMachine.m_EnemyController.UpdatePathDestination();
            stateMachine.m_EnemyController.SetNavDestination(stateMachine.m_EnemyController.GetDestinationOnPath());

            Move();
        }



        public override void Exit()
        {
            stateMachine.m_EnemyController.onDetectedTarget -= OnDetectedTarget;
            stateMachine.m_EnemyController.onDamaged -= OnDamaged;
        }


        private void OnDetectedTarget()
        {
            stateMachine.SwitchState(new EnemyFollowState(stateMachine));
        }

    }

}

