namespace Core.Character.Patapon
{
    class Mahopon : Patapon
    {
        protected override Stat DefaultStat
        {
            get => new Stat
            {
                HitPoint = 120,
                Defence = 0.8f,
                DamageMin = 2,
                DamageMax = 14,
                AttackSeconds = 3,
                Critical = 0.1f,
                CriticalResistance = 0.1f,
                Knockback = 0,
                KnockbackResistance = 0.1f,
                Stagger = 0,
                StaggerResistance = 0.1f,
                FireRate = 0,
                FireResistance = 0.1f,
                IceRate = 0,
                IceResistance = 0.1f,
                SleepRate = 0,
                SleepResistance = 0.1f
            };
        }
        private void Awake()
        {
            Init();
            Class = ClassType.Mahopon;
        }
        protected override void Attack(bool isFever)
        {
            if (!_charged)
            {
                base.Attack(false);
            }
            else
            {
                AttackInTime("charge-attack");
            }
        }
    }
}
