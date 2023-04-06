using System.Collections;
using poetools.Core;
using UnityEngine;

namespace poetools.Audio.Unity
{
    internal class UnityPooledAudioSource : MonoBehaviour
    {
        public ObjectPool<UnityPooledAudioSource> ParentPool { get; set; }

        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
        }

        public void Play(UnityAudioEvent audioEvent)
        {
            _audioSource.spatialize = false;
        
            IEnumerator playCoroutine = PlayCoroutine(audioEvent);
            StartCoroutine(playCoroutine);
        }

        public void PlayAt(UnityAudioEvent audioEvent, Vector3 position)
        {
            _audioSource.spatialize = true;
            transform.position = position;
        
            IEnumerator playCoroutine = PlayCoroutine(audioEvent);
            StartCoroutine(playCoroutine);
        }

        private IEnumerator PlayCoroutine(UnityAudioEvent audioEvent)
        {
            AudioClip clip = audioEvent.audioClip;
            _audioSource.clip = clip;
            _audioSource.Play();
        
            yield return new WaitForSeconds(clip.length);
        
            ParentPool.Release(this);
            transform.position = Vector3.zero;
        }
    }
}