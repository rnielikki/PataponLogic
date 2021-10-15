﻿namespace Core.Character.Patapon
{
    public class Kibapon : Patapon
    {
        protected override Stat DefaultStat
        {
            get => new Stat
            {
                HitPoint = 220,
                Defence = 1.2f,
                DamageMin = 3,
                DamageMax = 8,
                AttackSeconds = 2,
                Critical = 0.1f,
                CriticalResistance = 0.1f,
                Knockback = 0.1f,
                KnockbackResistance = 0.3f,
                Stagger = 0.3f,
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
            Class = ClassType.Kibapon;
        }
        protected override void Attack(bool isFever)
        {
            if (!isFever && !_charged)
            {
                base.Attack(false);
            }
            else
            {
                AttackInTime("attack-fever", Equipment.Weapon.AttackType.FeverAttack);
            }
        }
        protected override void Defend(bool isFever)
        {
            AttackInTime("defend", Equipment.Weapon.AttackType.Defend);
        }
    }
}