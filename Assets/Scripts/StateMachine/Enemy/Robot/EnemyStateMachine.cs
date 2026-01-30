using System;
using System.Collections;
using System.Collections.Generic;
using PoolSystem;
using Unity.FPS.AI;
using Unity.FPS.Game;
using UnityEngine;

namespace StateMachineCore.Enemy.Robot
{
    public class EnemyStateMachine : StateMachine
    {
        public Animator Animator;

        [Tooltip("Fraction of the enemy's attack range at which it will stop moving towards target while attacking")]
        [Range(0f, 1f)]
        public float AttackStopDistanceRatio = 0.5f;

        [Tooltip("The random hit damage effects")]
        public ParticleSystem[] RandomHitSparks;

        public ParticleSystem[] OnDetectVfx;
        public AudioClip OnDetectSfx;

        [Header("Sound")] public AudioClip MovementSound;
        public MinMaxFloat PitchDistortionMovementSpeed;

        public EnemyController m_EnemyController { get; private set; }
        public AudioSource m_AudioSource { get; private set; }

        Health m_Health;


        void Start()
        {
            if (m_EnemyController == null)
                m_EnemyController = GetComponent<EnemyController>();
            DebugUtility.HandleErrorIfNullGetComponent<EnemyController, EnemyMobile>(m_EnemyController, this,
                gameObject);

            m_EnemyController.SetPathDestinationToClosestNode();

            // adding a audio source to play the movement sound on it
            m_AudioSource = GetComponent<AudioSource>();
            DebugUtility.HandleErrorIfNullGetComponent<AudioSource, EnemyMobile>(m_AudioSource, this, gameObject);
            m_AudioSource.clip = MovementSound;
            m_AudioSource.Play();
        }

        void OnEnable()
        {
            if (m_Health == null)
                m_Health = GetComponent<Health>();
            if (m_EnemyController == null)
                m_EnemyController = GetComponent<EnemyController>();

            m_Health.OnDie += OnDie;
            SwitchState(new EnemyPatrollState(this));
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



