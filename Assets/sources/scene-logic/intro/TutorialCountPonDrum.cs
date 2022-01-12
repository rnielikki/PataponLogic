using UnityEngine;
using PataRoad.Core.Rhythm;
using PataRoad.Core.Rhythm.Command;

namespace PataRoad.SceneLogic.Intro
{
    class TutorialCountPonDrum : MonoBehaviour
    {
        private int _count;
        [SerializeField]
        private RhythmInput _input;
        [SerializeField]
        private TutorialPonDisplay _display;
        [SerializeField]
        private string _startText;
        [SerializeField]
        private string _noInputText;
        [SerializeField]
        private string _canceledText;
        [SerializeField]
        private UnityEngine.Events.UnityEvent _onFirstPracticeEnd;

        //for command
        private bool _started;
        private bool _gotAnyCommandInput;
        private int _turnCount;
        [SerializeField]
        private GameObject _turnSingingObject;
        [SerializeField]
        private string _noCommandInputText;
        [SerializeField]
        private string _noPlayerTurnText;
        [SerializeField]
        private string _commandCanceledText;
        [SerializeField]
        private string _onCommandText;
        [SerializeField]
        private UnityEngine.Events.UnityEvent _onPracticeEnd;
        [SerializeField]
        private UnityEngine.UI.Image _playerTurnImage;
        [SerializeField]
        private UnityEngine.UI.Image _pataponTurnImage;
        private const int _practiceCount = 8;
        private const int _commandPracticeCount = 3;

        private Common.GameDisplay.TutorialDrumUpdater _drumUpdater;
        private bool _lastWasSuccessful;

        public void StartListen() => RhythmTimer.Current.OnNext.AddListener(StartListenPrivate);

        private void StartListenPrivate()
        {
            _started = false;
            _display.gameObject.SetActive(true);
            _display.LoadKeyName();
            _display.UpdateText(_startText);

            _input.OnDrumHit.AddListener(ListenCount);
            _drumUpdater = _display.StartDrumTutorial(_practiceCount);
            _lastWasSuccessful = true;

            RhythmTimer.Current.OnHalfTime.AddListener(() =>
            {
                if (!_gotAnyCommandInput)
                {
                    if (_started)
                    {
                        _display.UpdateText(_noInputText);
                        _drumUpdater.ResetHit();
                    }
                    _count = 0;
                }
                _gotAnyCommandInput = false;
            });
        }
        private void ListenCount(RhythmInputModel model)
        {
            _started = true;
            _gotAnyCommandInput = true;
            if (model.Status != DrumHitStatus.Miss)
            {
                _drumUpdater.PlayOnIndex(_count);
                _count++;
                _lastWasSuccessful = true;
            }
            else
            {
                if (_lastWasSuccessful) _display.UpdateText(_canceledText);
                _drumUpdater.ResetHit();
                _count = 0;
                _lastWasSuccessful = false;
            }
            if (_count >= _practiceCount)
            {
                _count = 0;

                RhythmTimer.Current.OnHalfTime.RemoveAllListeners();
                _input.OnDrumHit.RemoveListener(ListenCount);
                _display.gameObject.SetActive(false);
                _input.gameObject.SetActive(false);
                _drumUpdater.ResetHit();

                _onFirstPracticeEnd.Invoke();
            }
        }
        public void StartTurn() => RhythmTimer.Current.OnNext.AddListener(StartTurnPrivate);
        private void StartTurnPrivate()
        {
            _started = false;
            _turnSingingObject.SetActive(true);
            _input.gameObject.SetActive(true);
            _display.gameObject.SetActive(true);
            _display.StartTurnTutorial();

            _input.OnDrumHit.AddListener(PerformTurn);
            RhythmTimer.Current.OnHalfTime.AddListener(() =>
            {
                if (!TurnCounter.IsPlayerTurn) return;
                else if (!_gotAnyCommandInput)
                {
                    if (_started)
                    {
                        if (_lastWasSuccessful)
                        {
                            _display.UpdateText(_noCommandInputText);
                            _display.PlayFailedSound();
                        }
                        _drumUpdater.ResetHit();
                        TurnCounter.Stop();
                    }
                    _count = 0;
                    _turnCount = 0;
                    SwitchPlayerTurn(true);
                    _lastWasSuccessful = false;
                }
                _gotAnyCommandInput = false;
            });
        }
        private void PerformTurn(RhythmInputModel model)
        {
            if (model.Status == DrumHitStatus.Miss)
            {
                _display.UpdateText(TurnCounter.IsPlayerTurn ? _commandCanceledText : _noPlayerTurnText);
                _display.StopSinging(_lastWasSuccessful);
                SwitchPlayerTurn(true);

                _drumUpdater.ResetHit();
                TurnCounter.Stop();
                _count = 0;
                _turnCount = 0;
                _lastWasSuccessful = false;
                return;
            }
            _drumUpdater.PlayOnIndex(_count);
            _count++;
            _started = true;
            _gotAnyCommandInput = true;
            _lastWasSuccessful = true;

            if (_count == 4)
            {
                _count = 0;
                if (!TurnCounter.IsOn)
                {
                    RhythmTimer.Current.OnNextHalfTime.AddListener(TurnCounter.Start);
                }
                TurnCounter.OnNextTurn.AddListener(() =>
                {
                    _display.UpdateText(_onCommandText);
                    _display.SingOnTurn();
                    SwitchPlayerTurn(false);
                    if (_turnCount > _commandPracticeCount)
                    {
                        ListenLastTurn();
                    }
                    else
                    {
                        _turnCount++;
                    }
                    TurnCounter.OnPlayerTurn.AddListener(() => SwitchPlayerTurn(true));
                });
            }
        }
        private void SwitchPlayerTurn(bool isPlayerTurn)
        {
            _playerTurnImage.enabled = isPlayerTurn;
            _pataponTurnImage.enabled = !isPlayerTurn;
        }
        private void ListenLastTurn()
        {
            //sure way to listen last. if instantly hit wrong on last turn it'll cancel this
            TurnCounter.OnPlayerTurn.AddListener(PerformLastTurn);
            void PerformLastTurn()
            {
                _input.OnDrumHit.RemoveListener(PerformTurn);
                RhythmTimer.Current.StopAndRemoveAllListeners();
                _input.gameObject.SetActive(false);
                _onPracticeEnd.Invoke();
            }
        }
    }
}
