using UnityEngine;

namespace PataRoad.GameSound
{
    /// <summary>
    /// Plays all "speaking" sounds on action - when get item, on fire status effect, die etc.
    /// </summary>
    public class SpeakManager : MonoBehaviour
    {
        private AudioSource _audioSource;
        public static SpeakManager Current { get; private set; }
        // Start is called before the first frame update
        void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            Current = this;
        }
        private void OnDestroy()
        {
            Current = null;
        }
        public void Play(AudioClip clip) => _audioSource.PlayOneShot(clip);
    }
}
