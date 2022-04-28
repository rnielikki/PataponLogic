using PataRoad.Core.Rhythm.Command;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PataRoad.Core.Rhythm.Bgm
{
    /*
     * Each theme folder must include:
     * song (directory), See details on "RhythmBgmSinging.cs"
     * intro.ogg [4s] : first play when the game is loaded
     * base.ogg : no command input
     * command.ogg : command input, but not yet chance to enter fever
     * before-fever-intro.ogg [4s] : command input, first chance to enter fever
     * before-fever.ogg : command input, chance to enter fever
     * fever-intro.ogg [4s] : before shouting "Fever!"
     * fever.ogg : music when on fever status
     * 
     * DON'T USE MP3. MP3 FILE HAS REPEAT DELAY PROBLEM.

     * NOTE: BE PRECISE TO SOURCE, KEEP ALL THE MUSIC TO %2==0 SECONDS! (except intro - still remember that intro is around 4s)
     * The whole rhythm script logic is based on *hit with no delay* so keep it in mind!
     * -> And hear start and end part e.g. for fever, 00:00 - 00:04:00, 01:00 - 01:04 to make sure that the music is on time.
     * 
     * Here is about time toolbar : https://manual.audacityteam.org/man/time_toolbar.html
     * You can also set exact time on "change speed" effect
    */
    /// <summary>
    /// Background music player. Also controls fever status music and Patapon sounds.
    /// </summary>
    public class RhythmBgmPlayer : MonoBehaviour
    {
        [SerializeField]
        AudioClip _feverEndSound;
        /// <summary>
        /// This MUST be initialized BEFORE this is initialized.
        /// </summary>
        public string MusicTheme { get; set; }
        private AudioSource _bgmShotSource; //for "Playing once"
        private AudioSource _bgmSource;
        private AudioSource _feverSource;

        //All music sources are pre-loaded and indexed. This helps fast search for corresponding status, in runtime.
        private Dictionary<RhythmBgmIndex, AudioClip> _audioClips;

        private float _defaultVolume;
        private bool _hadFeverChance = false; //for fever chance intro
        private bool _willPlayBaseMusic;

        private void Awake()
        {
            if (MusicTheme == null)
            {
                throw new System.MissingFieldException(nameof(RhythmBgmPlayer), nameof(MusicTheme));
            }
            var sources = GetComponents<AudioSource>();
            _bgmShotSource = sources[0];
            _bgmSource = sources[1];
            _feverSource = sources[2];
            _defaultVolume = _feverSource.volume;

            //All sources should work on Fixed Update
            foreach (var src in sources) src.velocityUpdateMode = AudioVelocityUpdateMode.Fixed;

            _bgmShotSource.loop = false;

            SetClips();
        }
        private void Start()
        {
            RhythmTimer.Current.OnStart.AddListener(() => ChangeMusicWithIntro(RhythmBgmIndex.Intro, RhythmBgmIndex.Base, _bgmSource));
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
                { RhythmBgmIndex.Fever, Resources.Load(RhythmEnvironment.ThemePath + MusicTheme + "/fever") as AudioClip }
            };
            if (_audioClips[0] == null)
            {
                throw new MissingReferenceException($"The music theme \"{MusicTheme}\" doesn't exist!");
            }
        }
        private void ChangeMusicWithIntro(RhythmBgmIndex introMusicIndex, RhythmBgmIndex musicIndex, AudioSource source)
        {
            var introMusic = _audioClips[introMusicIndex];
            source.clip = _audioClips[musicIndex];
            _bgmShotSource.PlayOneShot(introMusic);
            source.PlayDelayed(4);
        }
        private void ChangeMusic(RhythmBgmIndex bgmType)
        {
            _bgmSource.clip = _audioClips[bgmType];
            _bgmSource.Play();
        }

        public void PlayComboMusic(RhythmComboModel comboInfo)
        {
            bool feverChance = comboInfo.hasFeverChance;
            if (feverChance && !_hadFeverChance)
            {
                ChangeMusicWithIntro(RhythmBgmIndex.BeforeFeverIntro, RhythmBgmIndex.BeforeFever, _bgmSource);
            }
            else if (!feverChance)
            {
                ChangeMusic(RhythmBgmIndex.Command);
            }
            _hadFeverChance = feverChance;
        }
        public void PlayBaseMusic()
        {
            if (_willPlayBaseMusic) return;
            RhythmTimer.Current.OnNext.AddListener(ChangeToBaseMusic);
            _willPlayBaseMusic = true;
            void ChangeToBaseMusic()
            {
                ChangeMusic(RhythmBgmIndex.Base);
                _willPlayBaseMusic = false;
            }
        }
        public void PlayFever()
        {
            _bgmSource.Stop();
            _feverSource.volume = _defaultVolume;
            ChangeMusicWithIntro(RhythmBgmIndex.FeverIntro, RhythmBgmIndex.Fever, _feverSource);
        }
        public void StopPlayingFever()
        {
            if (_feverSource.isPlaying)
            {
                ChangeMusic(RhythmBgmIndex.Base);
                _bgmShotSource.PlayOneShot(_feverEndSound);
                StartCoroutine(FeverFadeOut());
            }
        }
        public void StopAllMusic()
        {
            _bgmSource.Stop();
            _feverSource.Stop();
        }
        private IEnumerator FeverFadeOut()
        {
            int countOffset = (RhythmTimer.Frequency - RhythmTimer.Count) + (TurnCounter.TurnCount + 4) * RhythmTimer.Frequency;

            float fadeOutScale = _feverSource.volume / countOffset;

            for (int i = 0; i < countOffset; i++)
            {
                _feverSource.volume -= fadeOutScale;
                yield return new WaitForFixedUpdate();
            }
            _feverSource.Stop();
        }
    }
}
