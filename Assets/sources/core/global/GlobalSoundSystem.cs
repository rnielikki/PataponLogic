using UnityEngine;

namespace PataRoad.Core
{
    /// <summary>
    /// Plays sound on scene change.
    /// </summary>
    public class GlobalSoundSystem : MonoBehaviour
    {
        private AudioSource _globalAudioSource;
        private AudioClip _globalAudioClip;
        private void Awake()
        {
            _globalAudioSource = GetComponentInChildren<AudioSource>();
        }
        public void Play(AudioClip clip)
        {
            _globalAudioClip = clip;
            _globalAudioSource.PlayOneShot(_globalAudioClip);
        }
    }
}
