using System;

namespace poetools.player.Player.Crouching.States
{
    public abstract class CrouchState
    {
        public event Action Entered;
        public event Action Exited;
            
        protected readonly FPSCrouchingLogic Parent;

        protected CrouchState(FPSCrouchingLogic parent)
        {
            Parent = parent;
        }

        public virtual void Enter()
        {
            Entered?.Invoke();
        }
            
        public virtual void Exit()
        {
            Exited?.Invoke();
        }
            
        public virtual void PhysicsTick() {}
        public virtual void Tick() {}
    }
}