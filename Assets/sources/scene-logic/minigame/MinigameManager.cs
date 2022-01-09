using PataRoad.Core.Rhythm;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace PataRoad.SceneLogic.Minigame
{
    public class MinigameManager : MonoBehaviour
    {
        [SerializeField]
        private AudioSource _audioSource;
        [SerializeField]
        private AudioClip _endSound;
        private static MinigameModel _model { get; set; }
        private static MinigameData _data { get; set; }
        [SerializeField]
        private MinigameDrumGrid[] _grids;

        [SerializeReference]
        private DonChakaVoicePlayer _voicePlayer = new DonChakaVoicePlayer();

        [SerializeReference]
        private MinigameResultDisplay _minigameResultDisplay;

        [SerializeField]
        [Tooltip("Minimum hit percentage, value of 0-1 expected")]
        private float _minimumHit = 0.15f;
        private int _minimumHitFrequency;
        [SerializeField]
        private AudioSource _music;

        //--- on hit
        private MinigameDrumType _lastRequiredDrum;
        private bool _gotAnyInput;
        public bool ListeningInput { get; private set; }
        private int _lastHitIndex;

        //offset from zero. zero frequency is best hit. max is half of frequency. smaller is better.
        private readonly System.Collections.Generic.List<int> _frequencyOffset = new System.Collections.Generic.List<int>();

        private int _currentIndex;
        private MinigameTurn _currentTurn => _data.MinigameTurns[_currentIndex];

        public static void Init(MinigameModel model)
        {
            _model = model;
            _data = model.MinigameData;
        }
        public void LoadGame()
        {
            _minimumHitFrequency = (int)(_minimumHit * RhythmTimer.HalfFrequency);
            _voicePlayer.Init(_audioSource);
            RhythmTimer.Current.OnNext.AddListener(PerformTurn);
            if (_data.Music != null) _music.clip = _data.Music;
            _music.Play();
        }
        private void PerformTurn()
        {
            var turn = _currentTurn;
            if (!_data.UseDonChakaGameSound) _audioSource.PlayOneShot(turn.Sound);
            int i = 0;
            int lastTiming = turn.Drums.Length > 3 ? 6 : 3;
            int nextTiming = lastTiming > 3 ? 7 : 3;

            RhythmTimer.Current.OnTime.AddListener(PlayTurn);

            void PlayTurn()
            {
                if (i < turn.Drums.Length)
                {
                    if (_data.UseDonChakaGameSound) _voicePlayer.Play(turn.Drums[i]);
                    _grids[i].Load(turn.Drums[i]);
                }
                if (i == lastTiming)
                {
                    _voicePlayer.ClearTurn();
                    _audioSource.PlayOneShot(_endSound);
                }
                if (i == nextTiming)
                {
                    RhythmTimer.Current.OnTime.RemoveListener(PlayTurn);
                    RhythmTimer.Current.OnNext.AddListener(ListenTurn);
                }
                i++;
            }
        }
        private void ListenTurn()
        {
            var turn = _currentTurn;
            ListeningInput = false;
            int i = 0;
            int lastTiming = turn.Drums.Length > 3 ? 6 : 3;
            int nextTiming = lastTiming > 3 ? 7 : 3;
            RhythmTimer.Current.OnHalfTime.AddListener(ListenThis);


            void ListenThis()
            {
                //Calculate
                if (ListeningInput && !_gotAnyInput && _lastRequiredDrum != MinigameDrumType.Empty)
                {
                    _grids[_lastHitIndex].Disappear();
                    _frequencyOffset.Add(RhythmTimer.HalfFrequency);
                }
                //Listen
                if (i < turn.Drums.Length)
                {
                    _gotAnyInput = false;
                    _lastHitIndex = i;
                    _lastRequiredDrum = turn.Drums[i];
                    ListeningInput = true;
                }
                else
                {
                    if (i == lastTiming)
                    {
                        ListeningInput = false;
                        _lastRequiredDrum = MinigameDrumType.Empty;
                        _gotAnyInput = false;
                        _lastHitIndex = -1;
                    }
                    if (i == nextTiming)
                    {
                        RhythmTimer.Current.OnHalfTime.RemoveListener(ListenThis);
                        _currentIndex++;
                        for (int j = 0; j < _grids.Length; j++)
                        {
                            _grids[j].ClearStatus();
                        }
                        if (_data.MinigameTurns.Length > _currentIndex)
                        {
                            RhythmTimer.Current.OnNext.AddListener(PerformTurn);
                        }
                        else
                        {
                            RhythmTimer.Current.OnNext.AddListener(ShowResult);
                        }
                    }
                }
                i++;
            }
        }
        private void ShowResult()
        {
            _audioSource.Stop();
            _music.Stop();
            _minigameResultDisplay.UpdateResult(_model, _frequencyOffset.Average(offset => (1 - (float)offset / RhythmTimer.HalfFrequency)));
        }

        public void CheckDrum(RhythmInputModel inputModel)
        {
            if (_lastRequiredDrum != MinigameDrumType.Empty && !_gotAnyInput &&
                    (int)inputModel.Drum == (int)_lastRequiredDrum)
            {
                _gotAnyInput = true;
                var timing = Mathf.Max(_minimumHitFrequency, inputModel.Timing);
                _grids[_lastHitIndex].Hit((float)timing / RhythmTimer.HalfFrequency);
                _frequencyOffset.Add(timing);
            }
        }
        private void OnDestroy()
        {
            _model = null;
            _data = null;
        }
        private void OnValidate()
        {
            if (_minimumHit < 0 || _minimumHit > 1)
            {
                throw new System.ArgumentException("Minimum hit must be range of 0-1");
            }
        }
    }
}
