using PataRoad.Core.Rhythm;
using System.Collections;
using UnityEngine;

namespace PataRoad.SceneLogic.Minigame
{
    public class MinigameManager : MonoBehaviour
    {
        [SerializeField]
        private AudioSource _audioSource;
        [SerializeField]
        private AudioClip _endSound;
        private static MinigameModel _model;
        private static MinigameData _data;
        [SerializeField]
        private MinigameDrumGrid[] _grids;

        [SerializeReference]
        private DonChakaVoicePlayer _voicePlayer = new DonChakaVoicePlayer();

        //--- on hit
        private MinigameDrumType _lastRequiredDrum;
        private bool _gotAnyInput;
        private int _lastHitIndex;

        private readonly System.Collections.Generic.List<float> _accuracies = new System.Collections.Generic.List<float>();

        public static void SetModel(MinigameModel model)
        {
            _model = model;
            _data = model.MinigameData;
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
                _audioSource.PlayOneShot(_endSound);

                //for example, PON-PON-PON-
                if (turn.Drums[turn.Drums.Length - 1] == MinigameDrumType.Empty)
                {
                    yield return new WaitForRhythmTime(RhythmTimer.HalfFrequency);
                    yield return new WaitForRhythmTime(0);
                }

                yield return new WaitForRhythmTime(0);
                yield return new WaitForRhythmTime(RhythmTimer.HalfFrequency);
                yield return ListenTurn(turn);
            }
        }

        private IEnumerator PerformTurn(MinigameTurn turn, bool useDonChakaGameSound)
        {
            if (!useDonChakaGameSound) _audioSource.PlayOneShot(turn.Sound);
            for (int i = 0; i < turn.Drums.Length; i++)
            {
                if (useDonChakaGameSound) _voicePlayer.Play(turn.Drums[i]);
                _grids[i].Load(turn.Drums[i]);
                yield return new WaitForRhythmTime(RhythmTimer.HalfFrequency);
                yield return new WaitForRhythmTime(0);
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

                yield return new WaitForRhythmTime(0);
                yield return new WaitForRhythmTime(RhythmTimer.HalfFrequency);

                if (_lastRequiredDrum != MinigameDrumType.Empty && !_gotAnyInput)
                {
                    _grids[i].Disappear();
                    _accuracies.Add(0);
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
                _grids[_lastHitIndex].Hit();
                _accuracies.Add((float)inputModel.Timing / RhythmTimer.Frequency);
            }
        }
    }
}
