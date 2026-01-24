using System.Collections;
using System.Collections.Generic;
using Unity.FPS.Game;
using UnityEngine;


namespace StateMachineCore.Player
{
    public class PlayerDieState : PlayerBaseState
    {
        public PlayerDieState(PlayerStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            stateMachine.IsDead = true;
            // Tell the weapons manager to switch to a non-existing weapon in order to lower the weapon
            stateMachine.m_WeaponsManager.SwitchToWeaponIndex(-1, true);
            EventManager.Broadcast(Events.PlayerDeathEvent);
        }

        public override void Tick(float deltaTime){}

        public override void Exit(){}
    }

}

