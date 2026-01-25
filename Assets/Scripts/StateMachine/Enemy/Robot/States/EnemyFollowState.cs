using System.Collections;
using System.Collections.Generic;
using Unity.FPS.Game;
using UnityEngine;


namespace StateMachineCore.Enemy.Robot
{
    public class EnemyFollowState : EnemyBaseState
    {
        public EnemyFollowState(EnemyStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {

            stateMachine.m_EnemyController.onLostTarget += OnLostTarget;
            stateMachine.m_EnemyController.onDamaged += OnDamaged;

            for (int i = 0; i < stateMachine.OnDetectVfx.Length; i++)
            {
                stateMachine.OnDetectVfx[i].Play();
            }

            if (stateMachine.OnDetectSfx)
            {
                AudioUtility.CreateSFX(stateMachine.OnDetectSfx, stateMachine.transform.position, AudioUtility.AudioGroups.EnemyDetection, 1f);
            }

            stateMachine.Animator.SetBool(k_AnimAlertedParameter, true);
            
        }

        public override void Tick(float deltaTime)
        {
            if (stateMachine.m_EnemyController.IsSeeingTarget && stateMachine.m_EnemyController.IsTargetInAttackRange)
            {
                stateMachine.SwitchState(new EnemyAttackState(stateMachine));
                return;
            }

            stateMachine.m_EnemyController.SetNavDestination(stateMachine.m_EnemyController.KnownDetectedTarget.transform.position);
            stateMachine.m_EnemyController.OrientTowards(stateMachine.m_EnemyController.KnownDetectedTarget.transform.position);
            stateMachine.m_EnemyController.OrientWeaponsTowards(stateMachine.m_EnemyController.KnownDetectedTarget.transform.position);

            Move();
        }

        public override void Exit()
        {
            stateMachine.m_EnemyController.onLostTarget -= OnLostTarget;
            stateMachine.m_EnemyController.onDamaged -= OnDamaged;
            stateMachine.Animator.SetBool(k_AnimAlertedParameter, false);
        }


        private void OnLostTarget()
        {
            stateMachine.SwitchState(new EnemyPatrollState(stateMachine));
        }


    }

}

