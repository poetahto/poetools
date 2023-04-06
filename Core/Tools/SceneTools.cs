using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace poetools.Core.Tools
{
    public static class SceneTools
    {
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

        public static void SetActiveScene(string sceneName)
        {
            var scene = SceneManager.GetSceneByName(sceneName);
            SceneManager.SetActiveScene(scene);
        }

        public static IEnumerator ReplaceActiveScene(string sceneName)
        {
            yield return UnloadActiveScene();
            yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            SetActiveScene(sceneName);
        }
    }
}