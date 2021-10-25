namespace Core.Character.Patapon
{
    public class Tatepon : Patapon
    {
        protected override Stat DefaultStat
        {
            get => new Stat
            {
                HitPoint = 250,
                Defence = 1.5f,
                DamageMin = 6,
                DamageMax = 8,
                AttackSeconds = 2,
                MovementSpeed = 8,
                Critical = 0.1f,
                CriticalResistance = 0.25f,
                Knockback = 0.1f,
                KnockbackResistance = 0.2f,
                Stagger = 0.05f,
                StaggerResistance = 0.2f,
                FireRate = 0.1f,
                FireResistance = 0.1f,
                IceRate = 0.1f,
                IceResistance = 0.1f,
                SleepRate = 0.1f,
                SleepResistance = 0.1f
            };
        }
        protected override float _attackDistance { get; set; }
        private void Awake()
        {
            Init();
            Class = ClassType.Tatepon;
            _attackDistance = transform.Find("Patapon-body/Face").GetComponent<UnityEngine.CircleCollider2D>().radius + 0.1f;
        }
        protected override void Attack(bool isFever)
        {
            if (_charged)
            {
                _animator.Animate("attack-charge");
                _pataponDistance.MoveRush(17);
            }
            else base.Attack(isFever);
        }

        protected override void Defend(bool isFever)
        {
            if (!isFever && !_charged)
            {
                _animator.Animate("defend");
            }
            else
            {
                _animator.Animate("defend-fever");
            }
            _pataponDistance.MoveZero(4);
        }
    }
}
