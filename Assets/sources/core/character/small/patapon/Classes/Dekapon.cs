namespace PataRoad.Core.Character.Patapons
{
    public class Dekapon : Patapon
    {
        protected override Stat DefaultStat
        {
            get => new Stat
            {
                HitPoint = 300,
                Defence = 1,
                DamageMin = 4,
                DamageMax = 10,
                AttackSeconds = 2,
                MovementSpeed = 6,
                Critical = 0.4f,
                CriticalResistance = 0.25f,
                Knockback = 0.1f,
                KnockbackResistance = 0.1f,
                Stagger = 0.05f,
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
            Class = ClassType.Dekapon;
            AttackType = Equipment.Weapon.AttackType.Crush;
        }
        void Start()
        {
            AddDefaultModelsToAttackMoveController();
        }
        protected override void Attack()
        {
            if (!Charged)
            {
                base.Attack();
            }
            else
            {
                CharAnimator.Animate("attack-charge");
                DistanceManager.MoveZero(1.6f);
            }
        }
    }
}
