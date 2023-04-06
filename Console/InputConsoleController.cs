using poetools.Core.Abstraction;
using UnityEngine;

namespace poetools.Console
{
    public class InputConsoleController : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.BackQuote))
            {
                RuntimeConsole.Singleton.View.Toggle();
            }
        }
    }
}