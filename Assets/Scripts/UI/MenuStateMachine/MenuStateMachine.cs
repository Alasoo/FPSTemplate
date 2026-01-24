using System.Collections.Generic;
using MyInputSystem;
using UnityEngine;

namespace MenuStateMachineSystem
{
    public class MenuStateMachine : Singleton<MenuStateMachine>
    {
        [field: Header("STATE MACHINE")]
        [SerializeField] List<MenuState> allStates = new();
        [SerializeField] private MenuState startMenuState;

        private Stack<MenuState> menuHistory = new Stack<MenuState>();
        private MenuState currentState;


        public void OpenMenu(MenuState _currentState)
        {
            currentState = _currentState;
            currentState?.Enter();
        }


        public void CloseAll()
        {
            foreach (var menu in menuHistory)
            {
                menu?.ForceClose();
            }
            menuHistory.Clear();
            currentState?.ForceClose();
            currentState = null;
        }

        void Start()
        {
            foreach (var menu in allStates)
            {
                menu.Init();
            }
        }



        private void OnMenuMode(bool menuMode)
        {
            if (menuMode)
                OpenMenu(startMenuState);
            else
                CloseAll();
        }


        public void ChangeMenu(MenuState newMenu)
        {
            if (currentState != null)
            {
                menuHistory.Push(currentState); // Guardar el menú actual antes de cambiar
                currentState.Exit();
            }

            currentState = newMenu;
            currentState?.Enter();
            Debug.Log($"ChangeMenu Enter: {currentState.transform.name}");
        }
        public void AddMenu(MenuState newMenu)
        {
            if (currentState != null)
            {
                menuHistory.Push(currentState); // Guardar el menú actual antes de cambiar
                //currentState.Exit();
            }

            currentState = newMenu;
            currentState?.Enter();
            Debug.Log($"ChangeMenu Enter: {currentState.transform.name}");
        }


        public void GoBack()
        {
            Debug.Log($"GoBack");

            if (menuHistory.Count > 0)
            {
                MenuState previousMenu = menuHistory.Pop(); // Recuperar el menú anterior
                currentState.Exit();
                currentState = previousMenu;
                currentState.Enter();
            }

            else
            {
                Debug.Log($"GoBack SetPlayerControls");
                InputSystem.Instance.SetPlayerControls();
            }

        }
    }
}
