/*namespace Application.Core.Serialization
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using Gameplay;
    using ImGuiNET;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using UniRx;
    using UnityEngine;
    using Utility;
    using Debug = UnityEngine.Debug;

    /// <summary>
    /// Central API for saving and loading game state.
    /// </summary>
    public class Serializer
    {
        /// <summary>
        /// The settings that this serializer uses during serialization.
        /// </summary>
        public readonly JsonSerializerSettings Settings;

        private readonly Subject<Unit> _onWrite = new Subject<Unit>();
        private readonly Subject<Unit> _onRead = new Subject<Unit>();

        private Dictionary<string, object> _savedData;

        /// <summary>
        /// Initializes a new instance of the <see cref="Serializer"/> class.
        /// </summary>
        public Serializer()
        {
            Settings = JsonConvert.DefaultSettings?.Invoke();

            if (Settings != null)
            {
                Settings.Formatting = Formatting.Indented;
                Settings.TypeNameHandling = TypeNameHandling.Auto;
            }

            _savedData = new Dictionary<string, object>();
            ImGuiUtil.Register(DrawDebugUI);
        }

        /// <summary>
        /// An observable that emits each time the disk is written to.
        /// </summary>
        /// <returns>An observable.</returns>
        public IObservable<Unit> ObserveWrite() => _onWrite;

        /// <summary>
        /// An observable that emits each time the disk is read from.
        /// </summary>
        /// <returns>An observable.</returns>
        public IObservable<Unit> ObserveRead() => _onRead;

        /// <summary>
        /// Finds a serialized value or creates one if it doesn't exist.
        /// </summary>
        /// <param name="key">The id of the value to search for.</param>
        /// <param name="result">The object saved by the serializer.</param>
        /// <param name="defaultValue">The initial value if no items exist with the desired id.</param>
        /// <typeparam name="T">The type of data to retrieve.</typeparam>
        public void Lookup<T>(string key, out T result, T defaultValue = default)
        {
            if (!Exists(key))
            {
                _savedData.Add(key, defaultValue);
            }

            result = (T)_savedData[key];
        }

        /// <summary>
        /// Tries to access a serialized value.
        /// </summary>
        /// <param name="key">The key of the data to search for.</param>
        /// <param name="result">The place to store the result of the query.</param>
        /// <typeparam name="T">The type of data to search for.</typeparam>
        /// <returns>True if the data was found, false if it was not.</returns>
        public bool TryLookup<T>(string key, out T result)
        {
            if (Exists(key))
            {
                result = _savedData[key] is JObject obj
                    ? obj.ToObject<T>()
                    : (T)_savedData[key];

                return true;
            }

            result = default;
            return false;
        }

        /// <summary>
        /// Checks if a key is stored in the serializer.
        /// </summary>
        /// <param name="key">The id of the key to search for.</param>
        /// <returns>True is the key is present, false if it does not exist.</returns>
        public bool Exists(string key)
        {
            return _savedData.ContainsKey(key);
        }

        /// <summary>
        /// Stores and updates a value in the serializer.
        /// </summary>
        /// <param name="key">The id of the object to store.</param>
        /// <param name="value">The data associated with the object.</param>
        /// <typeparam name="T">The type of data to store.</typeparam>
        public void Store<T>(string key, T value)
        {
            if (!Exists(key))
            {
                _savedData.Add(key, value);
            }
            else
            {
                _savedData[key] = value;
            }
        }

        /// <summary>
        /// Serializes all saved data into a save file.
        /// </summary>
        /// <param name="fileName">The name of the save file to write the information into.</param>
        public void WriteToDisk(string fileName)
        {
            _onWrite.OnNext(Unit.Default);
            Save(fileName, _savedData);
        }

        /// <summary>
        /// Deserializes all saved data from a save file.
        /// </summary>
        /// <param name="fileName">The name of the save file to read the information from.</param>
        public void ReadFromDisk(string fileName)
        {
            if (!TryLoad(fileName, out _savedData))
            {
                Debug.Log($"Failed to read \"{fileName}\" from disk when loading.");
            }

            _onRead.OnNext(Unit.Default);
        }

        /// <summary>
        /// Writes information to serializable components attached to a GameObject.
        /// </summary>
        /// <param name="gameObject">The GameObject to search for serializable components.</param>
        public void ApplySavedDataTo(GameObject gameObject)
        {
            if (gameObject == null)
            {
                return;
            }

            foreach (var serializable in gameObject.GetComponentsInChildren<ISerializable>())
            {
                ApplySavedDataTo(serializable);
            }
        }

        /// <summary>
        /// Writes saved information into a serializable object.
        /// </summary>
        /// <param name="serializable">The object to write information into.</param>
        public void ApplySavedDataTo(ISerializable serializable)
        {
            if (serializable == null)
            {
                return;
            }

            string id = serializable.Id;

            if (_savedData.ContainsKey(id))
            {
                var data = _savedData[id];
                serializable.ReadData(data);
            }
        }

        /// <summary>
        /// Reads information from serializable components attached to a GameObject.
        /// </summary>
        /// <param name="gameObject">The GameObject to search for serialized components.</param>
        public void UpdateSavedDataFrom(GameObject gameObject)
        {
            if (gameObject == null)
            {
                return;
            }

            foreach (var serializable in gameObject.GetComponentsInChildren<ISerializable>())
            {
                UpdateSavedDataFrom(serializable);
            }
        }

        /// <summary>
        /// Reads information from a serializable object.
        /// </summary>
        /// <param name="serializable">The object to read information from.</param>
        public void UpdateSavedDataFrom(ISerializable serializable)
        {
            if (serializable == null)
            {
                return;
            }

            string id = serializable.Id;

            if (!_savedData.ContainsKey(id))
            {
                _savedData.Add(id, serializable.WriteData());
            }
            else
            {
                _savedData[id] = serializable.WriteData();
            }
        }

        /// <summary>
        /// Returns the save path of a certain save file name.
        /// </summary>
        /// <param name="fileName">The name of the save file to use in the path.</param>
        /// <returns>The path to the save file named "fileName".</returns>
        private static string GetPath(string fileName)
        {
            string path = $"{Application.persistentDataPath}/Saves";
            Directory.CreateDirectory(path);
            return $"{path}/{fileName}";
        }

        /// <summary>
        /// Determine if a save file exists on the disk.
        /// </summary>
        /// <param name="fileName">The save file name to search for.</param>
        /// <returns>True if the save file exists, false if it does not.</returns>
        private static bool IsValid(string fileName)
        {
            string savePath = GetPath(fileName);
            return File.Exists(savePath);
        }

        /// <summary>
        /// Attempts to deserialize some data from a file.
        /// </summary>
        /// <param name="fileName">The name of the file to read from.</param>
        /// <param name="result">The result of the deserialization.</param>
        /// <typeparam name="T">The type of result we are expecting.</typeparam>
        /// <returns>True if we successfully loaded the data, false if something went wrong.</returns>
        private bool TryLoad<T>(string fileName, out T result)
        {
            if (IsValid(fileName))
            {
                string path = GetPath(fileName);
                string data = File.ReadAllText(path);
                result = JsonConvert.DeserializeObject<T>(data, Settings);
                return true;
            }

            result = default;
            return false;
        }

        /// <summary>
        /// Serializes some object data to the disk.
        /// </summary>
        /// <param name="fileName">The name of the file to write the data to.</param>
        /// <param name="target">The object to serialize into the file.</param>
        /// <typeparam name="T">The type of data we are serializing.</typeparam>
        private void Save<T>(string fileName, T target)
        {
            string path = GetPath(fileName);
            string data = JsonConvert.SerializeObject(target, Settings);
            File.WriteAllText(path, data);
        }

        private void DrawDebugUI()
        {
            ImGui.Begin("Serialization");

            var path = $"{Application.persistentDataPath}/Saves";

            if (ImGui.Button("Open Save Location"))
            {
                Process.Start(path);
            }

            if (ImGui.Button("Clear Old Saves"))
            {
                Directory.Delete(path);
            }

            if (ImGui.Button("Write"))
            {
                WriteToDisk("debug-save.json");
            }

            if (ImGui.Button("Read"))
            {
                ReadFromDisk("debug-save.json");
                Services.EventBus.Invoke(new StartGameCommand(), "Serializer IMGUI");
            }

            ImGui.End();
        }
    }
}
*/