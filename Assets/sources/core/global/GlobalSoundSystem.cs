using UnityEngine;

namespace PataRoad.Core.Global
{
    /// <summary>
    /// Plays sound on scene change.
    /// </summary>
    public class GlobalSoundSystem : MonoBehaviour
    {
        private AudioSource _globalAudioSource;
#pragma warning disable S1450 // No local, it's part of global data
        private AudioClip _globalAudioClip;
#pragma warning restore S1450 // Private fields only used as local variables in methods should become local variables
        [SerializeField]
        private AudioClip _selectedSound;
        [SerializeField]
        private AudioClip _beepSound;
        private void Awake()
        {
            _globalAudioSource = GetComponentInChildren<AudioSource>();
        }
        public void PlaySelected() => PlayInScene(_selectedSound);
        public void PlayBeep() => PlayInScene(_beepSound);
        public void PlayInScene(AudioClip clip) => _globalAudioSource.PlayOneShot(clip);
        public void PlayGlobal(AudioClip clip)
        {
            _globalAudioClip = clip;
            _globalAudioSource.PlayOneShot(_globalAudioClip);
        }
    }
}
