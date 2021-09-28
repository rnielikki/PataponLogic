using Core.Rhythm.Model;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Rhythm
{
    /// <summary>
    /// Called in <see cref="RhythmInput"/> method. Always follows with the input. Don't make it as Monobehaviour.
    /// </summary>
    internal class RhythmInputAudio
    {
        private static AudioSource _audioSource;
        //so long dictionary, but fast search is important
        private readonly Dictionary<DrumHitStatus, AudioClip> Audio;
        /// <summary>
        /// Creates new audio player for the drum.
        /// </summary>
        /// <param name="drumType"></param>
        /// <param name="sender">The Rhythm input.</param>
        internal RhythmInputAudio(DrumType drumType, RhythmInput sender)
        {
            string drumName = drumType.ToString();
            if (_audioSource == null)
            {
                _audioSource = GameObject.FindGameObjectWithTag("Sound").GetComponent<AudioSource>();
                _audioSource.velocityUpdateMode = AudioVelocityUpdateMode.Fixed;
            }
            Audio = new Dictionary<DrumHitStatus, AudioClip>()
                {
                    { DrumHitStatus.Perfect, Resources.Load(RhythmEnvironment.DrumSoundPath + drumName + "-perfect") as AudioClip },
                    { DrumHitStatus.Good, Resources.Load(RhythmEnvironment.DrumSoundPath + drumName +  "-good") as AudioClip },
                    { DrumHitStatus.Bad, Resources.Load(RhythmEnvironment.DrumSoundPath + drumName +  "-bad") as AudioClip },
                    { DrumHitStatus.Miss, Resources.Load(RhythmEnvironment.DrumSoundPath + drumName +  "-miss") as AudioClip }
                };

            sender.OnDrumHit.AddListener(PlaySound);
        }

        private void PlaySound(RhythmInputModel model)
        {
            _audioSource.PlayOneShot(Audio[model.Status]);
        }
    }
}
