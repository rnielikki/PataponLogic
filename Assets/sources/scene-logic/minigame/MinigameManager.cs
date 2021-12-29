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

        //--- on hit
        private MinigameDrumType _lastRequiredDrum;
        private bool _gotAnyInput;
        public bool ListeningInput { get; private set; }
        private int _lastHitIndex;

        //offset from zero. zero frequency is best hit. max is half of frequency. smaller is better.
        private readonly System.Collections.Generic.List<int> _frequencyOffset = new System.Collections.Generic.List<int>();

        public static void Init(MinigameModel model)
        {
            _model = model;
            _data = model.MinigameData;
        }
        public void LoadGame()
        {
            _minimumHitFrequency = (int)(_minimumHit * RhythmTimer.HalfFrequency);
            _voicePlayer.Init(_audioSource);
            RhythmTimer.Current.OnNext.AddListener(() => StartCoroutine(StartGame()));
        }
        private IEnumerator StartGame()
        {
            yield return WaitForNextRhythmTime();
            foreach (MinigameTurn turn in _data.MinigameTurns)
            {
                yield return PerformTurn(turn, _data.UseDonChakaGameSound);
                yield return WaitForNextHalfRhythmTime();
                yield return ListenTurn(turn);

                for (int i = 0; i < _grids.Length; i++)
                {
                    _grids[i].ClearStatus();
                }
            }
            yield return new WaitForRhythmTime(0);

            _minigameResultDisplay.UpdateResult(_model, _frequencyOffset.Average(offset => (1 - (float)offset / RhythmTimer.HalfFrequency)));
        }

        private IEnumerator PerformTurn(MinigameTurn turn, bool useDonChakaGameSound)
        {
            Coroutine coroutine = StartCoroutine(WaitForNextRhythmTime());
            bool isLastEmpty = turn.Drums[turn.Drums.Length - 1] == MinigameDrumType.Empty;
            var multiCoroutine = StartCoroutine(WaitForNextMultipleRhythmTime(turn.Drums.Length + 1));
            if (!useDonChakaGameSound) _audioSource.PlayOneShot(turn.Sound);
            int i;
            for (i = 0; i < turn.Drums.Length; i++)
            {
                yield return coroutine;
                coroutine = StartCoroutine(WaitForNextRhythmTime());
                if (useDonChakaGameSound) _voicePlayer.Play(turn.Drums[i]);
                _grids[i].Load(turn.Drums[i]);
            }
            _voicePlayer.ClearTurn();
            if (isLastEmpty)
            {
                yield return multiCoroutine;
                _audioSource.PlayOneShot(_endSound);
                yield return WaitForNextRhythmTime();
            }
            else //for example, PON-PON-PON-
            {
                _audioSource.PlayOneShot(_endSound);
                yield return multiCoroutine;
            }
        }
        private IEnumerator ListenTurn(MinigameTurn turn)
        {
            Coroutine coroutine = StartCoroutine(WaitForNextHalfRhythmTime());
            for (int i = 0; i < turn.Drums.Length; i++)
            {
                _gotAnyInput = false;
                _lastHitIndex = i;
                _lastRequiredDrum = turn.Drums[i];

                ListeningInput = true;
                yield return coroutine;
                coroutine = StartCoroutine(WaitForNextHalfRhythmTime());
                ListeningInput = false;

                if (_lastRequiredDrum != MinigameDrumType.Empty && !_gotAnyInput)
                {
                    _grids[i].Disappear();
                    _frequencyOffset.Add(RhythmTimer.HalfFrequency);
                }
            }
            _lastRequiredDrum = MinigameDrumType.Empty;
            _gotAnyInput = false;
            _lastHitIndex = -1;
            yield return coroutine;
            yield return new WaitForRhythmTime(0);
        }
        private IEnumerator WaitForNextRhythmTime()
        {
            yield return new WaitForRhythmTime(RhythmTimer.Frequency - 1);
            yield return new WaitForRhythmTime(0);
        }
        private IEnumerator WaitForNextMultipleRhythmTime(int time)
        {
            for (int i = 0; i < time; i++)
            {
                yield return WaitForNextRhythmTime();
            }
        }
        private IEnumerator WaitForNextHalfRhythmTime()
        {
            yield return new WaitForRhythmTime(RhythmTimer.HalfFrequency - 1);
            yield return new WaitForRhythmTime(RhythmTimer.HalfFrequency);
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
