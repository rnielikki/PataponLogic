using UnityEngine;
using PataRoad.Core.Rhythm;
using PataRoad.Core.Rhythm.Command;

namespace PataRoad.SceneLogic.Intro
{
    class TutorialCountPonDrum : MonoBehaviour
    {
        private int _count;
        private bool _listening = true;
        private bool _listeningTurn = false;
        [SerializeField]
        private RhythmInput _input;
        [SerializeField]
        private UnityEngine.Events.UnityEvent _onDrumNoInput;
        [SerializeField]
        private UnityEngine.Events.UnityEvent _onDrumCanceled;
        [SerializeField]
        private UnityEngine.Events.UnityEvent _onFirstPracticeEnd;

        //for command
        private bool _started;
        private bool _gotAnyCommandInput;
        private int _turnCount;
        [SerializeField]
        private UnityEngine.Events.UnityEvent _onCommandCanceled;
        [SerializeField]
        private UnityEngine.Events.UnityEvent _onCommand;
        [SerializeField]
        private UnityEngine.Events.UnityEvent _onPracticeEnd;
        private const int _practiceCount = 8;
        private const int _commandPracticeCount = 4;

        public void StartListen()
        {
            _started = false;
            _input.OnDrumHit.AddListener(ListenCount);
            RhythmTimer.Current.OnHalfTime.AddListener(() =>
            {
                if (!_gotAnyCommandInput)
                {
                    if (_started) _onDrumNoInput.Invoke();
                    _count = 0;
                }
                _gotAnyCommandInput = false;
            });
        }
        private void ListenCount(RhythmInputModel model)
        {
            _started = true;
            _gotAnyCommandInput = true;
            if (!_listening) return;
            if (model.Status != DrumHitStatus.Miss)
            {
                _count++;
            }
            else
            {
                _onDrumCanceled.Invoke();
                _count = 0;
            }
            if (_count > _practiceCount)
            {
                _count = 0;
                _input.OnDrumHit.RemoveListener(ListenCount);
                _onFirstPracticeEnd.Invoke();
                _input.gameObject.SetActive(false);
            }
        }
        public void StartTurn()
        {
            _started = false;
            _input.gameObject.SetActive(true);
            _input.OnDrumHit.AddListener(PerformTurn);
            RhythmTimer.Current.OnHalfTime.AddListener(() =>
            {
                if (!TurnCounter.IsPlayerTurn) return;
                else if (!_gotAnyCommandInput)
                {
                    if (_started) _onCommandCanceled.Invoke();
                    _count = 0;
                    _turnCount = 0;
                }
                _gotAnyCommandInput = false;
            });
        }
        private void PerformTurn(RhythmInputModel model)
        {
            if (model.Status == DrumHitStatus.Miss)
            {
                _onCommandCanceled.Invoke();
                _count = 0;
                _turnCount = 0;
            }
            _count++;
            _started = true;
            _gotAnyCommandInput = true;
            if (_count == 4)
            {
                if (!TurnCounter.IsOn)
                {
                    RhythmTimer.Current.OnNextHalfTime.AddListener(TurnCounter.Start);
                }
                TurnCounter.OnNextTurn.AddListener(() =>
                {
                    _onCommand.Invoke();
                    _count = 0;
                    if (_turnCount > _commandPracticeCount)
                    {
                        _input.OnDrumHit.RemoveListener(PerformTurn);
                        RhythmTimer.Current.StopAndRemoveAllListeners();
                        _input.gameObject.SetActive(false);
                        _onPracticeEnd.Invoke();
                    }
                    else _turnCount++;
                });
            }
        }
    }
}
