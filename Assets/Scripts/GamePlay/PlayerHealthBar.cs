using StateMachineCore.Player;
using Unity.FPS.Game;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerHealthBarCore
{
    public class PlayerHealthBar : MonoBehaviour
    {
        [Tooltip("Image component dispplaying current health")]
        public Image HealthFillImage;

        Health m_PlayerHealth;

        void Start()
        {
            DebugUtility.HandleErrorIfNullFindObject<PlayerStateMachine, PlayerHealthBar>(
                PlayerStateMachine.Instance, this);

            m_PlayerHealth = PlayerStateMachine.Instance.GetComponent<Health>();
            DebugUtility.HandleErrorIfNullGetComponent<Health, PlayerHealthBar>(m_PlayerHealth, this,
                PlayerStateMachine.Instance.gameObject);
        }

        void Update()
        {
            // update health bar value
            HealthFillImage.fillAmount = m_PlayerHealth.CurrentHealth / m_PlayerHealth.MaxHealth;
        }
    }
}