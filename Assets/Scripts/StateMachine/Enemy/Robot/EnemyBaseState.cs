using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachineCore.Enemy.Robot
{
    public abstract class EnemyBaseState : State
    {
        protected EnemyStateMachine stateMachine;

        public EnemyBaseState(EnemyStateMachine stateMachine)
        {
            this.stateMachine = stateMachine;
        }


        protected const string k_AnimMoveSpeedParameter = "MoveSpeed";
        protected const string k_AnimAttackParameter = "Attack";
        protected const string k_AnimAlertedParameter = "Alerted";
        protected const string k_AnimOnDamagedParameter = "OnDamaged";


        protected void OnDamaged()
        {
            if (stateMachine.RandomHitSparks.Length > 0)
            {
                int n = Random.Range(0, stateMachine.RandomHitSparks.Length - 1);
                stateMachine.RandomHitSparks[n].Play();
            }

            stateMachine.Animator.SetTrigger(k_AnimOnDamagedParameter);
        }


        protected void Move()
        {
            float moveSpeed = stateMachine.m_EnemyController.NavMeshAgent.velocity.magnitude;

            // Update animator speed parameter
            stateMachine.Animator.SetFloat(k_AnimMoveSpeedParameter, moveSpeed);

            // changing the pitch of the movement sound depending on the movement speed
            stateMachine.m_AudioSource.pitch = Mathf.Lerp(stateMachine.PitchDistortionMovementSpeed.Min, stateMachine.PitchDistortionMovementSpeed.Max,
                moveSpeed / stateMachine.m_EnemyController.NavMeshAgent.speed);
        }
    }
}



