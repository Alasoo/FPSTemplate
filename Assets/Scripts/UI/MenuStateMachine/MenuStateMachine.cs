using System;
using System.Collections.Generic;
using MyInputSystem;
using Unity.FPS.Gameplay;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MenuStateMachineSystem
{
    public class MenuStateMachine : Singleton<MenuStateMachine>, InputSystem_Actions.IUIActions
    {
        [field: Header("STATE MACHINE")]
        [SerializeField] List<MenuState> allStates = new();
        [SerializeField] private MenuState startMenuState;
        [SerializeField] private PlayerInputHandler m_InputHandler;

        private Stack<MenuState> menuHistory = new Stack<MenuState>();
        private MenuState currentState;
        private bool menuOpened = false;


        private InputSystem_Actions input;


        private void Start()
        {
            input = new();
            input.UI.SetCallbacks(this);
            input.UI.Enable();

            foreach (var menu in allStates)
            {
                menu.Init();
            }
        }

        private void OnDestroy()
        {
            if (input == null) return;
            input.UI.Disable();
        }




        public void OnMenu(InputAction.CallbackContext context)
        {
            Debug.Log($"OnMenu");
            if (context.started)
            {
                menuOpened = !menuOpened;
                OnMenuMode(menuOpened);
            }
        }



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
            menuOpened = false;
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }





        private void OnMenuMode(bool menuMode)
        {
            if (menuMode)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Time.timeScale = 0f;
                OpenMenu(startMenuState);
            }
            else
            {
                CloseAll();
            }
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
            }
        }

        public void OnNavigate(InputAction.CallbackContext context) { }

        public void OnSubmit(InputAction.CallbackContext context) { }

        public void OnCancel(InputAction.CallbackContext context) { }

        public void OnPoint(InputAction.CallbackContext context) { }

        public void OnClick(InputAction.CallbackContext context) { }

        public void OnRightClick(InputAction.CallbackContext context) { }

        public void OnMiddleClick(InputAction.CallbackContext context) { }

        public void OnScrollWheel(InputAction.CallbackContext context) { }

        public void OnTrackedDevicePosition(InputAction.CallbackContext context) { }

        public void OnTrackedDeviceOrientation(InputAction.CallbackContext context) { }
    }
}
