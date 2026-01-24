using MyInputSystem;
using UnityEngine;

namespace MenuStateMachineSystem
{
    public class SettingsMenuState : MenuState
    {
        [Header("SUBMENU")]
        [SerializeField] private MenuState VideoMenuState;
        [SerializeField] private MenuState AudioMenuState;
        [SerializeField] private MenuState ControlMenuState;

        private Vector2 startPos;
        private MenuState currentMenu;



        public void OnClickVideoMenuState()
        {
            currentMenu?.Exit();
            currentMenu = VideoMenuState;
            currentMenu?.Enter();
        }
        public void OnClickAudioMenuState()
        {
            currentMenu?.Exit();
            currentMenu = AudioMenuState;
            currentMenu?.Enter();
        }
        public void OnClickControlMenuState()
        {
            currentMenu?.Exit();
            currentMenu = ControlMenuState;
            currentMenu?.Enter();
        }

        public void OnClickCloseSettings()
        {
            MenuStateMachine.Instance.CloseAll();
            InputSystem.Instance.SetPlayerControls();
        }


        public override void Init()
        {
            VideoMenuState?.Exit();
            AudioMenuState?.Exit();
            ControlMenuState?.Exit();
            base.Init();
            startPos = transform.position;
            currentMenu = VideoMenuState;
        }


        public override void Enter()
        {
            transform.position = startPos;
            base.Enter();
            currentMenu?.Enter();
        }


    }
}
