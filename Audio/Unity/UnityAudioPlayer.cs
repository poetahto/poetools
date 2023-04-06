using poetools.Core;
using UnityEngine;

namespace poetools.Audio.Unity
{
    internal class UnityAudioPlayer : LazySingleton<UnityAudioPlayer>
    {
        private ObjectPool<UnityPooledAudioSource> _audioSources;

        protected override void Awake()
        {
            base.Awake();
            _audioSources = new ObjectPool<UnityPooledAudioSource>(CreateAudioSource);
        }

        private UnityPooledAudioSource CreateAudioSource()
        {
            GameObject audioObject = new GameObject($"Audio Source {_audioSources.CountAll}");
            var audioSource = audioObject.AddComponent<UnityPooledAudioSource>();
            audioSource.ParentPool = _audioSources;
            return audioSource;
        }

        public void Play(UnityAudioEvent audioEvent)
        {
            UnityPooledAudioSource audioSource = _audioSources.Get();
            audioSource.Play(audioEvent);
        }

        public void PlayAt(UnityAudioEvent audioEvent, Vector3 position)
        {
            UnityPooledAudioSource audioSource = _audioSources.Get();
            audioSource.PlayAt(audioEvent, position);
        }
    }
}