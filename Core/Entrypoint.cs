using System.Collections;
using poetools.Core;
using TriInspector;

namespace Application
{
    using System.Threading.Tasks;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    /// <summary>
    /// The part of the entrypoint that ensures it remains loaded.
    /// </summary>
    public class Entrypoint : MonoBehaviour
    {
#if USE_ENTRYPOINT
        private static bool _initialized;

        [Scene]
        [SerializeField]
        private string firstScene;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatics()
        {
            _initialized = false;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static async Task CheckForAwakeAsync()
        {
            if (!_initialized)
            {
#if UNITY_EDITOR
                Debug.Log($"{"[EDITOR ONLY]".Bold()} Loading Entrypoint...");
                string originalScene = SceneManager.GetActiveScene().name;
                await Task.Yield(); // We have to wait one frame here, so the Entrypoint can initialize itself
                SceneManager.LoadScene("Entrypoint", LoadSceneMode.Single);
                await Task.Yield(); // We have to wait one frame here, so the Entrypoint can initialize itself
                Debug.Log($"{"[EDITOR ONLY]".Bold()} Trying to load {originalScene} after Entrypoint...");
                // var message = new StartGameCommand { InitialScene = originalScene };
                // Services.EventBus.Invoke(message, "Editor Entrypoint Setup");
                SceneManager.LoadScene(originalScene);
#else
                Debug.LogError("Entrypoint must be initialized before anything else!");
                Application.Quit();
#endif
            }
        }

        private void Awake()
        {
            _initialized = true;
            DontDestroyOnLoad(gameObject);
        }

        private IEnumerator Start()
        {
            yield return null;
            SceneManager.LoadScene(firstScene);
        }
#endif
    }
}
