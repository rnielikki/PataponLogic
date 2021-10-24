namespace Core.Character.Patapon
{
    public class Yaripon : Patapon
    {
        protected override Stat DefaultStat
        {
            get => new Stat
            {
                HitPoint = 200,
                Defence = 1,
                DamageMin = 4,
                DamageMax = 12,
                AttackSeconds = 2,
                Critical = 0.1f,
                CriticalResistance = 0.1f,
                Knockback = 0.5f,
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

        protected override float _attackDistance { get; set; } = 7.5f;
        protected override float _moveVelocity { get; set; } = 8;

        private void Awake()
        {
            Init();
            Class = ClassType.Yaripon;
        }
        protected override void Attack(bool isFever)
        {
            if (!isFever && !_charged)
            {
                base.Attack(false);
            }
            else
            {
                AttackInTime("attack-fever");
            }
        }
    }
}
