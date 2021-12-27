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
        private static MinigameData _data;
        [SerializeField]
        private MinigameDrumGrid[] _grids;

        [SerializeReference]
        private DonChakaVoicePlayer _voicePlayer = new DonChakaVoicePlayer();

        [SerializeReference]
        private MinigameResultDisplay _minigameResultDisplay;

        private static UnityEngine.Events.UnityEvent _onMinigameEnd;

        //--- on hit
        private MinigameDrumType _lastRequiredDrum;
        private bool _gotAnyInput;
        public bool ListeningInput { get; private set; }
        private int _lastHitIndex;

        //offset from zero. zero frequency is best hit. max is half of frequency. smaller is better.
        private readonly System.Collections.Generic.List<int> _frequencyOffset = new System.Collections.Generic.List<int>();

        public static void Init(MinigameModel model, UnityEngine.Events.UnityEvent onMinigameEnd)
        {
            _model = model;
            _data = model.MinigameData;
            _onMinigameEnd = onMinigameEnd;
        }
        public void LoadGame()
        {
            _voicePlayer.Init(_audioSource);
            RhythmTimer.Current.OnNext.AddListener(() => StartCoroutine(StartGame()));
        }
        private IEnumerator StartGame()
        {
            foreach (MinigameTurn turn in _data.MinigameTurns)
            {
                yield return PerformTurn(turn, _data.UseDonChakaGameSound);

                //for example, PON-PON-PON-
                if (turn.Drums[turn.Drums.Length - 1] == MinigameDrumType.Empty)
                {
                    yield return new WaitForRhythmTime(RhythmTimer.HalfFrequency);
                    yield return new WaitForRhythmTime(0);
                }
                _audioSource.PlayOneShot(_endSound);

                yield return new WaitForRhythmTime(0);
                yield return new WaitForRhythmTime(RhythmTimer.HalfFrequency);
                yield return new WaitForRhythmTime(0);
                yield return new WaitForRhythmTime(RhythmTimer.HalfFrequency);
                yield return ListenTurn(turn);

                for (int i = 0; i < _grids.Length; i++)
                {
                    _grids[i].ClearStatus();
                }
            }
            yield return new WaitForRhythmTime(RhythmTimer.HalfFrequency);
            yield return new WaitForRhythmTime(0);

            _minigameResultDisplay.UpdateResult(_model, _frequencyOffset.Average(offset => (1 - (float)offset / RhythmTimer.HalfFrequency)), _onMinigameEnd);
        }

        private IEnumerator PerformTurn(MinigameTurn turn, bool useDonChakaGameSound)
        {
            if (!useDonChakaGameSound) _audioSource.PlayOneShot(turn.Sound);
            for (int i = 0; i < turn.Drums.Length; i++)
            {
                yield return new WaitForRhythmTime(RhythmTimer.HalfFrequency);
                yield return new WaitForRhythmTime(0);
                if (useDonChakaGameSound) _voicePlayer.Play(turn.Drums[i]);
                _grids[i].Load(turn.Drums[i]);
            }
            _voicePlayer.ClearTurn();
        }
        private IEnumerator ListenTurn(MinigameTurn turn)
        {
            for (int i = 0; i < turn.Drums.Length; i++)
            {
                _gotAnyInput = false;
                _lastHitIndex = i;
                _lastRequiredDrum = turn.Drums[i];

                ListeningInput = true;
                yield return new WaitForRhythmTime(0);
                yield return new WaitForRhythmTime(RhythmTimer.HalfFrequency);
                ListeningInput = false;

                if (_lastRequiredDrum != MinigameDrumType.Empty && !_gotAnyInput)
                {
                    _grids[i].Disappear();
                    _frequencyOffset.Add(0);
                }
            }
            _lastRequiredDrum = MinigameDrumType.Empty;
            _gotAnyInput = false;
            _lastHitIndex = -1;
            yield return new WaitForRhythmTime(RhythmTimer.HalfFrequency);
            yield return new WaitForRhythmTime(0);
        }

        public void CheckDrum(RhythmInputModel inputModel)
        {
            if (_lastRequiredDrum != MinigameDrumType.Empty && !_gotAnyInput &&
                    (int)inputModel.Drum == (int)_lastRequiredDrum)
            {
                _gotAnyInput = true;
                _grids[_lastHitIndex].Hit((1 - (float)inputModel.Timing / RhythmTimer.HalfFrequency));
                _frequencyOffset.Add(inputModel.Timing);
            }
        }
        private void OnDestroy()
        {
            _model = null;
            _data = null;
            _onMinigameEnd = null;
        }
    }
}
