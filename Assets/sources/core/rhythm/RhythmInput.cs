using Core.Rhythm.Model;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Core.Rhythm
{
    internal class RhythmInput : MonoBehaviour
    {
        /// <summary>
        /// When the drum is hit. Note that it's "THIS DRUM", not any drum. If you want "Any drum" you must find all of them manually.
        /// </summary>
        public readonly UnityEvent<RhythmInputModel> OnDrumHit = new UnityEvent<RhythmInputModel>();
        [SerializeField]
        private DrumType _drumType;
        private InputAction _action;
        private RhythmInputAudio _audio;
        /// <summary>
        /// Prevents from 'fast repeat hit'. Always miss if input interval is too fast.
        /// </summary>
        internal static bool Disabled { get; private set; }
        void Awake()
        {
            Disabled = false;
            var actions = EventSystem.current.gameObject.GetComponent<PlayerInput>().actions;
            _action = actions.FindAction("Drum/" + _drumType.ToString());
            _audio = new RhythmInputAudio(_drumType, this);
            RhythmTimer.OnHalfTime.AddListener(() => Disabled = false);
        }
        void DrumHit(InputAction.CallbackContext context)
        {
            RhythmInputModel model;
            if (Disabled || (Command.TurnCounter.IsOn && !Command.TurnCounter.IsPlayerTurn))
            {
                model = RhythmInputModel.Miss(_drumType);
            }
            else
            {
                model =
                    new RhythmInputModel(
                        _drumType,
                        RhythmTimer.Count
                        );
            }
            Debug.Log(model.IfLater + " and " + model.Timing);
            OnDrumHit.Invoke(model);
            Disabled = true;
        }
        private void OnEnable()
        {
            RhythmTimer.OnHalfTime.AddListener(SetEnable);
            _action.started += DrumHit;
            _action.Enable();
        }
        private void OnDisable()
        {
            RhythmTimer.OnHalfTime.RemoveListener(SetEnable);
            _action.started -= DrumHit;
            _action.Disable();
        }
        private void OnDestroy()
        {
            OnDisable();
            OnDrumHit.RemoveAllListeners();
        }
        private void SetEnable() => Disabled = false;

    }
}
