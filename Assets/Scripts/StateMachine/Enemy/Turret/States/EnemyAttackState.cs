using System.Collections;
using System.Collections.Generic;
using Unity.FPS.Game;
using UnityEngine;


namespace StateMachineCore.Enemy.Turret
{
    public class EnemyAttackState : EnemyBaseState
    {
        public EnemyAttackState(EnemyStateMachine stateMachine) : base(stateMachine) { }


        public override void Enter()
        {
            stateMachine.m_EnemyController.onDamaged += OnDamaged;
            stateMachine.m_EnemyController.onLostTarget += OnLostTarget;
            for (int i = 0; i < stateMachine.OnDetectVfx.Length; i++)
            {
                stateMachine.OnDetectVfx[i].Play();
            }

            if (stateMachine.OnDetectSfx)
            {
                AudioUtility.CreateSFX(stateMachine.OnDetectSfx, stateMachine.transform.position, AudioUtility.AudioGroups.EnemyDetection, 1f);
            }

            stateMachine.Animator.SetBool(k_AnimIsActiveParameter, true);
            stateMachine.m_TimeStartedDetection = Time.time;

        }


        public override void Tick(float deltaTime)
        {
            if (stateMachine.m_EnemyController.KnownDetectedTarget == null)
            {
                stateMachine.SwitchState(new EnemyIdleState(stateMachine));
                return;
            }
            bool mustShoot = Time.time > stateMachine.m_TimeStartedDetection + stateMachine.DetectionFireDelay;
            // Calculate the desired rotation of our turret (aim at target)
            Vector3 directionToTarget =
                (stateMachine.m_EnemyController.KnownDetectedTarget.transform.position - stateMachine.TurretAimPoint.position).normalized;
            Quaternion offsettedTargetRotation =
                Quaternion.LookRotation(directionToTarget) * stateMachine.m_RotationWeaponForwardToPivot;
            stateMachine.m_PivotAimingRotation = Quaternion.Slerp(stateMachine.m_PreviousPivotAimingRotation, offsettedTargetRotation,
                (mustShoot ? stateMachine.AimRotationSharpness : stateMachine.LookAtRotationSharpness) * Time.deltaTime);

            // shoot
            if (mustShoot)
            {
                Vector3 correctedDirectionToTarget =
                    (stateMachine.m_PivotAimingRotation * Quaternion.Inverse(stateMachine.m_RotationWeaponForwardToPivot)) *
                    Vector3.forward;

                stateMachine.m_EnemyController.TryAtack(stateMachine.TurretAimPoint.position + correctedDirectionToTarget);
            }

        }

        public override void LateTick(float deltaTime)
        {
            stateMachine.TurretPivot.rotation = stateMachine.m_PivotAimingRotation;
            stateMachine.m_PreviousPivotAimingRotation = stateMachine.TurretPivot.rotation;
        }


        public override void Exit()
        {
            stateMachine.m_EnemyController.onDamaged -= OnDamaged;
            stateMachine.m_EnemyController.onLostTarget -= OnLostTarget;
            stateMachine.Animator.SetBool(k_AnimIsActiveParameter, false);
        }

        private void OnLostTarget()
        {
            stateMachine.SwitchState(new EnemyIdleState(stateMachine));
        }

    }

}

