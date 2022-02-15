using UnityEngine;

namespace PataRoad.Core.Character.Bosses
{
    public class EnemyBoss : Boss, Map.IHavingLevel
    {
        public override Vector2 MovingDirection { get; } = Vector2.left;
        public BossTurnManager BossTurnManager { get; private set; }
        protected Patapons.PataponsManager _pataponsManager { get; set; }
        protected bool _noTarget = true;
        private bool _sleeping = true;

        private bool _moving;
        private bool _movingBack;
        private bool _movingBackQueued;
        private static int _movingBackPosition = (int)CharacterEnvironment.Sight + 15;
        private int _phase;

        private Vector3 _targetPosition;
        protected bool _useWalkWhenMovingBack;
        private bool _movingBackAnimating;

        public int Level { get; private set; }
        private CameraController.SafeCameraZoom _zoom;
        private EnemyBossBehaviour _behaviour;

        void Awake()
        {
            Init();
            BossTurnManager = new BossTurnManager(BossAttackData);
            DefaultWorldPosition = transform.position.x;
            DistanceCalculator = DistanceCalculator.GetBossDistanceCalculator(this);
            _pataponsManager = FindObjectOfType<Patapons.PataponsManager>();

            _zoom = Camera.main.GetComponent<CameraController.SafeCameraZoom>();

            _behaviour = GetComponent<EnemyBossBehaviour>();
            _behaviour.Init(this, _pataponsManager);
            CharacterSize = _behaviour.CharacterSize;
        }
        internal void UseWalkingBackAnimation()
        {
            _useWalkWhenMovingBack = true;
        }
        public override void Die()
        {
            base.Die();
            BossTurnManager.Destroy();
            StartCoroutine(WaitAndCelebrate());
            System.Collections.IEnumerator WaitAndCelebrate()
            {
                yield return new WaitForSeconds(4);
                Map.MissionPoint.Current.FilledMissionCondition = true;
                Map.MissionPoint.Current.EndMission();
            }
        }
        public override void TakeDamage(int damage)
        {
            base.TakeDamage(damage);
            if (!_movingBackQueued)
            {
                float current = (float)CurrentHitPoint / Stat.HitPoint;
                if (changedPhase(current))
                {
                    _movingBackQueued = true;
                    _movingBackAnimating = false;
                    BossTurnManager.OnAttackEnd.AddListener(StartMovingBack);
                }
            }
            bool changedPhase(float hp)
            {
                int phase = _phase;
                if (hp > 0.66f)
                {
                    _phase = 0;
                }
                else if (hp > 0.33f)
                {
                    _phase = 1;
                }
                else
                {
                    _phase = 2;
                }
                return phase != _phase;
            }
        }
        private void StartMovingBack()
        {
            if (BossAttackData.UseCustomDataPosition) return;
            _targetPosition = Vector3.right * (DefaultWorldPosition + _movingBackPosition);
            DefaultWorldPosition = _targetPosition.x;
            _movingBack = true;
            _zoom.enabled = true;
        }
        //When staggered or got knockback.
        public override void StopAttacking(bool pause)
        {
            BossTurnManager.End();
            base.StopAttacking(pause);
        }
        public void SetLevel(int level, int absoluteMaxLevel)
        {
            Level = level;
            BossAttackData.UpdateStatForBoss(level);
        }
        protected float CalculateAttack() =>
            _behaviour.CalculateAttack();
        protected float CalculateAttackOnIce() =>
            _behaviour.CalculateAttack();
        private void Update()
        {
            //phase 0: attacking or dead
            if (BossTurnManager.Attacking || IsDead || !StatusEffectManager.CanContinue) return;
            //phase 0.12345: don't disturb, now it's doing after-attacking gesture
            if (BossAttackData.UseCustomDataPosition)
            {
                BossAttackData.SetCustomPosition();
                return;
            }
            //phase -1 : On Ice. No moving, just ATTACK
            if (StatusEffectManager.CurrentStatusEffect == StatusEffectType.Ice)
            {
                CharAnimator.Animate("Idle");
                BossTurnManager.End();
                CalculateAttackOnIce();
                BossTurnManager.StartAttack();
            }

            //phase 0: moving back.
            if (_movingBack)
            {
                var backOffset = Stat.MovementSpeed * 3 * Time.deltaTime;
                transform.position = Vector2.MoveTowards(transform.position, _targetPosition, backOffset);
                if (!_movingBackAnimating)
                {
                    CharAnimator.Animate(_useWalkWhenMovingBack ? "walk" : "nothing");
                    _movingBackAnimating = true;
                }
                if (transform.position.x >= _targetPosition.x - backOffset)
                {
                    _movingBack = false;
                    _sleeping = true;
                    CharAnimator.Animate("stop");
                    _movingBackQueued = false;
                    _movingBackAnimating = false;
                }
                return;
            }
            //phase 1: sleeping and found in sight
            if (_sleeping)
            {
                if (DistanceCalculator.GetTargetOnSight(Sight) != null)
                {
                    _sleeping = false;
                    AttackDistance = CalculateAttack();
                }
                else return;
            }
            //phase 2: go forward
            var closest = DistanceCalculator.GetClosest() ?? _pataponsManager.transform.position;
            var targetPos = new Vector2(Mathf.Max(_pataponsManager.transform.position.x + CharacterSize + AttackDistance, closest.x + CharacterSize + AttackDistance), 0);
            var offset = Stat.MovementSpeed * Time.deltaTime;

            if (Mathf.Abs(transform.position.x - targetPos.x) > offset)
            {
                if (!_moving)
                {
                    _moving = true;
                    CharAnimator.Animate("walk");
                }
                transform.position = Vector2.MoveTowards(transform.position, targetPos, offset);
            }
            else //phase 3: now enemy's on forward.
            {
                _moving = false;
                CharAnimator.Animate("Idle");
                if (BossTurnManager.IsEmpty) CalculateAttack();
                BossTurnManager.StartAttack();
                _zoom.enabled = false;
            }
        }
        private void OnDestroy()
        {
            BossTurnManager.Destroy();
        }
    }
}
