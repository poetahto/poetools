using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace poetools.Core.Tools
{
    public static class SceneTools
    {
        public static bool IsLoaded(string scene)
        {
            bool result = false;
            result |= SceneManager.GetSceneByName(scene).isLoaded;
            result |= SceneManager.GetSceneByPath(scene).isLoaded;
            return result;
        }

        public static IEnumerable<T> GetObjectsOfType<T>(this Scene scene) where T : Component
        {
            GameObject[] rootObjects = scene.GetRootGameObjects();
            var results = new List<T>();

            foreach (var gameObject in rootObjects)
            {
                if (gameObject.TryGetComponent(typeof(T), out var component))
                    results.Add((T) component);
            }

            return results;
        }

        public static AsyncOperation UnloadActiveScene()
        {
            var activeScene = SceneManager.GetActiveScene();
            return SceneManager.UnloadSceneAsync(activeScene);
        }

        public static void SetActiveScene(string scene)
        {
            var sceneInstance = SceneManager.GetSceneByName(scene);

            // Try and fallback to the path, if the name doesn't work.
            if (!sceneInstance.isLoaded)
                sceneInstance = SceneManager.GetSceneByPath(scene);

            SceneManager.SetActiveScene(sceneInstance);
        }

        public static IEnumerator ReplaceActiveScene(string sceneName)
        {
            yield return UnloadActiveScene();
            yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            SetActiveScene(sceneName);
        }
    }
}
