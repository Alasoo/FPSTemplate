using StateMachineCore.Player;
using UnityEngine;


namespace Gameplay.Health
{
    public class HealthPickup : Pickup
    {
        [Header("Parameters")]
        [Tooltip("Amount of health to heal on pickup")]
        public float HealAmount;

        protected override void OnPicked()
        {
            if (PlayerStateMachine.Instance.m_Health.CanPickup())
            {
                PlayerStateMachine.Instance.m_Health.Heal(HealAmount);
                PlayPickupFeedback();
                Destroy(gameObject);
            }
        }
    }
}
