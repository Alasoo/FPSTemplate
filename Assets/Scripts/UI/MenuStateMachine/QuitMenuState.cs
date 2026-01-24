using UnityEngine;

namespace MenuStateMachineSystem
{
    public class QuitMenuState : MenuState
    {

        public void OnClickQuitApp()
        {
            Debug.Log($"Quit game");
            Application.Quit();
        }
    }
}
