using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachineCore
{
    public abstract class State
    {
        public abstract void Enter();
        public abstract void Tick(float deltaTime);
        public virtual void FixedTick(float deltaTime) { }  //opcional
        public abstract void Exit();

        protected float GetNormalizedTime(Animator animator, string tag, int layerIndex = 0)
        {
            AnimatorStateInfo currentInfo = animator.GetCurrentAnimatorStateInfo(layerIndex);
            AnimatorStateInfo nextInfo = animator.GetNextAnimatorStateInfo(layerIndex);

            if (animator.IsInTransition(layerIndex) && nextInfo.IsTag(tag))
            {
                return nextInfo.normalizedTime;
            }
            else if (!animator.IsInTransition(layerIndex) && currentInfo.IsTag(tag))
            {
                return currentInfo.normalizedTime;
            }
            else
            {
                return 0f;
            }
        }
    }
}



