/// <summary>
/// Plays metronome sound.
/// </summary>
namespace PataRoad.Core.Rhythm
{
    public class RhythmMetronome : UnityEngine.MonoBehaviour
    {
        [UnityEngine.SerializeField]
        UnityEngine.AudioClip _metronomeSound;
        public void StartMetronome()
        {
            var audioSource = GetComponent<UnityEngine.AudioSource>();
            RhythmTimer.Current.OnTime.AddListener(PlayMetronome);
            void PlayMetronome()
            {
                audioSource.PlayOneShot(_metronomeSound);
            }
        }
    }
}
