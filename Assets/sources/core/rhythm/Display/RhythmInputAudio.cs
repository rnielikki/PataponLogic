using System.Collections.Generic;
using UnityEngine;

namespace Core.Rhythm
{
    /// <summary>
    /// Called in <see cref="RhythmInput"/> method. Always follows with the input. Don't make it as Monobehaviour.
    /// </summary>
    internal class RhythmInputAudio : MonoBehaviour
    {
        [SerializeField]
        private AudioSource _audioSource;
        //so long dictionary, but fast search is important
        [SerializeField]
        private RhythmInput _input;
        private Dictionary<DrumHitStatus, AudioClip> Audio;
        private void Awake()
        {
            _audioSource.velocityUpdateMode = AudioVelocityUpdateMode.Fixed;
            string drumName = _input.DrumType.ToString();

            Audio = new Dictionary<DrumHitStatus, AudioClip>()
                {
                    { DrumHitStatus.Perfect, Resources.Load(RhythmEnvironment.DrumSoundPath + drumName + "-perfect") as AudioClip },
                    { DrumHitStatus.Good, Resources.Load(RhythmEnvironment.DrumSoundPath + drumName +  "-good") as AudioClip },
                    { DrumHitStatus.Bad, Resources.Load(RhythmEnvironment.DrumSoundPath + drumName +  "-bad") as AudioClip },
                    { DrumHitStatus.Miss, Resources.Load(RhythmEnvironment.DrumSoundPath + drumName +  "-miss") as AudioClip }
                };

            _input.OnDrumHit.AddListener(PlaySound);
        }

        private void PlaySound(RhythmInputModel model)
        {
            _audioSource.PlayOneShot(Audio[model.Status]);
        }
    }
}
