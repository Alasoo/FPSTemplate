using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MyInputSystem
{
    [RequireComponent(typeof(PlayerInput))]
    public class InputSystem : Singleton<InputSystem>
    {
        private PlayerInput playerInput;
        public InputReader inputReader { get; private set; }
        public event Action<bool> OnMenuMode;

        protected override void Awake()
        {
            base.Awake();
            playerInput = GetComponent<PlayerInput>();
            inputReader = GetComponent<InputReader>();
        }

        void Start()
        {
            StartCoroutine(EnablePlayerInputAfterDelay());
        }


        private IEnumerator EnablePlayerInputAfterDelay()
        {
            yield return null; // Espera un frame
            playerInput.SwitchCurrentActionMap("UI");
            playerInput.SwitchCurrentActionMap("Player");
        }




        [ContextMenu("Player mode")]
        public void SetPlayerControls()
        {
            playerInput.SwitchCurrentActionMap("Player");
            OnMenuMode?.Invoke(false);
        }

        [ContextMenu("UI mode")]
        public void SetUIControls()
        {
            playerInput.SwitchCurrentActionMap("UI");
            OnMenuMode?.Invoke(true);
        }

    }
}
