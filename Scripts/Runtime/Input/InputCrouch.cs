using UnityEngine;

namespace Input
{
    public class InputCrouch : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private CharacterController controller;
        [SerializeField] private Camera playerCamera;
        
        [Header("Settings")] 
        [SerializeField] private float sneakHeight = 1;
        [SerializeField] private Vector3 sneakCenter = new Vector3(0, 0.5f, 0);

        private float _originalHeight;
        private Vector3 _originalCenter;

        private void Awake()
        {
            _originalHeight = controller.height;
            _originalCenter = controller.center;
        }

        private void OnValidate()
        {
            _originalHeight = controller.height;
            _originalCenter = controller.center;
        }

        private void BeginCrouch()
        {
            controller.height = sneakHeight;
            controller.center = sneakCenter;
        }

        private void EndCrouch()
        {
            controller.height = _originalHeight;
            controller.center = _originalCenter;
        }

        #region Input Systems

#if ENABLE_INPUT_SYSTEM
        [JetBrains.Annotations.PublicAPI]
        public void ProvideLookInput(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            if (context.started)
                BeginCrouch();

            if (context.canceled)
                EndCrouch();
        }
#endif
        
#if ENABLE_LEGACY_INPUT_MANAGER && !ENABLE_INPUT_SYSTEM
        private void Update()
        {
            ProvideSneakInputLegacy();
        }

        private void ProvideSneakInputLegacy()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.LeftControl))
                BeginCrouch();
            
            else if (UnityEngine.Input.GetKeyUp(KeyCode.LeftControl))
                EndCrouch();
        }
#endif

        #endregion
    }
}