using UnityEngine;
using UnityEngine.Events;

namespace poetools.player.Player.WIP
{
    public class InputControlTarget : MonoBehaviour
    {
        [Header("Control Events")]
        public UnityEvent controlStartEvent;
        public UnityEvent controlStopEvent;
        
        [Header("Input Events")]
        public UnityEvent inputJumpEvent;
        public UnityEvent<Vector2> inputMoveEvent;
        public UnityEvent<bool> inputSprintEvent;
        public UnityEvent<Vector2> inputMouseDeltaEvent;
        public UnityEvent<bool> inputCrouchEvent;
    }
}