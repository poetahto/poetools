using UnityEngine;

namespace poetools.player.Player.Crouching.States
{
    public class CrouchingGroundState : CrouchState
    {
        public CrouchingGroundState(FPSCrouchingLogic parent) : base(parent)
        {
        }

        public override void Enter()
        {
            base.Enter();
            Parent.RawCameraTransform.localPosition = new Vector3(0, Parent.Settings.crouchHeight * Parent.Settings.cameraPercent, 0);
            Parent.CrouchDown();
        }

        public override void Tick()
        {
            base.Tick();
            var cur = Parent.SteadyBasePosition;
            cur.y = Mathf.Lerp(cur.y, Parent.RawCameraTransform.position.y - (Parent.Settings.cameraPercent * Parent.Settings.crouchHeight), Parent.Settings.crouchingSpeed * Time.deltaTime);
            Parent.SteadyBasePosition = cur;
        }

        public override void PhysicsTick()
        {
            base.PhysicsTick();

            if (!Parent.IsGrounded)
            {
                Parent.TransitionTo(Parent.CrouchingAir);
            }
            else if (!Parent.WantsToCrouch && Parent.CanStand)
            {
                Parent.TransitionTo(Parent.Standing);
            }
        }
    }
}
