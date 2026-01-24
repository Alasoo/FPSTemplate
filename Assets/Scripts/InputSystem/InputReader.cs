using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class InputReader : MonoBehaviour
{
    public Vector2 movementValue { get; private set; }
    public Vector2 lookValue { get; private set; }
    public Vector2 scrollValue { get; private set; }
    public bool aiming { get; private set; }
    public bool sprint { get; private set; } = true;
    public bool jump { get; set; }
    public bool attacking { get; set; }


    public event Action<bool> OnAimEvent;
    public event Action OnAttackEvent;
    //public event Action OnJumpEvent;
    public event Action OnCancelMenuEvent;
    public event Action OnCancelGameEvent;



    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            jump = true;
        }
        //if (context.performed) return;
        //OnJumpEvent?.Invoke();
    }


    public void OnLook(InputAction.CallbackContext context)
    {
        lookValue = context.ReadValue<Vector2>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movementValue = context.ReadValue<Vector2>();
    }



    public void OnAttack(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        OnAttackEvent?.Invoke();
    }

    public void OnCancelGame(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        OnCancelGameEvent?.Invoke();
    }
    public void OnCancelMenu(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        OnCancelMenuEvent?.Invoke();
    }


    public void OnScroll(InputAction.CallbackContext context)
    {
        scrollValue = context.ReadValue<Vector2>();
    }
    public void OnAim(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            aiming = true;
            OnAimEvent?.Invoke(aiming);
        }
        if (context.canceled)
        {
            aiming = false;
            OnAimEvent?.Invoke(aiming);
        }
    }


}



