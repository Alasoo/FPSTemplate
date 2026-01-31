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

        public void OnClickVideoMenuState()
        {
            MenuStateMachine.Instance.ChangeMenu(VideoMenuState);
        }
        public void OnClickAudioMenuState()
        {
            MenuStateMachine.Instance.ChangeMenu(AudioMenuState);
        }
        public void OnClickControlMenuState()
        {
            MenuStateMachine.Instance.ChangeMenu(ControlMenuState);
        }

        public void OnClickQuit()
        {
            Application.Quit();
        }
    }
}
