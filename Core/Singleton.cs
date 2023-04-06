using UnityEngine;
using Object = UnityEngine.Object;

namespace poetools.Core
{
    public abstract class LazySingleton<T> : MonoBehaviour where T : LazySingleton<T>
    {
        private static T _singleton;
        public static T Singleton
        {
            get
            {
                if (_singleton == null)
                    _singleton = SingletonHelper.CreateInstance<T>();

                return _singleton;
            }
        }

        protected virtual void Awake()
        {
            if (_singleton == null)
                _singleton = (T) this;

            SingletonHelper.ValidateInstance(this, _singleton);
        }
    }

    public abstract class PreparedSingleton<T> : MonoBehaviour where T : PreparedSingleton<T>
    {
        private static T _singleton;
        public static T Singleton
        {
            get
            {
                if (_singleton == null)
                {
                    string errorMessage = "Singleton was not prepared correctly. Call Prepare() before accessing!";
                    Debug.LogError(errorMessage.Format(Rtf.Error));
                }

                return _singleton;
            }
        }

        protected virtual void Awake()
        {
            if (_singleton == null)
                _singleton = (T) this;

            SingletonHelper.ValidateInstance(this, _singleton);
        }

        public static void Prepare()
        {
            _singleton = SingletonHelper.CreateInstance<T>();
        }
    }

    internal static class SingletonHelper
    {
        internal static T CreateInstance<T>() where T : MonoBehaviour
        {
            GameObject instanceObject = new GameObject(typeof(T).Name);
            Object.DontDestroyOnLoad(instanceObject);
            return instanceObject.AddComponent<T>();
        }

        internal static void ValidateInstance<T>(T current, T instance) where T : MonoBehaviour
        {
            if (instance != null && current != instance)
            {
                var errorMessage = $"Duplicate Singleton removed from {current.gameObject.scene}.";
                Debug.LogError(errorMessage.Format(Rtf.Error));
                Object.Destroy(current);
            }
        }
    }
}
