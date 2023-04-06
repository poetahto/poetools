using UnityEngine;

namespace poetools.Audio.Unity
{
    [CreateAssetMenu(menuName = AudioNaming.AssetMenuName + "/UnityAudioEvent")]
    public class UnityAudioEvent : AudioEvent
    {
        public AudioClip audioClip;
    
        public override void Play()
        {
            UnityAudioPlayer.Singleton.Play(this);
        }

        public override void PlayAt(Vector3 position)
        {
            UnityAudioPlayer.Singleton.PlayAt(this, position);
        }
    }
}