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
    public class PataponsManager : MonoBehaviour, IDistanceCalculatable
    {
        private System.Collections.Generic.List<Patapon> _patapons;
        private System.Collections.Generic.List<PataponGroup> _groups;
        public System.Collections.Generic.IEnumerable<PataponGroup> Groups => _groups;
        //--- this should be general but temp value for position and patapata test
        private bool _isAlreadyIdle;
        public int PataponCount => _patapons.Count;
        public int PataponGroupCount => _groups.Count;
        public Patapon FirstPatapon => _patapons.Count > 0 ? _patapons[0] : null;

        /// <summary>
        /// If this is set to true, Patapons go forward, also whole Patapon position does. For PATAPATA song.
        /// </summary>
        internal static bool IsMovingForward { get; set; }
        internal static bool AllowedToGoForward { get; set; } = true;

        public float DefaultWorldPosition => transform.position.x;

        public Vector2 MovingDirection => Vector2.right;

        public float AttackDistance => 0;

        public float Sight => CharacterEnvironment.Sight;

        private float _missionEndPosition;
        private bool _useMissionTower;

        private bool _hasEnemyOnSight = true;

        //------------------------------------- sounds of Patapon
        [SerializeField]
        private AudioClip[] _pataponSpeakingOnMiss;
        int _onMissSpeakingIndex;

        private CameraController.CameraMover _cameraMover;
        private CameraController.CameraZoom _cameraZoom;
        private DistanceCalculator _distanceCalculator;
        private const float _minimumPosition = -10;

        private void Awake()
        {
            PataponGroupGenerator.Generate(Global.GlobalData.CurrentSlot.PataponInfo.CurrentClasses, this);

            _patapons = new System.Collections.Generic.List<Patapon>(GetComponentsInChildren<Patapon>());
            _groups = GetComponentsInChildren<PataponGroup>().ToList();

            _cameraMover = Camera.main.GetComponent<CameraController.CameraMover>();
            _cameraMover.SetTarget(transform, false);
            _cameraZoom = Camera.main.GetComponent<CameraController.CameraZoom>();
            _distanceCalculator = DistanceCalculator.GetPataponManagerDistanceCalculator(this);

            TurnCounter.OnTurn.AddListener(() => IsMovingForward = false);
            TurnCounter.OnTurn.AddListener(() =>
            {
                if (!TurnCounter.IsPlayerTurn) General.PataponGeneral.ShoutedOnThisTurn = false;
            });
            RhythmTimer.Current.OnTime.AddListener(CheckIfZoom);
        }
        private void Start()
        {
            _useMissionTower = Map.MissionPoint.Current.UseMissionTower;
            if (_useMissionTower) _missionEndPosition = Map.MissionPoint.Current.MissionPointPosition.x;
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
                    if (AllowedToGoForward) IsMovingForward = true;
                    break;
                case CommandSong.Ponpata:
                    break;
            }
        }
        public void RemovePon(Patapon patapon)
        {
            _patapons.Remove(patapon);
            if (!patapon.Eaten) SpeakManager.Current.Play(CharacterSoundLoader.Current.PataponSounds.OnDead);
            if (!_patapons.Any(p => p.IsGeneral))
            {
                Map.MissionPoint.Current.WaitAndFailMission(4);
                FindObjectOfType<Rhythm.Bgm.RhythmBgmSinging>().StopSinging();
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
                    pon.DoMissionCompleteGesture();
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
                _groups.Remove(group);
                Destroy(group.gameObject);
            }
        }
        private bool HasEnemyOnSight() //for camera move
        {
            if (_patapons.Count == 0) return true;
            else return _distanceCalculator.GetClosest() != null;
        }
        public bool CanGoForward()
        {
            if (transform.position.x + PataponEnvironment.Steps >= Hazorons.HazoronPositionManager.GetClosestHazoronPosition()) return false;
            else if (_patapons.Count == 0) return true;
            var firstPon = _patapons[0];
            var closestV2 = firstPon.DistanceCalculator.GetClosest();
            if (closestV2 == null) return true;
            var closest = closestV2.Value.x + PataponEnvironment.Steps * Time.deltaTime;
            var nextPosition = transform.position.x + PataponEnvironment.Steps * Time.deltaTime;
            return closest > nextPosition;
        }

        private void Update()
        {
            if (IsMovingForward && CanGoForward())
            {
                transform.position += PataponEnvironment.Steps * Vector3.right * Time.deltaTime;
                if (_useMissionTower && transform.position.x >= _missionEndPosition)
                {
                    Map.MissionPoint.Current.EndMission();
                    Map.MissionPoint.Current.UseMissionTower = false;
                }
            }
        }
        private void LateUpdate()
        {
            var forward = FirstPatapon?.DistanceCalculator?.GetClosest()?.x;
            if (forward != null && forward.Value < transform.position.x)
            {
                var newPosition = forward.Value;
                if (newPosition < _minimumPosition) return;
                var pos = transform.position;
                var offset = transform.position.x - newPosition; //+
                foreach (var group in _groups)
                {
                    foreach (var pon in _patapons)
                    {
                        var ponPos = pon.transform.position;
                        ponPos.x += offset;
                        pon.transform.position = ponPos;
                    }
                }
                pos.x = newPosition;
                transform.position = pos;
            }
        }
        public void CheckIfZoom()
        {
            bool hasEnemyOnSight = HasEnemyOnSight() || (FirstPatapon.DistanceCalculator.GetClosest() != null);
            if (!_hasEnemyOnSight && hasEnemyOnSight) //has enemy on sight
            {
                _cameraZoom.ZoomOut();

                _hasEnemyOnSight = true;
            }
            else if (_hasEnemyOnSight && !hasEnemyOnSight) //no enemy on sight
            {
                var target = _groups[0].FirstPon.gameObject;
                _cameraZoom.ZoomIn(target.transform);
                _hasEnemyOnSight = false;
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
            AllowedToGoForward = true;
        }
    }
}
