using System.Collections.Generic;
using UnityEngine;

namespace poetools.player.Player.Rotation
{
    public class FPSRotationLogic
    {
        private readonly RotationSettings _settings;

        private float _currentPitch;
        private float _currentYaw;

        public FPSRotationLogic(RotationSettings settings)
        {
            _settings = settings;
        }

        public ICollection<Transform> PitchTargets { get; } = new List<Transform>();
        public ICollection<Transform> YawTargets { get; } = new List<Transform>();

        public void ApplyDelta(Vector2 mouseDelta)
        {
            float yawAngle = mouseDelta.x * _settings.sensitivity;
            float pitchAngle = mouseDelta.y * _settings.sensitivity * (_settings.invertY ? 1 : -1);

            SetRotation(_currentPitch + pitchAngle, _currentYaw + yawAngle);
        }

        public void SetRotation(float pitch, float yaw)
        {
            SetRotation(new Vector3(pitch, yaw));
        }

        public void SetRotation(Vector3 euler)
        {
            _currentYaw = Mathf.Repeat(euler.y, 360);
            _currentPitch = Mathf.Clamp(euler.x, -90, 90);

            foreach (var pitchTransform in PitchTargets)
            {
                var current = pitchTransform.localRotation.eulerAngles;
                current.x = _currentPitch;
                pitchTransform.localRotation = Quaternion.Euler(current);
            }

            foreach (var yawTransform in YawTargets)
            {
                var current = yawTransform.localRotation.eulerAngles;
                current.y = _currentYaw;
                yawTransform.localRotation = Quaternion.Euler(current);
            }
        }

        public Vector3 GetRotation()
        {
            return new Vector3(_currentPitch, _currentYaw);
        }
    }
}
