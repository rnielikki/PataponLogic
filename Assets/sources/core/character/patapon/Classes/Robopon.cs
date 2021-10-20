﻿namespace Core.Character.Patapon
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

        private void Awake()
        {
            Init();
            Class = ClassType.Robopon;
        }
        protected override void Attack(bool isFever)
        {
            if (!_charged)
            {
                base.Attack(false);
            }
            else
            {
                AttackInTime("attack-charge");
            }
        }
    }
}
