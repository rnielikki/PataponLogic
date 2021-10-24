namespace Core.Character.Patapon
{
    class Yumipon : Patapon
    {
        protected override Stat DefaultStat
        {
            get => new Stat
            {
                HitPoint = 150,
                Defence = 1,
                DamageMin = 1,
                DamageMax = 4,
                AttackSeconds = 2,
                Critical = 0.2f,
                CriticalResistance = 0.1f,
                Knockback = 0.1f,
                KnockbackResistance = 0.1f,
                Stagger = 0.1f,
                StaggerResistance = 0.1f,
                FireRate = 0.1f,
                FireResistance = 0.1f,
                IceRate = 0.1f,
                IceResistance = 0.1f,
                SleepRate = 0.1f,
                SleepResistance = 0.1f
            };
        }
        protected override float _attackDistance { get; set; } = 20;
        protected override float _moveVelocity { get; set; } = 8;

        private void Awake()
        {
            Init();
            Class = ClassType.Yumipon;
        }
        protected override void Attack(bool isFever)
        {
            if (!isFever && !_charged)
            {
                base.Attack(false);
            }
            else
            {
                AttackInTime("attack", speed: 3);
            }
        }
    }
}
