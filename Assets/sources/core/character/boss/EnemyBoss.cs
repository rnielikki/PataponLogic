using UnityEngine;

namespace PataRoad.Core.Character.Bosses
{
    public abstract class EnemyBoss : Boss
    {
        public override Vector2 MovingDirection { get; } = Vector2.left;
        public BossTurnManager BossTurnManager { get; } = new BossTurnManager();
        protected Patapons.PataponsManager _pataponsManager { get; set; }
        protected bool _noTarget = true;
        private bool _moving;
        [SerializeField]
        protected int _level;

        protected override void Init(BossAttackData data)
        {
            base.Init(data);
            DistanceCalculator = DistanceCalculator.GetBossDistanceCalculator(this);
            _pataponsManager = FindObjectOfType<Patapons.PataponsManager>();
        }
        public override void Die()
        {
            base.Die();
            BossTurnManager.End();
            StartCoroutine(WaitAndCelebrate());
            System.Collections.IEnumerator WaitAndCelebrate()
            {
                yield return new WaitForSeconds(4);
                Map.MissionPoint.Current.FilledMissionCondition = true;
                Map.MissionPoint.Current.EndMission();
            }
        }
        //When staggered or got knockback.
        public override void StopAttacking()
        {
            BossTurnManager.End();
            base.StopAttacking();
        }

        protected abstract void CalculateAttack();
        private void Update()
        {
            if (BossTurnManager.Attacking || IsDead) return;
            var closest = DistanceCalculator.GetClosest();
            if (closest != null)
            {
                var targetPos = new Vector2(Mathf.Max(_pataponsManager.transform.position.x, closest.Value.x), 0);
                var offset = Stat.MovementSpeed * Time.deltaTime;
                if (transform.position.x - targetPos.x > AttackDistance - offset)
                {
                    targetPos.x += AttackDistance;
                    if (!_moving)
                    {
                        CharAnimator.Animate("walk");
                        _moving = true;
                    }
                    transform.position = Vector2.MoveTowards(transform.position, targetPos, offset);
                }
                else
                {
                    _moving = false;
                    CalculateAttack();
                }
            }
        }
    }
}
