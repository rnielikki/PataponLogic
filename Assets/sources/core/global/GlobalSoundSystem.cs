using UnityEngine;

namespace PataRoad.Core.Global
{
    /// <summary>
    /// Plays sound on scene change.
    /// </summary>
    public class GlobalSoundSystem : MonoBehaviour
    {
        [SerializeField]
        private AudioSource _globalAudioSource;
        private AudioClip _globalAudioClip { get; set; }
        [SerializeField]
        private AudioClip _selectedSound;
        [SerializeField]
        private AudioClip _beepSound;

        /// <summary>
        /// Music volume, range of 0-1.
        /// </summary>
        public float MusicVolume { get; set; }
        /// <summary>
        /// Voice and sound effect volume, range of 0-1.
        /// </summary>
        public float SoundVolume { get; set; }

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
