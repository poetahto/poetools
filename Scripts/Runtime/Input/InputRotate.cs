using UnityEngine;

namespace Input
{
    public class InputRotate : MonoBehaviour
    {
        [Header("Rotation Settings")]
        [SerializeField] public float rotationSpeed = 1f;
        [SerializeField] private Vector2 rotationConstraints = new Vector2(90f, -1f);
        [SerializeField] private bool rotateX;
        [SerializeField] private bool rotateY;
        
        private Vector2 _currentRotation = Vector2.zero;

        private void Update()
        {
#if ENABLE_LEGACY_INPUT_MANAGER && !ENABLE_INPUT_SYSTEM
            ProvideJumpInputLegacy();
#endif
        
            ClampCurrentRotation();
            ApplyRotation();
        }

        private void ApplyInputsToRotation(Vector2 inputs)
        {
            _currentRotation.x += inputs.y * rotationSpeed ;
            _currentRotation.y += inputs.x * rotationSpeed ;
        }

        private void ClampCurrentRotation()
        {
            if (rotationConstraints.x > 0)
                _currentRotation.x = Mathf.Clamp(_currentRotation.x, -rotationConstraints.x, rotationConstraints.x);
        
            if (rotationConstraints.y > 0)
                _currentRotation.y = Mathf.Clamp(_currentRotation.y, -rotationConstraints.y, rotationConstraints.y);
        }

        private void ApplyRotation()
        {
            Vector3 targetRotation = Vector3.zero;

            if (rotateX)
                targetRotation.x = -_currentRotation.x;

            if (rotateY)
                targetRotation.y = _currentRotation.y;

            transform.localRotation = Quaternion.Euler(targetRotation);
        }

        #region Input Systems

#if ENABLE_INPUT_SYSTEM
        [JetBrains.Annotations.PublicAPI]
        public void ProvideLookInput(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            if (context.performed)
                ApplyInputsToRotation(context.ReadValue<Vector2>());
        }
#endif
        
#if ENABLE_LEGACY_INPUT_MANAGER && !ENABLE_INPUT_SYSTEM
        private void ProvideJumpInputLegacy()
        {
            float mouseX = UnityEngine.Input.GetAxisRaw("Mouse X");
            float mouseY = UnityEngine.Input.GetAxisRaw("Mouse Y");
            
            ApplyInputsToRotation(new Vector2(mouseX, mouseY));
        }
#endif

        #endregion
    }
}