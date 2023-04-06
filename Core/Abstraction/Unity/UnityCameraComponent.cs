using UnityEngine;

namespace poetools.Core.Abstraction.Unity
{
    [RequireComponent(typeof(Camera))]
    public class UnityCameraComponent : CameraComponent
    {
        private Camera _camera;
        private bool _cameraLoaded;

        private Camera GetCamera()
        {
            if (_cameraLoaded == false)
                _camera = GetComponent<Camera>();
            
            return _camera;
        }

        public override float Fov
        {
            get => GetFov();
            set => SetFov(value);
        }

        private float GetFov()
        {
            Camera cam = GetCamera();
            
            if (cam.orthographic)
                return cam.orthographicSize;

            return cam.fieldOfView;
        }
        
        private void SetFov(float value)
        {
            Camera cam = GetCamera();
            
            if (cam.orthographic)
                cam.orthographicSize = value;

            else cam.fieldOfView = value;
        }
    }
}