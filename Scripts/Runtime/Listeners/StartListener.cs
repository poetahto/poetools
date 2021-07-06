using UnityEngine;
using UnityEngine.Events;

namespace Listeners
{
    public class StartListener : MonoBehaviour
    {
        public UnityEvent startEvent;

        private void Start()
        {
            startEvent.Invoke();
        }
    }
}