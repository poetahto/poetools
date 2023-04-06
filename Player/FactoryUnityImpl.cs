using poetools.Core;
using poetools.Core.Abstraction;
using poetools.Core.Abstraction.Unity;
using UnityEngine;

namespace poetools.player.Player
{
    public static class FactoryUnityImpl
    {
        public static Gravity AddGravity(this GameObject target)
        {
            var gravity = target.GetComponentInChildren<Gravity>();

            if (target.TryGetComponent(out Rigidbody rb))
                rb.useGravity = false;

            if (gravity == null)
            {
                gravity = target.AddComponent<Gravity>();
                gravity.groundCheck = AddGroundCheck(target);
            }

            return gravity;
        }

        public static GroundCheck AddGroundCheck(this GameObject target)
        {
            var groundCheck = target.GetComponentInChildren<GroundCheck>();

            if (groundCheck == null)
                groundCheck = target.AddComponent<GroundCheck>();

            return groundCheck;
        }

        public static PhysicsComponent AddPhysicsRigidbody(this GameObject target)
        {
            var result = target.GetComponentInChildren<PhysicsComponent>();

            if (result != null)
                return result;

            else
            {
                var rb = target.AddComponent<Rigidbody>();
                rb.interpolation = RigidbodyInterpolation.Interpolate;
                rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
                rb.constraints = RigidbodyConstraints.FreezeRotation;

                if (target.TryGetComponent(out Gravity _))
                    rb.useGravity = false;
            }

            return target.AddComponent<RigidbodyWrapper>();
        }

        public static PhysicsComponent AddPhysicsCharacterController(this GameObject target)
        {
            var result = target.GetComponentInChildren<PhysicsComponent>();

            if (result != null)
                return result;
            else
            {
                var cc = target.AddComponent<CharacterController>();
                cc.height = 2;
                cc.center = new Vector3(0, 1, 0);
            }

            return target.AddComponent<CharacterControllerWrapper>();
        }
    }
}
