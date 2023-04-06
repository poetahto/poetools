using poetools.Core;
using poetools.Core.Abstraction;
using poetools.Core.Abstraction.Unity;
using poetools.player.Player.Crouching.Colliders;
using UnityEditor;
using UnityEngine;

namespace poetools.player.Player
{
#if UNITY_EDITOR

    public static class EditorFactoryUnityImpl
    {
        public static Gravity AddGravityWithUndo(GameObject target)
        {
            var gravity = target.GetComponentInChildren<Gravity>();

            if (target.TryGetComponent(out Rigidbody rb))
                rb.useGravity = false;

            if (gravity == null)
            {
                gravity = Undo.AddComponent<Gravity>(target);
                gravity.groundCheck = AddGroundCheckWithUndo(target);
            }

            return gravity;
        }

        public static GroundCheck AddGroundCheckWithUndo(GameObject target)
        {
            var groundCheck = target.GetComponentInChildren<GroundCheck>();

            if (groundCheck == null)
                groundCheck = Undo.AddComponent<GroundCheck>(target);

            return groundCheck;
        }

        public static PhysicsComponent AddPhysicsRigidbodyWithUndo(GameObject target)
        {
            var result = target.GetComponentInChildren<PhysicsComponent>();

            if (result != null)
                return result;

            else
            {
                var rb = Undo.AddComponent<Rigidbody>(target);
                rb.interpolation = RigidbodyInterpolation.Interpolate;
                rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
                rb.constraints = RigidbodyConstraints.FreezeRotation;

                if (target.TryGetComponent(out Gravity _))
                    rb.useGravity = false;

                var capsule = target.GetComponent<CapsuleCollider>();

                if (capsule == null)
                {
                    capsule = Undo.AddComponent<CapsuleCollider>(target);
                    capsule.center = new Vector3(0, 1, 0);
                    capsule.height = 2;
                    capsule.material = new PhysicMaterial
                    {
                        dynamicFriction = 0,
                        staticFriction = 0,
                        frictionCombine = PhysicMaterialCombine.Minimum,
                    };
                }
            }

            return Undo.AddComponent<RigidbodyWrapper>(target);
        }

        public static PhysicsComponent AddPhysicsCharacterControllerWithUndo(GameObject target)
        {
            var result = target.GetComponentInChildren<PhysicsComponent>();

            if (result != null)
                return result;

            else
            {
                var cc = Undo.AddComponent<CharacterController>(target);
                cc.height = 2;
                cc.center = new Vector3(0, 1, 0);
            }

            return Undo.AddComponent<CharacterControllerWrapper>(target);
        }

        public static CrouchingCollider AddCrouchingCharacterWithUndo(GameObject target)
        {
            var result = target.GetComponentInChildren<CrouchingCollider>();

            if (result != null)
                return result;

            else
            {
                var cc = target.GetComponent<CharacterController>();

                if (cc == null)
                {
                    cc = Undo.AddComponent<CharacterController>(target);
                    cc.height = 2;
                    cc.center = new Vector3(0, 1, 0);
                }

                var ccw = target.GetComponent<CharacterControllerWrapper>();

                if (ccw == null)
                    Undo.AddComponent<CharacterControllerWrapper>(target);

                var r = Undo.AddComponent<CharacterCrouchingCollider>(target);
                r.capsuleCollider = cc;
                return r;
            }
        }

        public static CrouchingCollider AddCrouchingCapsuleWithUndo(GameObject target)
        {
            var result = target.GetComponentInChildren<CrouchingCollider>();

            if (result != null)
                return result;

            else
            {
                var rb = target.GetComponent<Rigidbody>();

                if (rb == null)
                {
                    rb = Undo.AddComponent<Rigidbody>(target);
                    rb.interpolation = RigidbodyInterpolation.Interpolate;
                    rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
                    rb.constraints = RigidbodyConstraints.FreezeRotation;

                    if (target.TryGetComponent(out Gravity _))
                        rb.useGravity = false;
                }

                var rbw = target.GetComponent<RigidbodyWrapper>();

                if (rbw == null)
                    Undo.AddComponent<RigidbodyWrapper>(target);

                var capsule = target.GetComponent<CapsuleCollider>();

                if (capsule == null)
                {
                    capsule = Undo.AddComponent<CapsuleCollider>(target);
                    capsule.center = new Vector3(0, 1, 0);
                    capsule.height = 2;
                    capsule.material = new PhysicMaterial
                    {
                        dynamicFriction = 0,
                        staticFriction = 0,
                        frictionCombine = PhysicMaterialCombine.Minimum,
                    };
                }

                var r = Undo.AddComponent<CapsuleCrouchingCollider>(target);
                r.capsuleCollider = capsule;
                return r;
            }
        }
    }
#endif
}
