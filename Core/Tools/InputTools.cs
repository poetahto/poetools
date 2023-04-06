using UnityEngine;

namespace poetools.Core.Tools
{
    public static class InputTools
    {
        public static bool GetKeyDown(params KeyCode[] keyCodes)
        {
            bool result = false;
            
            foreach (var keyCode in keyCodes)
                result |= Input.GetKeyDown(keyCode);

            return result;
        }
    }
}