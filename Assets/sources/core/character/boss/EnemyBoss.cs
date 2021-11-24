using UnityEngine;

namespace PataRoad.Core.Character.Bosses
{
    public abstract class EnemyBoss : Boss
    {
        public override Vector2 MovingDirection { get; } = Vector2.left;
        public BossTurnManager BossTurnManager { get; private set; }
        protected Patapons.PataponsManager _pataponsManager { get; set; }
        protected bool _noTarget = true;
        private bool _sleeping = true;

        private bool _moving;
        private bool _movingBack;
        private bool _movingBackQueued;

        private Vector3 _targetPosition;
        [SerializeField]
        protected int _level;
        [SerializeField]
        protected bool _useWalkWhenMovingBack;

        protected override void Init(BossAttackData data)
        {
            base.Init(data);
            BossTurnManager = new BossTurnManager(data);
            DistanceCalculator = DistanceCalculator.GetBossDistanceCalculator(this);
            _pataponsManager = FindObjectOfType<Patapons.PataponsManager>();
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
            var before = (float)CurrentHitPoint / Stat.HitPoint;
            base.TakeDamage(damage);
            if (!_movingBackQueued)
            {
                float current = (float)CurrentHitPoint / Stat.HitPoint;
                if (before > 0.66f && current < 0.66f || before > 0.33f && current < 0.33f)
                {
                    _movingBackQueued = true;
                    BossTurnManager.OnAttackEnd.AddListener(StartMovingBack);
                }
            }
        }
        private void StartMovingBack()
        {
            _targetPosition = transform.position + Vector3.right * 50;
            _movingBack = true;
            CharAnimator.Animate(_useWalkWhenMovingBack ? "walk" : "nothing");
        }
        //When staggered or got knockback.
        public override void StopAttacking()
        {
            BossTurnManager.End();
            base.StopAttacking();
        }

        protected abstract float CalculateAttack();
        private void Update()
        {
            //phase 0: attacking or dead
            if (BossTurnManager.Attacking || IsDead) return;

            if (_movingBack)
            {
                var backOffset = Stat.MovementSpeed * 3 * Time.deltaTime;
                transform.position = Vector2.MoveTowards(transform.position, _targetPosition, backOffset);
                if (transform.position.x >= _targetPosition.x - backOffset)
                {
                    _movingBack = false;
                    _sleeping = true;
                    CharAnimator.Animate("sleep");
                    _movingBackQueued = false;
                }
                return;
            }

            //phase 1: sleeping and found in sight
            if (_sleeping && DistanceCalculator.HasAttackTarget())
            {
                _sleeping = false;
                AttackDistance = CalculateAttack();
            }
            //phase 2: go forward
            var closest = DistanceCalculator.GetClosest() ?? _pataponsManager.transform.position;
            var targetPos = new Vector2(Mathf.Max(_pataponsManager.transform.position.x, closest.x), 0);
            var offset = Stat.MovementSpeed * Time.deltaTime;

            if (transform.position.x - targetPos.x > AttackDistance + offset)
            {
                targetPos.x += AttackDistance;
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
            }
        }
        private void OnDestroy()
        {
            BossTurnManager.Destroy();
        }
    }
}
