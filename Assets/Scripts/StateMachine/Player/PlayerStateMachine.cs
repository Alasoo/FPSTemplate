using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace StateMachineCore.Player
{
    public class PlayerStateMachine : StateMachine
    {
        [field: SerializeField] public CharacterController Controller { get; private set; }
        [field: SerializeField] public Animator Animator { get; private set; }
        [field: SerializeField] public float FreeLookMovementSpeed { get; private set; }
        [field: SerializeField] public float RotationDamping { get; private set; }
        [field: SerializeField] public float DodgeDuration { get; private set; }
        [field: SerializeField] public float DodgeLength { get; private set; }
        [field: SerializeField] public float JumpForce { get; private set; }


        [field: SerializeField] public Transform SlotsAbilitiesContent { get; private set; }
        [field: SerializeField] public List<Outline> SlotsAbilities = new();


        [HideInInspector] public int CurrentSlotSelected;
        public float PreviousDodgeTime { get; private set; } = Mathf.NegativeInfinity;
        public Transform MainCameraTransform { get; private set; }





        private void Start()
        {
            //SwitchState(new PlayerFreeLookState(this));


        }




        private void OnEnable()
        {
        }




        private void OnDisable()
        {
        }
    }
}



