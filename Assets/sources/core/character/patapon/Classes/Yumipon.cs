﻿namespace Core.Character.Patapon
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
                MovementSpeed = 7.5f,
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
            Init();
            InitDistanceFromHead(20);
            AttackType = Equipment.Weapon.AttackType.Stab;
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
