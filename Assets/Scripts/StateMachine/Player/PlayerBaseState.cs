using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachineCore.Player
{
    public abstract class PlayerBaseState : State
    {
        protected PlayerStateMachine stateMachine;

        public PlayerBaseState(PlayerStateMachine stateMachine)
        {
            this.stateMachine = stateMachine;
        }

        const float k_JumpGroundingPreventionTime = 0.2f;
        const float k_GroundCheckDistanceInAir = 0.07f;

        protected readonly int MovXHash = Animator.StringToHash("MovX");
        protected readonly int MovYHash = Animator.StringToHash("MovY");


        protected const float AnimatorDampTime = .1f;

        protected void GroundCheck(float deltaTime)
        {
            // Make sure that the ground check distance while already in air is very small, to prevent suddenly snapping to ground
            float chosenGroundCheckDistance =
                stateMachine.isGrounded ? (stateMachine.m_Controller.skinWidth + stateMachine.GroundCheckDistance) : k_GroundCheckDistanceInAir;

            // reset values before the ground check
            stateMachine.IsGrounded = false;
            stateMachine.m_GroundNormal = Vector3.up;

            // only try to detect ground if it's been a short amount of time since last jump; otherwise we may snap to the ground instantly after we try jumping
            if (Time.time >= stateMachine.m_LastTimeJumped + k_JumpGroundingPreventionTime)
            {
                // if we're grounded, collect info about the ground normal with a downward capsule cast representing our character capsule
                if (Physics.CapsuleCast(GetCapsuleBottomHemisphere(), GetCapsuleTopHemisphere(stateMachine.m_Controller.height),
                    stateMachine.m_Controller.radius, Vector3.down, out RaycastHit hit, chosenGroundCheckDistance, stateMachine.GroundCheckLayers,
                    QueryTriggerInteraction.Ignore))
                {
                    // storing the upward direction for the surface found
                    stateMachine.m_GroundNormal = hit.normal;

                    // Only consider this a valid ground hit if the ground normal goes in the same direction as the character up
                    // and if the slope angle is lower than the character controller's limit
                    if (Vector3.Dot(hit.normal, stateMachine.transform.up) > 0f &&
                        IsNormalUnderSlopeLimit(stateMachine.m_GroundNormal))
                    {
                        stateMachine.IsGrounded = true;

                        // handle snapping to the ground
                        if (hit.distance > stateMachine.m_Controller.skinWidth)
                        {
                            stateMachine.m_Controller.Move(Vector3.down * hit.distance);
                        }
                    }
                }
            }
        }

        Vector3 GetCapsuleTopHemisphere(float atHeight)
        {
            return stateMachine.transform.position + (stateMachine.transform.up * (atHeight - stateMachine.m_Controller.radius));
        }

        // Gets the center point of the bottom hemisphere of the character controller capsule    
        Vector3 GetCapsuleBottomHemisphere()
        {
            return stateMachine.transform.position + (stateMachine.transform.up * stateMachine.m_Controller.radius);
        }

        // Returns true if the slope angle represented by the given normal is under the slope angle limit of the character controller
        bool IsNormalUnderSlopeLimit(Vector3 normal)
        {
            return Vector3.Angle(stateMachine.transform.up, normal) <= stateMachine.m_Controller.slopeLimit;
        }


        // returns false if there was an obstruction
        public bool SetCrouchingState(bool crouched, bool ignoreObstructions)
        {
            // set appropriate heights
            if (crouched)
            {
                stateMachine.m_TargetCharacterHeight = stateMachine.CapsuleHeightCrouching;
            }
            else
            {
                // Detect obstructions
                if (!ignoreObstructions)
                {
                    Collider[] standingOverlaps = Physics.OverlapCapsule(
                        GetCapsuleBottomHemisphere(),
                        GetCapsuleTopHemisphere(stateMachine.CapsuleHeightStanding),
                        stateMachine.m_Controller.radius,
                        -1,
                        QueryTriggerInteraction.Ignore);
                    foreach (Collider c in standingOverlaps)
                    {
                        if (c != stateMachine.m_Controller)
                        {
                            return false;
                        }
                    }
                }

                stateMachine.m_TargetCharacterHeight = stateMachine.CapsuleHeightStanding;
            }

            if (stateMachine.OnStanceChanged != null)
            {
                stateMachine.OnStanceChanged.Invoke(crouched);
            }

            stateMachine.IsCrouching = crouched;
            return true;
        }



        public void UpdateCharacterHeight(bool force, float deltaTime)
        {
            // Update height instantly
            if (force)
            {
                stateMachine.m_Controller.height = stateMachine.m_TargetCharacterHeight;
                stateMachine.m_Controller.center = Vector3.up * stateMachine.m_Controller.height * 0.5f;
                stateMachine.PlayerCamera.transform.localPosition = Vector3.up * stateMachine.m_TargetCharacterHeight * stateMachine.CameraHeightRatio;
                stateMachine.m_Actor.AimPoint.transform.localPosition = stateMachine.m_Controller.center;
            }
            // Update smooth height
            else if (stateMachine.m_Controller.height != stateMachine.m_TargetCharacterHeight)
            {
                // resize the capsule and adjust camera position
                stateMachine.m_Controller.height = Mathf.Lerp(stateMachine.m_Controller.height, stateMachine.m_TargetCharacterHeight,
                    stateMachine.CrouchingSharpness * Time.deltaTime);
                stateMachine.m_Controller.center = Vector3.up * stateMachine.m_Controller.height * 0.5f;
                stateMachine.PlayerCamera.transform.localPosition = Vector3.Lerp(stateMachine.PlayerCamera.transform.localPosition,
                    Vector3.up * stateMachine.m_TargetCharacterHeight * stateMachine.CameraHeightRatio, stateMachine.CrouchingSharpness * deltaTime);
                stateMachine.m_Actor.AimPoint.transform.localPosition = stateMachine.m_Controller.center;
            }
        }

        protected void CheckZKill()
        {
            // check for Y kill
            if (!stateMachine.IsDead && stateMachine.transform.position.y < stateMachine.KillHeight)
            {
                stateMachine.m_Health.Kill();
            }
        }



        protected void HandleCharacterMovement(float deltaTime)
        {
            // horizontal character rotation
            {
                // rotate the transform with the input speed around its local Y axis
                stateMachine.transform.Rotate(
                    new Vector3(0f, (stateMachine.m_InputHandler.GetLookInputsHorizontal() * stateMachine.RotationSpeed * stateMachine.RotationMultiplier),
                        0f), Space.Self);
            }

            // vertical camera rotation
            {
                // add vertical inputs to the camera's vertical angle
                stateMachine.m_CameraVerticalAngle += stateMachine.m_InputHandler.GetLookInputsVertical() * stateMachine.RotationSpeed * stateMachine.RotationMultiplier;

                // limit the camera's vertical angle to min/max
                stateMachine.m_CameraVerticalAngle = Mathf.Clamp(stateMachine.m_CameraVerticalAngle, -89f, 89f);

                // apply the vertical angle as a local rotation to the camera transform along its right axis (makes it pivot up and down)
                stateMachine.PlayerCamera.transform.localEulerAngles = new Vector3(stateMachine.m_CameraVerticalAngle, 0, 0);
            }

            bool isSprinting = stateMachine.m_InputHandler.GetSprintInputHeld();
            SprintMode(isSprinting, deltaTime);

            // apply the final calculated velocity value as a character movement
            Vector3 capsuleBottomBeforeMove = GetCapsuleBottomHemisphere();
            Vector3 capsuleTopBeforeMove = GetCapsuleTopHemisphere(stateMachine.m_Controller.height);
            stateMachine.m_Controller.Move(stateMachine.CharacterVelocity * deltaTime);

            // detect obstructions to adjust velocity accordingly
            stateMachine.m_LatestImpactSpeed = Vector3.zero;
            if (Physics.CapsuleCast(capsuleBottomBeforeMove, capsuleTopBeforeMove, stateMachine.m_Controller.radius,
                stateMachine.CharacterVelocity.normalized, out RaycastHit hit, stateMachine.CharacterVelocity.magnitude * deltaTime, -1,
                QueryTriggerInteraction.Ignore))
            {
                // We remember the last impact speed because the fall damage logic might need it
                stateMachine.m_LatestImpactSpeed = stateMachine.CharacterVelocity;

                stateMachine.CharacterVelocity = Vector3.ProjectOnPlane(stateMachine.CharacterVelocity, hit.normal);
            }
        }


        protected void SprintMode(bool isSprinting, float deltaTime)
        {
            float speedModifier = isSprinting ? stateMachine.SprintSpeedModifier : 1f;

            // converts move input to a worldspace vector based on our character's transform orientation
            Vector3 worldspaceMoveInput = stateMachine.transform.TransformVector(stateMachine.m_InputHandler.GetMoveInput());

            // handle grounded movement
            if (stateMachine.isGrounded)
            {
                // calculate the desired velocity from inputs, max speed, and current slope
                Vector3 targetVelocity = worldspaceMoveInput * stateMachine.MaxSpeedOnGround * speedModifier;
                // reduce speed if crouching by crouch speed ratio
                if (stateMachine.isCrouching)
                    targetVelocity *= stateMachine.MaxSpeedCrouchedRatio;
                targetVelocity = GetDirectionReorientedOnSlope(targetVelocity.normalized, stateMachine.m_GroundNormal) *
                                 targetVelocity.magnitude;

                // smoothly interpolate between our current velocity and the target velocity based on acceleration speed
                stateMachine.CharacterVelocity = Vector3.Lerp(stateMachine.CharacterVelocity, targetVelocity,
                    stateMachine.MovementSharpnessOnGround * deltaTime);

                /*
                // jumping
                if (stateMachine.isGrounded && stateMachine.m_InputHandler.GetJumpInputDown())
                {
                    // force the crouch state to false
                    if (SetCrouchingState(false, false))
                    {
                        // start by canceling out the vertical component of our velocity
                        stateMachine.CharacterVelocity = new Vector3(stateMachine.CharacterVelocity.x, 0f, stateMachine.CharacterVelocity.z);

                        // then, add the jumpSpeed value upwards
                        stateMachine.CharacterVelocity += Vector3.up * stateMachine.JumpForce;

                        // play sound
                        stateMachine.AudioSource.PlayOneShot(stateMachine.JumpSfx);

                        // remember last time we jumped because we need to prevent snapping to ground for a short time
                        stateMachine.m_LastTimeJumped = Time.time;
                        stateMachine.HasJumpedThisFrame = true;

                        // Force grounding to false
                        stateMachine.IsGrounded = false;
                        stateMachine.m_GroundNormal = Vector3.up;
                    }
                }
                */

                // footsteps sound
                float chosenFootstepSfxFrequency =
                    (isSprinting ? stateMachine.FootstepSfxFrequencyWhileSprinting : stateMachine.FootstepSfxFrequency);
                if (stateMachine.m_FootstepDistanceCounter >= 1f / chosenFootstepSfxFrequency)
                {
                    stateMachine.m_FootstepDistanceCounter = 0f;
                    stateMachine.AudioSource.PlayOneShot(stateMachine.FootstepSfx);
                }

                // keep track of distance traveled for footsteps sound
                stateMachine.m_FootstepDistanceCounter += stateMachine.CharacterVelocity.magnitude * deltaTime;
            }
            // handle air movement
            else
            {
                // add air acceleration
                stateMachine.CharacterVelocity += worldspaceMoveInput * stateMachine.AccelerationSpeedInAir * deltaTime;

                // limit air speed to a maximum, but only horizontally
                float verticalVelocity = stateMachine.CharacterVelocity.y;
                Vector3 horizontalVelocity = Vector3.ProjectOnPlane(stateMachine.CharacterVelocity, Vector3.up);
                horizontalVelocity = Vector3.ClampMagnitude(horizontalVelocity, stateMachine.MaxSpeedInAir * speedModifier);
                stateMachine.CharacterVelocity = horizontalVelocity + (Vector3.up * verticalVelocity);

                // apply the gravity to the velocity
                stateMachine.CharacterVelocity += Vector3.down * stateMachine.GravityDownForce * deltaTime;
            }
        }

        protected void Jump()
        {
            if (stateMachine.isGrounded)
            {
                // force the crouch state to false
                if (SetCrouchingState(false, false))
                {
                    // start by canceling out the vertical component of our velocity
                    stateMachine.CharacterVelocity = new Vector3(stateMachine.CharacterVelocity.x, 0f, stateMachine.CharacterVelocity.z);

                    // then, add the jumpSpeed value upwards
                    stateMachine.CharacterVelocity += Vector3.up * stateMachine.JumpForce;

                    // play sound
                    stateMachine.AudioSource.PlayOneShot(stateMachine.JumpSfx);

                    // remember last time we jumped because we need to prevent snapping to ground for a short time
                    stateMachine.m_LastTimeJumped = Time.time;
                    stateMachine.HasJumpedThisFrame = true;

                    // Force grounding to false
                    stateMachine.IsGrounded = false;
                    stateMachine.m_GroundNormal = Vector3.up;
                }
            }
        }

        // Gets a reoriented direction that is tangent to a given slope
        Vector3 GetDirectionReorientedOnSlope(Vector3 direction, Vector3 slopeNormal)
        {
            Vector3 directionRight = Vector3.Cross(direction, stateMachine.transform.up);
            return Vector3.Cross(slopeNormal, directionRight).normalized;
        }



    }
}



