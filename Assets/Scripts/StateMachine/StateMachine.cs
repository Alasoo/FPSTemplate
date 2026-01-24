using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace StateMachineCore
{
    public abstract class StateMachine : MonoBehaviour
    {
        private State currentState = null;

        public void SwitchState(State newState)
        {
            currentState?.Exit();
            currentState = newState;
            currentState?.Enter();
        }

        private void Update()
        {
            currentState?.Tick(Time.deltaTime);
        }

        private void FixedUpdate()
        {
            currentState?.FixedTick(Time.fixedDeltaTime);
        }
    }
}



