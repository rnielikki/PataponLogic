using Core.Character.Equipment;
using UnityEngine;

namespace Core.Character.Patapon
{
    public class Yaripon : Patapon
    {
        protected override Stat DefaultStat
        {
            get => new Stat
            {
                HitPoint = 200,
                DamageMin = 1,
                DamageMax = 4,
                AttackSeconds = 2,
                Critical = 0.1f,
                CriticalResistance = 0.1f,
                Knockback = 0.3f,
                KnockbackResistance = 0.1f,
                Stagger = 0.1f,
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
            Class = ClassType.Yaripon;

            ///TEST FOR 2x SPEED!
            Stat.AttackSeconds *= 0.5f;
        }
        protected override void Attack(bool isFever)
        {
            if (!isFever && !_charged)
            {
                base.Attack(false);
            }
            else
            {
                _animator.AnimateInTime("attack-fever", Stat.AttackSeconds);
                Weapon.Attack();
            }
        }
        protected override void Defend(bool isFever)
        {
            base.Defend(isFever);
            if (isFever || _charged)
            {
                Weapon.Attack();
            }
        }

    }
}
