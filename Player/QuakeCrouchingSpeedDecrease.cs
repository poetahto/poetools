using poetools.player.Player.Crouching;
using poetools.player.Player.Movement;
using UnityEngine;
using UnityEngine.Serialization;

namespace poetools.player.Player
{
    public class QuakeCrouchingSpeedDecrease : MonoBehaviour
    {
        [FormerlySerializedAs("crouchingBehavior")] [SerializeField]
        private FPSCrouchingLogicContainer fpsCrouchingLogicContainer;

        [SerializeField]
        private FPSQuakeMovementLogicContainer movement;

        [SerializeField]
        private float movementSlowdown = 0.5f;

        private float _originalSpeed;
        private float _originalFriction;

        private void Start()
        {
            fpsCrouchingLogicContainer.CrouchingLogic.CrouchingGround.Entered += CrouchGroundOnEntered;
            fpsCrouchingLogicContainer.CrouchingLogic.CrouchingGround.Exited += CrouchGroundOnExited;
        }

        private void OnDestroy()
        {
            fpsCrouchingLogicContainer.CrouchingLogic.CrouchingGround.Entered -= CrouchGroundOnEntered;
            fpsCrouchingLogicContainer.CrouchingLogic.CrouchingGround.Exited -= CrouchGroundOnExited;
        }

        private void CrouchGroundOnEntered()
        {
            _originalSpeed = movement.QuakeMovementLogic.Settings.maxGroundSpeed;
            _originalFriction = movement.QuakeMovementLogic.Settings.friction;

            movement.QuakeMovementLogic.Settings.maxGroundSpeed *= movementSlowdown;
            movement.QuakeMovementLogic.Settings.friction /= movementSlowdown;
        }

        private void CrouchGroundOnExited()
        {
            movement.QuakeMovementLogic.Settings.maxGroundSpeed = _originalSpeed;
            movement.QuakeMovementLogic.Settings.friction = _originalFriction;
        }
    }
}
