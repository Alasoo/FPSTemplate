using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace StateMachineCore.Enemy.Robot
{
    public class EnemyAttackState : EnemyBaseState
    {
        public EnemyAttackState(EnemyStateMachine stateMachine) : base(stateMachine) { }


        public override void Enter()
        {
            stateMachine.m_EnemyController.SetNavDestination(stateMachine.transform.position);
            stateMachine.m_EnemyController.onDamaged += OnDamaged;
            stateMachine.Animator.SetTrigger(k_AnimAttackParameter);
        }


        public override void Tick(float deltaTime)
        {
            // Transition to follow when no longer a target in attack range
            if (!stateMachine.m_EnemyController.IsTargetInAttackRange)
            {
                stateMachine.SwitchState(new EnemyFollowState(stateMachine));
                return;
            }

            if (Vector3.Distance(stateMachine.m_EnemyController.KnownDetectedTarget.transform.position,
                    stateMachine.m_EnemyController.DetectionModule.DetectionSourcePoint.position)
                >= (stateMachine.AttackStopDistanceRatio * stateMachine.m_EnemyController.DetectionModule.AttackRange))
            {
                stateMachine.m_EnemyController.SetNavDestination(stateMachine.m_EnemyController.KnownDetectedTarget.transform.position);
            }
            else
            {
                stateMachine.m_EnemyController.SetNavDestination(stateMachine.transform.position);
            }

            stateMachine.m_EnemyController.OrientTowards(stateMachine.m_EnemyController.KnownDetectedTarget.transform.position);
            stateMachine.m_EnemyController.TryAtack(stateMachine.m_EnemyController.KnownDetectedTarget.transform.position);

            Move();
        }



        public override void Exit()
        {
            stateMachine.m_EnemyController.onDamaged -= OnDamaged;

        }


    }

}

