using System.Collections.Generic;
using UnityEngine;

namespace Core.Rhythm.Bgm
{
    /*
     * Each theme folder must include:
     * intro.mp3 : first play when the game is loaded
     * base.mp3 : no command input
     * command.mp3 : command input, but not yet chance to enter fever
     * combo-intro.mp3 : command input, first chance to enter fever
     * combo.mp3 : command input, chance to enter fever
     * fever-intro.mp3 : before shouting "Fever!"
     * fever.mp3 : music when on fever status

     * NOTE: BE PRECISE TO SOURCE, KEEP ALL THE MUSIC TO %2==0 SECONDS!
    */
    /// <summary>
    /// Background music player. Also controls fever status music and Patapon sounds.
    /// </summary>
    // for more tips about scheduled (seamless) play, see https://gamedevbeginner.com/ultimate-guide-to-playscheduled-in-unity/
    internal class RhythmBgmPlayer : MonoBehaviour
    {
        //Bgm name, Serialize Field
        [SerializeField]
        private string MusicTheme;
        //Should be removed later
        private AudioSource _audioSource;
        //All music sources are pre-loaded and indexed. This helps fast search for corresponding status, in runtime.
        private Dictionary<RhythmBgmIndex, AudioClip> _audioClips;
        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _audioSource.velocityUpdateMode = AudioVelocityUpdateMode.Fixed;
            SetClips();
            ChangeMusicWithIntro(RhythmBgmIndex.Intro, RhythmBgmIndex.Base);
        }
        private void SetClips()
        {
            _audioClips = new Dictionary<RhythmBgmIndex, AudioClip>()
            {
                { RhythmBgmIndex.Intro, Resources.Load(RhythmEnvironment.ThemePath + MusicTheme + "/intro") as AudioClip },
                { RhythmBgmIndex.Base, Resources.Load(RhythmEnvironment.ThemePath + MusicTheme + "/base") as AudioClip },
                { RhythmBgmIndex.Command, Resources.Load(RhythmEnvironment.ThemePath + MusicTheme + "/command") as AudioClip },
                { RhythmBgmIndex.BeforeFeverIntro, Resources.Load(RhythmEnvironment.ThemePath + MusicTheme + "/before-fever-intro") as AudioClip },
                { RhythmBgmIndex.BeforeFever, Resources.Load(RhythmEnvironment.ThemePath + MusicTheme + "/before-fever") as AudioClip },
                { RhythmBgmIndex.FeverIntro, Resources.Load(RhythmEnvironment.ThemePath + MusicTheme + "/fever-intro") as AudioClip },
                { RhythmBgmIndex.Fever, Resources.Load(RhythmEnvironment.ThemePath + MusicTheme + "/fever") as AudioClip },
            };
        }
        private void ChangeMusicWithIntro(RhythmBgmIndex introMusicIndex, RhythmBgmIndex musicIndex)
        {
            var introMusic = _audioClips[introMusicIndex];
            var music = _audioClips[musicIndex];
            _audioSource.Stop();
            _audioSource.clip = music;
            _audioSource.PlayOneShot(introMusic);
            _audioSource.PlayDelayed(introMusic.length);
        }
        private void ChangeMusic(RhythmBgmIndex bgmType)
        {
            _audioSource.clip = _audioClips[bgmType];
            _audioSource.Play();
        }
        public void PlayCommandMusic() => ChangeMusic(RhythmBgmIndex.Command);
        public void PlayBaseMusic() => ChangeMusic(RhythmBgmIndex.Base);
        public void PlayBeforeFever() => ChangeMusicWithIntro(RhythmBgmIndex.BeforeFeverIntro, RhythmBgmIndex.BeforeFever);
        public void PlayFever() => ChangeMusic(RhythmBgmIndex.Fever);
    }
}
