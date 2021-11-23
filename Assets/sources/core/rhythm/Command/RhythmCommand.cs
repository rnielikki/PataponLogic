using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace PataRoad.Core.Rhythm.Command
{
    /// <summary>
    /// Reads command. This is separated, because some storyline (e.g. very first) requires just PON-PON-, fore example.
    /// Attach with <see cref="RhythmTimer"/>.
    /// </summary>
    public class RhythmCommand : MonoBehaviour
    {
        /// <summary>
        /// This is "input references" added from Unity Editor - It DOESN'T contain how accurate the drum was, what kind of drum was etc.
        /// </summary>
        [SerializeField]
        private RhythmInput[] _rhythmInputs;

        //Perfect sound should be played *right after* command is perfect!
        [SerializeField]
        private AudioClip _perfectSound;
        [SerializeField]
        private AudioSource _audioSource;

        /// <summary>
        /// Invokes RIGHT AFTER the perfect command input.
        /// </summary>
        [SerializeField]
        [Header("RIGHT AFTER getting the perfect command, without delay.")]
        private UnityEvent<RhythmCommandModel> _onPerfectEnd;

        /// <summary>
        /// Invokes when command is complete e.g. one "PATA PON DON CHAKA".
        /// </summary>
        [Header("Command Events")]
        public UnityEvent<RhythmCommandModel> OnCommandInput; //I hate it but Unity Editor ignores properties
        /// <summary>
        /// Invokes when command is canceled.
        /// </summary>
        public UnityEvent OnCommandCanceled;

        /// <summary>
        /// If any valid drum is sent in turn. default is <c>false</c>. ONE drum (in right time) is enough to activate this as <c>true</c>.
        /// <note>This is checked every time in player turn + on next timer interval. Initialized when turn is changed.</note>
        /// </summary>
        private bool _gotAnyCommandInput;

        [SerializeReference]
        public RhythmCombo ComboManager = new RhythmCombo();

        private CommandListData _data;

        private readonly Queue<RhythmInputModel> _currentHits = new Queue<RhythmInputModel>();

        [SerializeField]
        private MiracleListener _miracleListener;

        /// <summary>
        /// This also counts first command, before <see cref="TurnCounter.IsOn"/>
        /// </summary>
        private bool _started;

        // Start is called before the first frame update
        private void Awake()
        {
            _onPerfectEnd.AddListener((_) => _audioSource.PlayOneShot(_perfectSound));
            OnCommandCanceled.AddListener(ComboManager.EndCombo);
            OnCommandCanceled.AddListener(() =>
            {
                RhythmTimer.OnNextHalfTime.RemoveListener(TurnCounter.Start);
            });
            // --------------- Command sent check start
            OnCommandCanceled.AddListener(() =>
            {
                _started = false;
            });
            RhythmTimer.OnHalfTime.AddListener(() =>
            {
                if (!TurnCounter.IsPlayerTurn) return;
                else if (!_gotAnyCommandInput)
                {
                    if (_started) OnCommandCanceled.Invoke();
                    ClearDrumHits();
                }
                _gotAnyCommandInput = false;
            });
            // --------------- Command sent check end

            //Initalizes drum beats
            //Will loaded based on game progress
            _data = new CommandListData(
                new CommandSong[]
                {
                    CommandSong.Patapata,CommandSong.Ponpon, CommandSong.Chakachaka, CommandSong.Dondon, CommandSong.Ponchaka, CommandSong.Ponpata
                }
                );
        }

        private void AddDrumHit(RhythmInputModel inputModel)
        {
            if (inputModel.Status == DrumHitStatus.Miss)
            {
                OnCommandCanceled.Invoke();
                ClearDrumHits();
            }
            else
            {
                EnqueueInputModel(inputModel);
                CheckCommand(inputModel);
            }
        }
        private void OnEnable()
        {
            OnCommandCanceled.AddListener(TurnCounter.Stop);
            OnCommandCanceled.AddListener(_miracleListener.ResetMiracle);

            foreach (var rhythmInput in _rhythmInputs)
            {
                rhythmInput.OnDrumHit.AddListener(AddDrumHit);
            }
        }
        private void OnDisable()
        {
            ComboManager.EndComboImmediately();
            OnCommandCanceled.RemoveListener(TurnCounter.Stop);
            OnCommandCanceled.RemoveListener(_miracleListener.ResetMiracle);

            foreach (var rhythmInput in _rhythmInputs)
            {
                rhythmInput.OnDrumHit.RemoveListener(AddDrumHit);
            }
        }
        private void CheckCommand(RhythmInputModel inputModel)
        {
            //I don't know maybe there are better way...
            var drums = _currentHits.Select(hit => hit.Drum);
            _gotAnyCommandInput = true;
            bool acceptedAsCommand = false;

            if (CommandExists(drums, out CommandSong song))
            {
                _started = true;
                acceptedAsCommand = true;

                if (_currentHits.Count == 4)
                {
                    if (!TurnCounter.IsOn)
                    {
                        RhythmTimer.OnNextHalfTime.AddListener(TurnCounter.Start);
                    }
                    var model = new RhythmCommandModel(_currentHits, song);
                    model.ComboType = ComboManager.CountCombo(model);
                    if (model.PerfectCount == 4)
                    {
                        _onPerfectEnd.Invoke(model);
                    }
                    TurnCounter.OnNextTurn.AddListener(() =>
                    {
                        OnCommandInput.Invoke(model);
                        ClearDrumHits();
                    });
                }
            }
            if (_miracleListener.HasMiracleChance(drums, inputModel))
            {
                _started = true;
                acceptedAsCommand = true;

                if (_miracleListener.MiracleDrumCount == 5)
                {
                    TurnCounter.OnNextTurn.AddListener(() =>
                    {
                        _miracleListener.ResetMiracle();
                        _miracleListener.OnMiracle.Invoke();
                        ClearDrumHits();
                    });
                }
            }
            if (!acceptedAsCommand)
            {
                OnCommandCanceled.Invoke();
            }
        }
        public PracticingCommandListData ToPracticeMode(CommandSong song)
        {
            OnCommandCanceled?.Invoke();
            var data = new PracticingCommandListData(this, _data, song);
            _data = data;
            return data;
        }
        internal void SetCommandListData(CommandListData data) => _data = data;
        public PracticingMiracleListener ToMiraclePracticeMode()
        {
            var practicingMiracleListener = _miracleListener.gameObject
                .AddComponent<PracticingMiracleListener>()
                .LoadFromMiracleListener(this, _miracleListener);
            _miracleListener = practicingMiracleListener;
            return practicingMiracleListener;
        }
        internal void SetMiracleListener(MiracleListener listener)
        {
            _miracleListener.enabled = false;
            listener.enabled = true;
            _miracleListener = listener;
        }

        private bool CommandExists(IEnumerable<DrumType> drums, out CommandSong song) => _data.TryGetCommand(drums, out song);
        private void ClearDrumHits() => _currentHits.Clear();
        private void EnqueueInputModel(RhythmInputModel model)
        {
            _currentHits.Enqueue(model);
            if (_currentHits.Count > 4) _currentHits.Dequeue();
        }
        private void OnDestroy()
        {
            _onPerfectEnd.RemoveAllListeners();
            OnCommandInput.RemoveAllListeners();
            OnCommandCanceled.RemoveAllListeners();
            _miracleListener.OnMiracle.RemoveAllListeners();
            TurnCounter.Destroy();
            ComboManager.Destroy();
        }
    }
}
