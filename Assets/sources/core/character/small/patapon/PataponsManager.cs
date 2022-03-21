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
    public class PataponsManager : MonoBehaviour, IDistanceCalculatable, Map.IHavingLevel
    {
        private System.Collections.Generic.List<Patapon> _patapons;
        private System.Collections.Generic.List<PataponGroup> _groups;
        public System.Collections.Generic.IEnumerable<PataponGroup> Groups => _groups;
        private readonly System.Collections.Generic.List<Class.ClassType> _classes = new System.Collections.Generic.List<Class.ClassType>();
        public System.Collections.Generic.IEnumerable<Class.ClassType> CurrentClasses => _classes;
        //--- this should be general but temp value for position and patapata test
        private bool _isAlreadyIdle;
        public int PataponCount => _patapons.Count;
        public int PataponGroupCount => _groups.Count;
        public Patapon FirstPatapon => _patapons.Count > 0 ? _patapons[0] : null;
        public System.Collections.Generic.IEnumerable<Patapon> Patapons => _patapons;

        /// <summary>
        /// If this is set to true, Patapons go forward, also whole Patapon position does. For PATAPATA song.
        /// </summary>
        internal static bool IsMovingForward { get; set; }
        internal static bool AllowedToGoForward { get; set; } = true;

        public float DefaultWorldPosition => transform.position.x;

        public Vector2 MovingDirection => Vector2.right;

        public float AttackDistance => 0;

        public float Sight => CharacterEnvironment.Sight;

        public bool UseCenterAsAttackTarget => true;

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
        public const float MinimumPosition = -10;

        private float _minSteps = PataponEnvironment.Steps;
        private float _maxSteps = PataponEnvironment.Steps;
        public float Steps { get; private set; } = PataponEnvironment.Steps;
        //--- Adjusted By Level
        public static float DodgeSpeedMinimumMultiplier { get; private set; } = 1;
        public static PataponsManager Current { get; private set; }

        private void Awake()
        {
            Current = this;
            PataponGroupGenerator.Generate(Global.GlobalData.CurrentSlot.PataponInfo.CurrentClasses, this);

            _patapons = new System.Collections.Generic.List<Patapon>(GetComponentsInChildren<Patapon>());
            _groups = GetComponentsInChildren<PataponGroup>().ToList();

            _cameraMover = Camera.main.GetComponent<CameraController.CameraMover>();
            _cameraMover.SetTarget(transform);
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
            if (Hazorons.HazoronPositionManager.Current != null)
            {
                Hazorons.HazoronPositionManager.Current.Init(transform);
            }
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
        public void RegisterGroup(PataponGroup group)
        {
            foreach (var pon in group.Patapons)
            {
                _patapons.Add(pon);
            }
            _groups.Add(group);
            _classes.Add(group.ClassType);
        }
        public void SendGeneralMode(RhythmCommandModel model)
        {
            if (model.ComboType != ComboStatus.Fever) return;
            foreach (var group in _groups.Where(g => g.General != null))
            {
                group.General.ActivateGeneralMode(model.Song);
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
            if (model.Song == CommandSong.Patapata && AllowedToGoForward)
            {
                IsMovingForward = true;
                Steps = Mathf.Lerp(_minSteps, _maxSteps, model.AccuracyRate);
            }
        }
        public void SetMinMaxStepRatio(float min, float max)
        {
            _minSteps = min * PataponEnvironment.Steps;
            _maxSteps = max * PataponEnvironment.Steps;
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
            foreach (var group in _groups.Where(g => g.General != null))
            {
                group.General.CancelGeneralMode();
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
                //Get the item!
                if (!Global.GlobalData.CurrentSlot.MapInfo.NextMap.MapData.UseMissionTower)
                {
                    Steps *= 1.25f;
                }
                foreach (var pon in _patapons)
                {
                    pon.DoMissionCompleteGesture();
                }
            }
        }
        public bool IsAllMelee()
        {
            foreach (var group in _groups)
            {
                var firstPon = group.FirstPon;
                if (firstPon == null) continue;
                if (!group.FirstPon.IsMeleeUnit) return false;
            }
            return true;
        }
        public int GetMeleeCount()
        {
            int count = 0;
            foreach (var group in _groups)
            {
                if (group.FirstPon.IsMeleeUnit) count++;
            }
            return count;
        }
        public bool ContainsClass(Class.ClassType type)
        {
            foreach (var group in _groups)
            {
                if (group.ClassType == type) return true;
            }
            return false;
        }
        public bool ContainsClassOnly(Class.ClassType type)
        {
            return _groups.Count == 1 && _groups[0].ClassType == type;
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
                _classes.Remove(group.ClassType);
                Destroy(group.gameObject);
            }
        }
        private bool HasEnemyOnSight() //for camera move
        {
            if (_patapons.Count == 0) return true;
            else
            {
                if (FirstPatapon != null && FirstPatapon.DistanceCalculator != null && FirstPatapon.DistanceCalculator.GetClosest() != null)
                {
                    return true;
                }
                return _distanceCalculator.GetClosest() != null;
            }
        }
        public bool CanGoForward()
        {
            if (transform.position.x + Steps >= Hazorons.HazoronPositionManager.GetClosestHazoronPosition()) return false;
            else if (_patapons.Count == 0) return true;
            var closestV2 = _distanceCalculator.GetClosest();
            if (closestV2 == null) return true;
            var closest = closestV2.Value.x + (Steps * Time.deltaTime);
            var nextPosition = transform.position.x + (Steps * Time.deltaTime);
            return closest > nextPosition;
        }

        private void Update()
        {
            if (IsMovingForward && CanGoForward())
            {
                transform.position += Steps * Time.deltaTime * Vector3.right;
                if (_useMissionTower && transform.position.x >= _missionEndPosition)
                {
                    Map.MissionPoint.Current.EndMission();
                    Map.MissionPoint.Current.UseMissionTower = false;
                }
            }
        }
        private void LateUpdate()
        {
            //pushback
            var closest = FirstPatapon == null ? null : FirstPatapon.DistanceCalculator?.GetClosest();
            var forward = closest?.x;
            if (forward < transform.position.x)
            {
                var newPosition = forward.Value;
                if (newPosition < MinimumPosition) return;
                var pos = transform.position;
                var offset = transform.position.x - newPosition; //+
                foreach (var pon in _patapons)
                {
                    if (pon == null || !pon.StatusEffectManager.CanContinue)
                    {
                        continue;
                    }
                    var ponPos = pon.transform.position;
                    ponPos.x += offset;
                    pon.transform.position = ponPos;
                }
                pos.x = newPosition;
                transform.position = pos;
            }
        }
        public void CheckIfZoom()
        {
            bool hasEnemyOnSight = HasEnemyOnSight();
            if (!_hasEnemyOnSight && hasEnemyOnSight) //has enemy on sight
            {
                _cameraZoom.ZoomOut();
                _cameraMover.SetCameraOffset(5);
                _hasEnemyOnSight = true;
            }
            else if (_hasEnemyOnSight && !hasEnemyOnSight) //no enemy on sight
            {
                var target = _groups[0].FirstPon.gameObject;
                _cameraZoom.ZoomIn(target.transform);
                _cameraMover.SetToDefaultCameraOffset();
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
            Current = null;
        }
        public void SetLevel(int level, int absoluteMaxLevel)
        {
            var offset = (float)(level - 1) / (absoluteMaxLevel - 1);
            DodgeSpeedMinimumMultiplier = Mathf.Lerp(0.8f, 1, 1 - offset);
        }
    }
}
