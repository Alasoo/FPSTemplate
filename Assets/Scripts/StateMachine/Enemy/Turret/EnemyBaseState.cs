using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachineCore.Enemy.Turret
{
    public abstract class EnemyBaseState : State
    {
        protected EnemyStateMachine stateMachine;

        public EnemyBaseState(EnemyStateMachine stateMachine)
        {
            this.stateMachine = stateMachine;
        }

        protected const string k_AnimOnDamagedParameter = "OnDamaged";
        protected const string k_AnimIsActiveParameter = "IsActive";


        protected void OnDamaged()
        {
            if (stateMachine.RandomHitSparks.Length > 0)
            {
                int n = Random.Range(0, stateMachine.RandomHitSparks.Length - 1);
                stateMachine.RandomHitSparks[n].Play();
            }

            stateMachine.Animator.SetTrigger(k_AnimOnDamagedParameter);
        }


    }
}



