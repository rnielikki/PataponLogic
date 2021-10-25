namespace Core.Character.Patapon
{
    public class Robopon : Patapon
    {
        protected override Stat DefaultStat
        {
            get => new Stat
            {
                HitPoint = 180,
                Defence = 0.85f,
                DamageMin = 8,
                DamageMax = 12,
                AttackSeconds = 2,
                MovementSpeed = 8.25f,
                Critical = 0.1f,
                CriticalResistance = 0.1f,
                Knockback = 0.1f,
                KnockbackResistance = 0.1f,
                Stagger = 0.1f,
                StaggerResistance = 0.1f,
                FireRate = 0,
                FireResistance = 0.1f,
                IceRate = 0,
                IceResistance = 0.1f,
                SleepRate = 0,
                SleepResistance = 0.1f
            };
        }

        protected override float _attackDistance { get; set; }
        private void Awake()
        {
            Init();
            Class = ClassType.Robopon;
            _attackDistance = transform.Find("Patapon-body/Face").GetComponent<UnityEngine.CircleCollider2D>().radius + 0.2f;
        }
        protected override void Attack(bool isFever)
        {
            if (!_charged)
            {
                base.Attack(false);
            }
            else
            {
                AttackInTime("attack-charge", 5);
            }
        }
    }
}
