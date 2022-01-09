using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PataRoad.Core.Global
{
    /// <summary>
    /// Plays sound on scene change.
    /// </summary>
    public class GlobalSoundSystem : MonoBehaviour
    {
        [SerializeField]
        private AudioSource _globalAudioSource;
        internal AudioSource AudioSource => _globalAudioSource;
        private AudioClip _globalAudioClip { get; set; }
        [SerializeField]
        private AudioClip _selectedSound;
        [SerializeField]
        private AudioClip _beepSound;

        private void Start()
        {
            SceneManager.sceneLoaded += SetVolume;
        }

        private void SetVolume(Scene scene, LoadSceneMode mode)
        {
            SetVolumeEach("Music", GlobalData.Settings.MusicVolume);
            SetVolumeEach("Sound", GlobalData.Settings.SoundVolume);

            void SetVolumeEach(string tag, float volume)
            {
                foreach (var audioObj in GameObject.FindGameObjectsWithTag(tag))
                {
                    foreach (var audio in audioObj.GetComponents<AudioSource>())
                    {
                        audio.volume = volume;
                    }
                }
            }
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
