using poetools.player.Player.Movement;
using UnityEngine;

namespace poetools.player.Player
{
    public class QuakeForwardFovBoost : MonoBehaviour
    {
        [SerializeField] private new Camera camera;
        [SerializeField] private FPSQuakeMovementLogicContainer quakeMovement;
        [SerializeField] private float fovMult = 1.25f;
        [SerializeField] private float fovTime = 3f;

        private float _normalFOV;
        private float _sprintFOV;

        public bool IsSprinting => quakeMovement.QuakeMovementLogic.TargetDirection.y > 0;

        private void Awake()
        {
            _normalFOV = camera.fieldOfView;
            _sprintFOV = _normalFOV * fovMult;
        }

        private void Update()
        {
            var targetFov = IsSprinting ? _sprintFOV : _normalFOV;
            camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, targetFov, fovTime * Time.deltaTime);
        }
    }
}
