using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace StateMachineCore.Enemy.Turret
{
    public class EnemyIdleState : EnemyBaseState
    {
        public EnemyIdleState(EnemyStateMachine stateMachine) : base(stateMachine) { }


        public override void Enter()
        {

            for (int i = 0; i < stateMachine.OnDetectVfx.Length; i++)
            {
                stateMachine.OnDetectVfx[i].Stop();
            }

            stateMachine.Animator.SetBool(k_AnimIsActiveParameter, false);
            stateMachine.m_TimeLostDetection = Time.time;

            stateMachine.m_EnemyController.onDetectedTarget += OnDetectedTarget;
            stateMachine.m_EnemyController.onDamaged += OnDamaged;
        }



        public override void Tick(float deltaTime)
        {
             stateMachine.TurretPivot.rotation = Quaternion.Slerp(stateMachine.m_PivotAimingRotation, stateMachine.TurretPivot.rotation,
                        (Time.time - stateMachine.m_TimeLostDetection) / stateMachine.AimingTransitionBlendTime);
           
            stateMachine.m_PreviousPivotAimingRotation = stateMachine.TurretPivot.rotation;

        }



        public override void Exit()
        {
            stateMachine.m_EnemyController.onDetectedTarget -= OnDetectedTarget;
            stateMachine.m_EnemyController.onDamaged -= OnDamaged;
        }


        private void OnDetectedTarget()
        {
            stateMachine.SwitchState(new EnemyAttackState(stateMachine));
        }

    }

}

