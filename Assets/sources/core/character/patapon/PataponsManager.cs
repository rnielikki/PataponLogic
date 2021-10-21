using Core.Rhythm;
using Core.Rhythm.Command;
using UnityEngine;

namespace Core.Character.Patapon
{
    /// <summary>
    /// Gets command and drum status and sends message to Patapons.
    /// </summary>
    class PataponsManager : MonoBehaviour
    {
        private Patapon[] _patapons;
        private PataponGroup[] _groups;
        //--- this should be general but temp value for position and patapata test
        private Patapon _headPon;
        private bool _isAlreadyIdle;

        [SerializeField]
        [Tooltip("Defines walking steps for one PATAPATA song.")]
        private float _walkingSteps;
        private float _steps;

        /// <summary>
        /// If this is set to true, Patapons go forward, also whole Patapon position does. For PATAPATA song.
        /// </summary>
        internal static bool IsMovingForward { get; set; }

        private void Awake()
        {
            _steps = _walkingSteps / RhythmEnvironment.TurnSeconds;

            _patapons = GetComponentsInChildren<Patapon>();
            _groups = GetComponentsInChildren<PataponGroup>();

            _headPon = _patapons[0];
            Camera.main.GetComponent<CameraController.CameraMover>().Target = _headPon;
            _headPon.IsOnFirst = true;
            TurnCounter.OnTurn.AddListener(() => IsMovingForward = false);
        }
        /// <summary>
        /// Attach to <see cref="RhythmInput.OnDrumHit"/>.
        /// </summary>
        /// <param name="model"></param>
        public void SendDrumInput(RhythmInputModel model)
        {
            if (model.Status == DrumHitStatus.Miss)
            {
                return;
            }
            var drumName = model.Drum.ToString();
            foreach (var pon in _patapons)
            {
                pon.MoveOnDrum(drumName);
            }
        }
        /// <summary>
        /// Attach to <see cref="RhythmCommand.OnCommandInput"/>.
        /// </summary>
        /// <param name="model"></param>
        public void SendAction(RhythmCommandModel model)
        {
            _isAlreadyIdle = false;
            foreach (var pon in _patapons)
            {
                pon.Act(model.Song, model.ComboType == ComboStatus.Fever);
            }
            Move(model.Song);
        }
        /// <summary>
        /// Attach to <see cref="RhythmCommand.OnCommandCanceled"/>.
        /// </summary>
        public void ResetAction()
        {
            if (_isAlreadyIdle) return;
            IsMovingForward = false;
            _isAlreadyIdle = true;
            foreach (var pon in _patapons)
            {
                pon.PlayIdle();
            }
        }
        public void Move(CommandSong song)
        {
            switch (song)
            {
                case CommandSong.Patapata:
                    IsMovingForward = true;
                    break;
                case CommandSong.Ponpata:
                    break;
            }
        }
        private void Update()
        {
            if (IsMovingForward) transform.Translate(_steps * Time.deltaTime, 0, 0);
        }
        private void OnDestroy()
        {
            IsMovingForward = false;
        }
    }
}
