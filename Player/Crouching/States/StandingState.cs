using UnityEngine;

namespace poetools.player.Player.Crouching.States
{
    public class StandingState : CrouchState
    {
        public StandingState(FPSCrouchingLogic parent) : base(parent)
        {
        }

        public override void Enter()
        {
            base.Enter();
            Parent.RawCameraTransform.localPosition = new Vector3(0, Parent.Settings.standingHeight * Parent.Settings.cameraPercent, 0);
            Parent.StandUp();
        }

        public override void Tick()
        {
            base.Tick();
            var cur = Parent.SteadyBasePosition;
            cur.y = Mathf.Lerp(cur.y, Parent.RawCameraTransform.position.y - (Parent.Settings.cameraPercent * Parent.Settings.standingHeight), Parent.Settings.crouchingSpeed * Time.deltaTime);
            Parent.SteadyBasePosition = cur;
        }

        public override void PhysicsTick()
        {
            base.PhysicsTick();

            if (Parent.WantsToCrouch)
            {
                if (Parent.IsGrounded)
                    Parent.TransitionTo(Parent.CrouchingGround);

                else
                {
                    Parent.TransitionTo(Parent.CrouchingAir);

                    var amount = Parent.Parent.up * (Parent.Settings.standingHeight - Parent.Settings.crouchHeight);
                    Parent.Parent.position += amount;
                    Parent.RawCameraTransform.position -= amount;
                    Parent.SteadyBasePosition -= amount;
                    Parent.SmoothedCrouchPosition -= amount;
                }
            }
        }
    }
}
