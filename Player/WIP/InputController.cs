using UnityEngine;

namespace poetools.player.Player.WIP
{
    public class InputController : MonoBehaviour
    {
        [SerializeField] 
        private InputControlTarget initialTarget;
        
        public InputControlTarget Target { get; private set; }
        
        private bool _hasTarget;
        public static float sensitivity = 1;

        private void Start()
        {
            SetTarget(initialTarget);
            Target.inputSprintEvent.Invoke(true);
        }

        private void Update()
        {
            if (_hasTarget)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                    Target.inputJumpEvent.Invoke();

                // if (Input.GetKeyDown(KeyCode.LeftShift))
                    // Target.inputSprintEvent.Invoke(true);
                
                // else if (Input.GetKeyUp(KeyCode.LeftShift))
                    // Target.inputSprintEvent.Invoke(false);

                if (Input.GetKeyDown(KeyCode.LeftControl))
                    Target.inputCrouchEvent.Invoke(true);
                
                else if (Input.GetKeyUp(KeyCode.LeftControl))
                    Target.inputCrouchEvent.Invoke(false);
                
                Vector2 inputDirection = new Vector2(
                    Input.GetAxisRaw("Horizontal"), 
                    Input.GetAxisRaw("Vertical")
                );

                Vector2 mouseDelta = new Vector2(
                    Input.GetAxisRaw("Mouse X"),
                    Input.GetAxisRaw("Mouse Y")
                ) * sensitivity;
                
                Target.inputMoveEvent.Invoke(inputDirection);
                Target.inputMouseDeltaEvent.Invoke(mouseDelta);
            }
        }

        public void SetTarget(InputControlTarget newTarget)
        {
            if (Target == newTarget)
                return;
                
            if (_hasTarget)
                Target.controlStopEvent.Invoke();

            Target = newTarget;
            _hasTarget = Target != null;
                
            if (_hasTarget)
                Target.controlStartEvent.Invoke();
        }
    }
}