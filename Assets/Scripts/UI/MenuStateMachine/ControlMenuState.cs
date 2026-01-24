using MyUI;
using UnityEngine;
using UnityEngine.UI;

namespace MenuStateMachineSystem
{
    public class ControlMenuState : MenuState
    {
        [Header("BUTTON")]
        [SerializeField] private Button controlButton;    //CustomButton

        public override void Enter()
        {
            base.Enter();
            //controlButton.Select(true);
        }
        public override void Exit()
        {
            base.Exit();
            //controlButton.Select(false);
        }
    }
}