using Core.Rhythm.Model;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Core.Rhythm.Command
{
    /// <summary>
    /// Reads command. This is separated, because some storyline (e.g. very first) requires just PON-PON-, fore example.
    /// Attach with <see cref="RhythmTimer"/>.
    /// </summary>
    internal class RhythmCommand : MonoBehaviour
    {
        /// <summary>
        /// This is "input references" added from Unity Editor - It DOESN'T contain how accurate the drum was, what kind of drum was etc.
        /// </summary>
        [SerializeField]
        private RhythmInput[] _rhythmInputs;
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
        // Start is called before the first frame update
        private void Awake()
        {
            _miracleListener.OnMiracle.AddListener(() => Debug.Log("------------------- MIRACLE ---------------------"));

            OnCommandInput.AddListener(ComboManager.CountCombo);
            OnCommandCanceled.AddListener(ComboManager.EndCombo);
            OnCommandCanceled.AddListener(() =>
            {
                RhythmTimer.OnNextHalfTime.RemoveListener(TurnCounter.Start);
            });
            // --------------- Command sent check start
            OnCommandCanceled.AddListener(() => _gotAnyCommandInput = false);
            TurnCounter.OnTurn.AddListener(() =>
            {
                if (TurnCounter.IsPlayerTurn)
                {
                    RhythmTimer.OnNextHalfTime.AddListener(() =>
                        RhythmTimer.OnNext.AddListener(() =>
                        {
                            if (!_gotAnyCommandInput)
                            {
                                OnCommandCanceled.Invoke();
                            }
                        })
                    );
                }
                else
                {
                    _gotAnyCommandInput = false;
                }
            });

            // --------------- Command sent check end

            //Initalizes drum beats
            //Will loaded based on game progress
            _data = new CommandListData(
                new Dictionary<CommandSong, DrumType[]>()
                {
                    { CommandSong.Patapata, new DrumType[]{ DrumType.Pata, DrumType.Pata, DrumType.Pata, DrumType.Pon }}, //PATA PATA PATA PON
                    { CommandSong.Ponpon, new DrumType[]{ DrumType.Pon, DrumType.Pon, DrumType.Pata, DrumType.Pon } }, //PON PON PATA PON
                    { CommandSong.Chakachaka, new DrumType[]{ DrumType.Chaka, DrumType.Chaka, DrumType.Pata, DrumType.Pon } }, //CHAKA CHAKA PATA PON
                    { CommandSong.Ponpata, new DrumType[]{ DrumType.Pon, DrumType.Pata, DrumType.Pon, DrumType.Pata } } ,//PON PATA PON PATA
                    { CommandSong.Ponchaka, new DrumType[]{ DrumType.Pon, DrumType.Pon, DrumType.Chaka, DrumType.Chaka } } ,//PON PON CHAKA CHAKA
                    { CommandSong.Dondon, new DrumType[]{ DrumType.Don, DrumType.Don, DrumType.Chaka, DrumType.Chaka } } ,//DON DON CHAKA CHAKA
                    { CommandSong.Donchaka, new DrumType[]{ DrumType.Pata, DrumType.Pon, DrumType.Don, DrumType.Chaka } } ,//PATA PON DON CHAKA
                    { CommandSong.Patachaka, new DrumType[]{ DrumType.Pata, DrumType.Chaka, DrumType.Pata, DrumType.Pon } } ,//PATA CHAKA PATA PON (NEW): *DOES NOTHING*
                });
        }

        private void AddDrumHit(RhythmInputModel inputModel)
        {
            if (inputModel.Status == DrumHitStatus.Miss)
            {
                ClearDrumHits();
                OnCommandCanceled.Invoke();
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
            OnCommandCanceled.AddListener(_miracleListener.Reset);

            foreach (var rhythmInput in _rhythmInputs)
            {
                rhythmInput.OnDrumHit.AddListener(AddDrumHit);
            }
        }
        private void OnDisable()
        {
            ComboManager.EndComboImmediately();
            OnCommandCanceled.RemoveListener(TurnCounter.Stop);
            OnCommandCanceled.RemoveListener(_miracleListener.Reset);

            foreach (var rhythmInput in _rhythmInputs)
            {
                rhythmInput.OnDrumHit.RemoveListener(AddDrumHit);
            }
        }
        private void CheckCommand(RhythmInputModel inputModel)
        {
            //I don't know maybe there are better way...
            var drums = _currentHits.Select(hit => hit.Drum);

            if (CommandExists(drums, out CommandSong song))
            {
                _gotAnyCommandInput = true;

                if (_currentHits.Count == 4)
                {
                    if (!TurnCounter.IsOn)
                    {
                        RhythmTimer.OnNextHalfTime.AddListener(TurnCounter.Start);
                    }
                    TurnCounter.OnNextTurn.AddListener(() =>
                    {
                        OnCommandInput.Invoke(new RhythmCommandModel(_currentHits, song));
                        ClearDrumHits();
                    });
                }
            }
            else if (RhythmFever.IsFever && _miracleListener.HasMiracleChance(inputModel))
            {
                _gotAnyCommandInput = true;
                if (_miracleListener.MiracleDrumCount == 5)
                {
                    _miracleListener.Reset();
                    _miracleListener.OnMiracle.Invoke();
                    TurnCounter.Stop();
                    RhythmTimer.OnNext.AddListener(() =>
                    {
                        ClearDrumHits();
                        //Stop command listening, should pass to miracle status
                        enabled = false;
                    });
                }
            }
            else
            {
                OnCommandCanceled.Invoke();
            }
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
            OnCommandInput.RemoveAllListeners();
            OnCommandCanceled.RemoveAllListeners();
            _miracleListener.OnMiracle.RemoveAllListeners();
            ComboManager.Destroy();
        }
    }
}
