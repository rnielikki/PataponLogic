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
        bool _ready;

        internal void Init()
        {
            if (_ready) return;
            _ready = true;
            SceneManager.sceneLoaded += SetVolume;
        }
        private void SetVolume(Scene scene, LoadSceneMode mode)
        {
            foreach (var audioObj in Resources.FindObjectsOfTypeAll<AudioSource>())
            {
                var tag = audioObj.tag;
                float vol = 0;
                switch (tag)
                {
                    case "Music":
                        vol = GlobalData.Settings.MusicVolume;
                        break;
                    case "Sound":
                        vol = GlobalData.Settings.SoundVolume;
                        break;
                }
                foreach (var audio in audioObj.GetComponents<AudioSource>())
                {
                    audio.volume = vol;
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
