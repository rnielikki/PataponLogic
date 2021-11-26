using PataRoad.Core.Rhythm.Command;
using System.Collections.Generic;
using UnityEngine;

namespace PataRoad.Core.Rhythm.Bgm
{
    /*
     * (theme name)/song directory contains
     * /before_fever (directory): patapata, ponpon, chakachaka
     * patapata1.wav, patapata2.wav
     * ponpon1.wav, ponpon2.wav
     * chakachaka1.wav, chakachaka2.wav
     * other commands are "common sound": on /themes/common/song contains
     * - ponpata.wav, ponchaka.wav, dondon.wav, donchaka.wav, patachaka.wav / also with "-fever" suffix on file name / and normal "patapata, ponpon, chakachaka" (sound without fever chance)
     * make sure any one of them are not omitted.
     * 
     * Also, it doesn't need to check very precisely 2 seconds unlike bgm player.
     * All of singing sounds are extracted with PSound (if there're other way to extract some sources that we don't have, other methods are welcomed!)
     */
    /// <summary>
    /// Defines Patapons song on command.
    /// </summary>
    public class RhythmBgmSinging : MonoBehaviour
    {
        [SerializeField]
        RhythmBgmPlayer _player;
        [SerializeField]
        AudioClip _singingMiracle;
        private string _themeName;
        AudioSource _audioSource;

        private Dictionary<CommandSong, AudioClip> _audioClipsNoFever;
        private Dictionary<CommandSong, AudioClip> _audioClipsMayFever;
        private Dictionary<CommandSong, AudioClip[]> _audioClipsFever;
        private AudioClip _feverShout;

        private int _offset; //on Fever, determines 1 (second music) is called first.
        private bool _canSing = true;

        // Start is called before the first frame update
        void Awake()
        {
            _themeName = _player.MusicTheme;
            _audioSource = GetComponent<AudioSource>();
            _audioSource.velocityUpdateMode = AudioVelocityUpdateMode.Fixed;
            InitAudioClips();
        }
        public void StopSinging() => _canSing = false;
        public void Sing(RhythmCommandModel command)
        {
            if (!_canSing) return;
            AudioClip clip;
            switch (command.ComboType)
            {
                case ComboStatus.Fever:
                    _offset = (_offset + 1) % 2;
                    var commandClips = _audioClipsFever[command.Song];
                    if (commandClips.Length > 1)
                    {
                        clip = commandClips[_offset];
                    }
                    else
                    {
                        clip = commandClips[0];
                    }
                    break;
                case ComboStatus.MayFever:
                    switch (command.Song)
                    {
                        case CommandSong.Patapata:
                        case CommandSong.Ponpon:
                        case CommandSong.Chakachaka:
                            clip = _audioClipsMayFever[command.Song];
                            break;
                        default:
                            clip = _audioClipsNoFever[command.Song];
                            break;
                    }
                    break;
                default:
                    clip = _audioClipsNoFever[command.Song];
                    break;
            }
            _audioSource.clip = clip;
            _audioSource.Play();
        }
        public void SingMiracle() => _audioSource.PlayOneShot(_singingMiracle);
        public void ShoutFever()
        {
            //A bit delay needed to add listener in right time
            RhythmTimer.OnNextHalfTime.AddListener(() =>
                TurnCounter.OnNextTurn.AddListener(() => _audioSource.PlayOneShot(_feverShout))
            );
        }

        public void End()
        {
            _audioSource.Stop();
            _offset = 0;
        }
        private void InitAudioClips()
        {
            _audioClipsNoFever = new Dictionary<CommandSong, AudioClip>()
            {
                { CommandSong.Patapata, Resources.Load(RhythmEnvironment.ThemePath + "common/song/patapata") as AudioClip },
                { CommandSong.Ponpon, Resources.Load(RhythmEnvironment.ThemePath + "common/song/ponpon") as AudioClip },
                { CommandSong.Chakachaka, Resources.Load(RhythmEnvironment.ThemePath + "common/song/chakachaka") as AudioClip },
                { CommandSong.Ponpata, Resources.Load(RhythmEnvironment.ThemePath + "common/song/ponpata") as AudioClip },
                { CommandSong.Ponchaka, Resources.Load(RhythmEnvironment.ThemePath + "common/song/ponchaka") as AudioClip },
                { CommandSong.Dondon, Resources.Load(RhythmEnvironment.ThemePath + "common/song/dondon") as AudioClip },
                { CommandSong.Donchaka, Resources.Load(RhythmEnvironment.ThemePath + "common/song/donchaka") as AudioClip },
                { CommandSong.Patachaka, Resources.Load(RhythmEnvironment.ThemePath + "common/song/patachaka") as AudioClip },
            };
            _audioClipsMayFever = new Dictionary<CommandSong, AudioClip>()
            {
                { CommandSong.Patapata, Resources.Load(RhythmEnvironment.ThemePath + _themeName + "/song/before_fever/patapata") as AudioClip },
                { CommandSong.Ponpon, Resources.Load(RhythmEnvironment.ThemePath + _themeName + "/song/before_fever/ponpon") as AudioClip },
                { CommandSong.Chakachaka, Resources.Load(RhythmEnvironment.ThemePath + _themeName + "/song/before_fever/chakachaka") as AudioClip },
            };
            _audioClipsFever = new Dictionary<CommandSong, AudioClip[]>()
            {
                { CommandSong.Patapata,
                    new[]{
                        Resources.Load(RhythmEnvironment.ThemePath + _themeName + "/song/patapata1") as AudioClip,
                        Resources.Load(RhythmEnvironment.ThemePath + _themeName + "/song/patapata2") as AudioClip
                    }
                },
                { CommandSong.Ponpon,
                    new[]{
                        Resources.Load(RhythmEnvironment.ThemePath + _themeName + "/song/ponpon1") as AudioClip,
                        Resources.Load(RhythmEnvironment.ThemePath + _themeName + "/song/ponpon2") as AudioClip
                    }
                },
                { CommandSong.Chakachaka,
                    new[]{
                        Resources.Load(RhythmEnvironment.ThemePath + _themeName + "/song/chakachaka1") as AudioClip,
                        Resources.Load(RhythmEnvironment.ThemePath + _themeName + "/song/chakachaka2") as AudioClip
                    }
                },

                { CommandSong.Ponpata, new[]{ Resources.Load(RhythmEnvironment.ThemePath + "common/song/ponpata-fever") as AudioClip } },
                { CommandSong.Ponchaka, new []{ Resources.Load(RhythmEnvironment.ThemePath + "common/song/ponchaka-fever") as AudioClip } },
                { CommandSong.Dondon, new[] { Resources.Load(RhythmEnvironment.ThemePath + "common/song/dondon-fever") as AudioClip } },
                { CommandSong.Donchaka, new[]{ Resources.Load(RhythmEnvironment.ThemePath + "common/song/donchaka-fever") as AudioClip } },
                { CommandSong.Patachaka, new[]{ Resources.Load(RhythmEnvironment.ThemePath + "common/song/patachaka-fever") as AudioClip } }
            };
            _feverShout = Resources.Load(RhythmEnvironment.ThemePath + "common/fever") as AudioClip;
        }
    }
}
