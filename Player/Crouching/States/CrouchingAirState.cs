using UnityEngine;

namespace poetools.player.Player.Crouching.States
{
    public class CrouchingAirState : CrouchState
    {
        public CrouchingAirState(FPSCrouchingLogic parent) : base(parent)
        {
        }

        public override void Enter()
        {
            base.Enter();
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

            if (Parent.IsGrounded)
                Parent.TransitionTo(Parent.CrouchingGround);

            else if (!Parent.WantsToCrouch)
            {
                if (Parent.HeadRoom.CurrentColliders.Count == 0)
                {
                    var amount = Vector3.down * (Parent.Settings.standingHeight - Parent.Settings.crouchHeight);
                    Parent.Parent.transform.position += amount;
                    Parent.SteadyBasePosition -= amount;
                    Parent.SmoothedCrouchPosition -= amount;
                    Parent.TransitionTo(Parent.Standing);
                }
                else if (Parent.CanStand && Parent.Cast(Vector3.down, float.PositiveInfinity, out RaycastHit info))
                {
                    var origPosT = Parent.RawCameraTransform.position;
                    var origPosS = Parent.SteadyBasePosition;
                    var origPosC = Parent.SmoothedCrouchPosition;
                    var parentTransform = Parent.Parent.transform;
                    var parentPos = parentTransform.position;
                    parentPos.y = info.point.y;
                    parentTransform.position = parentPos;
                    Parent.RawCameraTransform.position = origPosT;
                    Parent.SteadyBasePosition = origPosS;
                    Parent.SmoothedCrouchPosition = origPosC;

                    Parent.TransitionTo(Parent.Standing);
                }
            }
        }
    }
}
