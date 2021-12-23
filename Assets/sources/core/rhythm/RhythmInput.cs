using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace PataRoad.Core.Rhythm
{
    /// <summary>
    /// Sends one drum input signal. <see cref="RhythmInputMiracle"/> inherits this.
    /// </summary>
    public class RhythmInput : MonoBehaviour
    {
        /// <summary>
        /// When the drum is hit. Note that it's "THIS DRUM", not any drum. If you want "Any drum" you must find all of them manually.
        /// </summary>
        public readonly UnityEvent<RhythmInputModel> OnDrumHit = new UnityEvent<RhythmInputModel>();
        [SerializeField]
        private DrumType _drumType;
        protected InputAction _action;
        public DrumType DrumType => _drumType;
        /// <summary>
        /// Prevents from 'fast repeat hit'. Always miss if input interval is too fast.
        /// </summary>
        public static bool Disabled { get; protected set; }
        [SerializeField]
        bool _alwaysHit;
        void Awake()
        {
            Init();
        }
        protected virtual void Init()
        {
            var actions = FindObjectOfType<PlayerInput>().actions;
            _action = actions.FindAction("Drum/" + _drumType.ToString());
            SetResetTimer();
            _action.Enable();
            RhythmTimer.Current.OnStart.AddListener(() => Disabled = false);
        }
        protected virtual void SetResetTimer()
        {
            RhythmTimer.Current.OnHalfTime.AddListener(SetEnable);
        }
        protected void DrumHit(InputAction.CallbackContext context)
        {
            RhythmInputModel model;
            if (!_alwaysHit && (Disabled || (Command.TurnCounter.IsOn && !Command.TurnCounter.IsPlayerTurn)))
            {
                model = RhythmInputModel.Miss(_drumType);
            }
            else
            {
                model = GetInputModel();
            }
            OnDrumHit.Invoke(model);
        }
        protected virtual RhythmInputModel GetInputModel()
        {
            Disabled = true;
            return new RhythmInputModel(
                            _drumType,
                            RhythmTimer.Count
                            );
        }
        private void OnEnable() => Enable();
        private void OnDisable() => Disable();
        private void OnDestroy() => Destroy();
        protected void Enable()
        {
            RhythmTimer.Current?.OnHalfTime?.AddListener(SetEnable);
            _action.started += DrumHit;
        }
        protected void Disable()
        {
            RhythmTimer.Current?.OnHalfTime?.RemoveListener(SetEnable);
            _action.started -= DrumHit;
        }
        protected void Destroy()
        {
            OnDisable();
            OnDrumHit.RemoveAllListeners();
        }
        protected void SetEnable() => Disabled = false;
    }
}
