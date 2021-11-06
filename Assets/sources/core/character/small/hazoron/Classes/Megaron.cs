using PataRoad.Core.Character.Patapons;

namespace PataRoad.Core.Character.Hazorons
{
    class Megaron : Hazoron
    {
        protected override Stat DefaultStat
        {
            get => new Stat
            {
                HitPoint = 150,
                Defence = 1,
                DamageMin = 1,
                DamageMax = 2,
                AttackSeconds = 2,
                MovementSpeed = 8,
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
        private void Awake()
        {
            AttackDistance = 10;
            Init();
            AttackType = Equipment.Weapon.AttackType.Sound;
            Class = ClassType.Megapon;
        }
        void Start()
        {
            SetAttackMoveController()
                .AddModels(
                new System.Collections.Generic.Dictionary<string, AttackMoveModel>()
                {
                    { "attack", GetAttackMoveModel("attack") },
                }
                );
            StartAttack("attack");
        }
    }
}
