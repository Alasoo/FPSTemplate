using System;
using System.Collections;
using System.Collections.Generic;
using PoolSystem;
using Unity.FPS.AI;
using Unity.FPS.Game;
using UnityEngine;

namespace StateMachineCore.Enemy.Turret
{
    public class EnemyStateMachine : StateMachine
    {
        public Transform TurretPivot;
        public Transform TurretAimPoint;
        public Animator Animator;
        public float AimRotationSharpness = 5f;
        public float LookAtRotationSharpness = 2.5f;
        public float DetectionFireDelay = 1f;
        public float AimingTransitionBlendTime = 1f;

        [Tooltip("The random hit damage effects")]
        public ParticleSystem[] RandomHitSparks;

        public ParticleSystem[] OnDetectVfx;
        public AudioClip OnDetectSfx;

        public EnemyController m_EnemyController { get; private set; }
        Health m_Health;
        public Quaternion m_RotationWeaponForwardToPivot { get; set; }
        public float m_TimeStartedDetection { get; set; }
        public float m_TimeLostDetection { get; set; }
        public Quaternion m_PreviousPivotAimingRotation { get; set; }
        public Quaternion m_PivotAimingRotation { get; set; }


        void Start()
        {
            if (m_Health == null)
                m_Health = GetComponent<Health>();
            DebugUtility.HandleErrorIfNullGetComponent<Health, EnemyTurret>(m_Health, this, gameObject);

            if (m_EnemyController == null)
                m_EnemyController = GetComponent<EnemyController>();
            DebugUtility.HandleErrorIfNullGetComponent<EnemyController, EnemyTurret>(m_EnemyController, this,
                gameObject);

            // Remember the rotation offset between the pivot's forward and the weapon's forward
            m_RotationWeaponForwardToPivot =
                Quaternion.Inverse(m_EnemyController.GetCurrentWeapon().WeaponMuzzle.rotation) * TurretPivot.rotation;

            m_TimeStartedDetection = Mathf.NegativeInfinity;
            m_PreviousPivotAimingRotation = TurretPivot.rotation;
        }

        void OnEnable()
        {
            if (m_Health == null)
                m_Health = GetComponent<Health>();
            if (m_EnemyController == null)
                m_EnemyController = GetComponent<EnemyController>();

            m_Health.OnDie += OnDie;
            SwitchState(new EnemyAttackState(this));
        }

        void OnDisable()
        {
            m_Health.OnDie -= OnDie;
            m_Health.Reset();
        }

        private void OnDie()
        {
            EnemyPool.Instance.Return(this);
        }

    }
}



