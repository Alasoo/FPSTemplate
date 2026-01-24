using UnityEngine;

namespace MenuStateMachineSystem
{
    public class OptionsMenuState : MenuState
    {

        [SerializeField] private MenuState SettingsMenuState;
        [SerializeField] private MenuState QuitMenuState;

        public void OnClickSettingsMenuState()
        {
            MenuStateMachine.Instance.ChangeMenu(SettingsMenuState);
        }
        public void OnClickQuitMenuState()
        {
            MenuStateMachine.Instance.ChangeMenu(QuitMenuState);
        }
        public void OnClickResume()
        {
            MenuStateMachine.Instance.GoBack();
        }
    }
}