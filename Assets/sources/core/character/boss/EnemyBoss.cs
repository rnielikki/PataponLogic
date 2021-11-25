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
        private const int _movingBackPosition = 50;
        private int _phase;

        private Vector3 _targetPosition;
        protected bool _useWalkWhenMovingBack;
        private bool _movingBackAnimating;

        protected int _level = 1; //should be loaded later!

        protected override void Init(BossAttackData data)
        {
            base.Init(data);
            BossTurnManager = new BossTurnManager(data);
            DefaultWorldPosition = transform.position.x;
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
            _targetPosition = Vector3.right * (DefaultWorldPosition + _movingBackPosition);
            DefaultWorldPosition = _targetPosition.x;
            _movingBack = true;
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
            if (BossTurnManager.Attacking || IsDead || !StatusEffectManager.CanContinue) return;

            //phase n: moving back.
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
                    CharAnimator.Animate("Sleep");
                    _movingBackQueued = false;
                    _movingBackAnimating = false;
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
