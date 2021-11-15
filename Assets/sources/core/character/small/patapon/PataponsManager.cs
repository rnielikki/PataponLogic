using PataRoad.Core.Rhythm;
using PataRoad.Core.Rhythm.Command;
using PataRoad.GameSound;
using System.Linq;
using UnityEngine;

namespace PataRoad.Core.Character.Patapons
{
    /// <summary>
    /// Gets command and drum status and sends message to Patapons.
    /// </summary>
    class PataponsManager : MonoBehaviour
    {
        private System.Collections.Generic.List<Patapon> _patapons;
        private System.Collections.Generic.List<PataponGroup> _groups;
        public System.Collections.Generic.IEnumerable<PataponGroup> Groups => _groups;
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

        //------------------------------------- sounds of Patapon
        [SerializeField]
        private AudioClip[] _pataponSpeakingOnMiss;
        int _onMissSpeakingIndex;

        private CameraController.CameraMover _cameraMover;

        private void Awake()
        {
            PataponGroupGenerator.Generate(_pataponTypes, this);

            _patapons = new System.Collections.Generic.List<Patapon>(GetComponentsInChildren<Patapon>());
            _groups = GetComponentsInChildren<PataponGroup>().ToList();

            //Camera.main.GetComponent<CameraController.CameraMover>().Target = _firstGroup.Patapons[0].gameObject;
            _cameraMover = Camera.main.GetComponent<CameraController.CameraMover>();
            _cameraMover.Target = gameObject;

            TurnCounter.OnTurn.AddListener(() => IsMovingForward = false);
            TurnCounter.OnTurn.AddListener(() =>
            {
                if (!TurnCounter.IsPlayerTurn) General.PataponGeneral.ShoutedOnThisTurn = false;
            });
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
                SpeakManager.Current.Play(_pataponSpeakingOnMiss[_onMissSpeakingIndex]);
                _onMissSpeakingIndex = (_onMissSpeakingIndex + 1) % _pataponSpeakingOnMiss.Length;
                return;
            }
            var drumName = model.Drum.ToString();
            foreach (var pon in _patapons)
            {
                pon.MoveOnDrum(drumName);
            }
        }
        public void SendGeneralMode(RhythmCommandModel model)
        {
            if (model.ComboType != ComboStatus.Fever) return;
            foreach (var group in _groups)
            {
                group.General?.ActivateGeneralMode(model.Song);
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
            foreach (var group in _groups)
            {
                group.General?.CancelGeneralMode();
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
        public void RemovePon(Patapon patapon)
        {
            _patapons.Remove(patapon);
            SpeakManager.Current.Play(CharacterSoundLoader.Current.PataponSounds.OnDead);
            if (!_patapons.Any(p => p.IsGeneral))
            {
                Map.MissionPoint.Current.WaitAndFailMission(4);
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
        public void RemoveGroup(PataponGroup group)
        {

            var index = _groups.IndexOf(group);
            if (index < 0) return;

            if (_groups.Count > 1)
            {
                if (index == 0)
                {
                    transform.position = _groups[1].transform.position;
                    _cameraMover.SmoothMoving = true;
                }
                for (int i = index + 1; i < _groups.Count; i++)
                {
                    _groups[i].MoveTo(i - 1, index > 0);
                }
            }

            _groups.Remove(group);
            Destroy(group.gameObject);
        }
        private void Update()
        {
            if (IsMovingForward && _groups[0].CanGoForward())
            {
                transform.Translate(PataponEnvironment.Steps * Time.deltaTime, 0, 0);
                if (_useMissionTower && transform.position.x >= _missionEndPosition)
                {
                    Map.MissionPoint.Current.EndMission();
                    _useMissionTower = false;
                }
            }
        }
        public void HealAll(int amount)
        {
            foreach (var group in _groups)
            {
                group.HealAllInGroup(amount);
            }
        }
        private void OnDestroy()
        {
            IsMovingForward = false;
        }
    }
}
