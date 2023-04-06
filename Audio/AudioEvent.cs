using UnityEngine;

namespace poetools.Audio
{
    public abstract class AudioEvent : ScriptableObject
    {
        public abstract void Play();
        public abstract void PlayAt(Vector3 location);
    }
}
