﻿using Core.Rhythm;
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
        private PataponGroup _firstGroup;
        //--- this should be general but temp value for position and patapata test
        private bool _isAlreadyIdle;

        /// <summary>
        /// If this is set to true, Patapons go forward, also whole Patapon position does. For PATAPATA song.
        /// </summary>
        internal static bool IsMovingForward { get; set; }
        private float _missionEndPosition;

        [SerializeField]
        [Tooltip("Mission complete condition is going through Mission tower point.")]
        private bool _useMissionTower = true;

        //------------ Serialize field for auto generation, may be changed later.
        [SerializeField]
        private ClassType[] _pataponTypes;

        private void Awake()
        {
            PataponGroupGenerator.Generate(_pataponTypes, transform);

            _patapons = GetComponentsInChildren<Patapon>();
            var groups = GetComponentsInChildren<PataponGroup>();
            _firstGroup = groups[0];

            Camera.main.GetComponent<CameraController.CameraMover>().Target = _firstGroup.Patapons[0].gameObject;

            TurnCounter.OnTurn.AddListener(() => IsMovingForward = false);
            _missionEndPosition = GameObject.FindGameObjectWithTag("Finish").transform.position.x;
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
                pon.Act(model);
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
        public void DoMissionEndAction(bool complete)
        {
            if (!complete)
            {
                IsMovingForward = false;
            }
            else
            {
                IsMovingForward = true;
                foreach (var pon in _patapons)
                {
                    pon.DoMisisonCompleteGesture();
                }
            }
        }
        private void Update()
        {
            if (IsMovingForward && _firstGroup.CanGoForward())
            {
                transform.Translate(PataponEnvironment.Steps * Time.deltaTime, 0, 0);
                if (_useMissionTower && transform.position.x >= _missionEndPosition)
                {
                    Map.MissionPoint.Current.EndMission();
                    _useMissionTower = false;
                }
            }
        }
        private void OnDestroy()
        {
            IsMovingForward = false;
        }
    }
}
