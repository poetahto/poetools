using UnityEngine;

namespace poetools.Core
{
    public class CursorSettings : MonoBehaviour
    {
        [SerializeField]
        private CursorLockMode lockMode;
        
        [SerializeField]
        private bool isVisible;
        
        private void Awake()
        {
            Cursor.lockState = lockMode;
            Cursor.visible = isVisible;
        }

        private void OnValidate()
        {
            Awake();
        }
    }
}