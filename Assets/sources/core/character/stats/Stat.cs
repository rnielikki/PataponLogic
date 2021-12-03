﻿using System.Linq;

namespace PataRoad.Core.Character
{
    /// <summary>
    /// Reperesents stats for every characters and equipments.
    /// </summary>
    /// <note>FOR BOTH SERIALIZATION (in Unity Editor) AND MANUAL MODIFICATIONN, don't make them private or property.</note>
    [System.Serializable]
    public class Stat
    {
        /// <summary>
        /// "HP", A.k.a stamina.
        /// </summary>
        [UnityEngine.SerializeField]
        private int _hitPoint;
        public int HitPoint
        {
            get => _hitPoint;

            set => _hitPoint = value;
        }
        /// <summary>
        /// How much can resist attack, in minimum value.
        /// </summary>
        [UnityEngine.SerializeField]
        private float _defenceMin;
        public float DefenceMin
        {
            get => _defenceMin;
            set => _defenceMin = value;
        }
        /// <summary>
        /// How much can resist attack, in maximum value.
        /// </summary>
        [UnityEngine.SerializeField]
        private float _defenceMax;
        public float DefenceMax
        {
            get => _defenceMax;
            set => _defenceMax = value;
        }
        /// <summary>
        /// Minimum damage value.
        /// </summary>
        [UnityEngine.SerializeField]
        private int _damageMin;
        public int DamageMin
        {
            get => _damageMin;
            set => _damageMin = value;
        }
        /// <summary>
        /// Maximum damage value.
        /// </summary>
        [UnityEngine.SerializeField]
        private int _damageMax;
        public int DamageMax
        {
            get => _damageMax;
            set => _damageMax = value;
        }
        /// <summary>
        /// Second(s) for one attack. Default for ordinary pons are 2 seconds (Mahopon 3s).
        /// </summary>
        [UnityEngine.SerializeField]
        private float _attackSeconds;
        public float AttackSeconds
        {
            get => _attackSeconds;
            set => _attackSeconds = value;
        }
        /// <summary>
        /// Movement speed (for attack, dodge etc) of a Patapon. Default for a normal patapon is usually expected as 8.
        /// </summary>
        [UnityEngine.SerializeField]
        private float _movementSpeed;
        public float MovementSpeed
        {
            get => _movementSpeed;
            set => _movementSpeed = value;
        }
        /// <summary>
        /// Chance to inflict critical damage. e.g. If critical chance is 100% it's 1 and if it's 150% this value is 1.5.
        /// </summary>
        [UnityEngine.SerializeField]
        private float _critical;
        public float Critical
        {
            get => _critical;
            set => _critical = value;
        }
        /// <summary>
        /// Chance to stagger others. e.g. If stagger chance is 100% it's 1 and if it's 150% this value is 1.5.
        /// </summary>
        [UnityEngine.SerializeField]
        private float _stagger;
        public float Stagger
        {
            get => _stagger;
            set => _stagger = value;
        }
        /// <summary>
        /// Chance to knockback others. e.g. If knockback chance is 100% it's 1 and if it's 150% this value is 1.5.
        /// </summary>
        [UnityEngine.SerializeField]
        private float _knockback;
        public float Knockback
        {
            get => _knockback;
            set => _knockback = value;
        }
        /// <summary>
        /// How much can resist from <see cref="Critical"/> damage.
        /// </summary>
        [UnityEngine.SerializeField]
        private float _criticalResistance;
        public float CriticalResistance
        {
            get => _criticalResistance;
            set => _criticalResistance = value;
        }
        /// <summary>
        /// How much can resist from <see cref="Stagger"/> status.
        /// </summary>
        [UnityEngine.SerializeField]
        private float _staggerResistance;
        public float StaggerResistance
        {
            get => _staggerResistance;
            set => _staggerResistance = value;
        }
        /// <summary>
        /// How much can resist from <see cref="Knockback"/> status.
        /// </summary>
        [UnityEngine.SerializeField]
        private float _knockbackResistance;
        public float KnockbackResistance
        {
            get => _knockbackResistance;
            set => _knockbackResistance = value;
        }
        /// <summary>
        /// How much one can cause fire status effect. 1 means 100%.
        /// </summary>
        [UnityEngine.SerializeField]
        private float _fireRate;
        public float FireRate
        {
            get => _fireRate;
            set => _fireRate = value;
        }
        /// <summary>
        /// How much one can cause ice status effect. 1 means 100%.
        /// </summary>
        [UnityEngine.SerializeField]
        private float _iceRate;
        public float IceRate
        {
            get => _iceRate;
            set => _iceRate = value;
        }
        /// <summary>
        /// How much one can cause sleep status effect. 1 means 100%.
        /// </summary>
        [UnityEngine.SerializeField]
        private float _sleepRate;
        public float SleepRate
        {
            get => _sleepRate;
            set => _sleepRate = value;
        }

        /// <summary>
        /// How much can resist from fire status effect. Counterpart of enemy's <see cref="FireRate"/>.
        /// </summary>
        [UnityEngine.SerializeField]
        private float _fireResistance;
        public float FireResistance
        {
            get => _fireResistance;
            set => _fireResistance = value;
        }
        /// <summary>
        /// How much can resist from ice status effect. Counterpart of enemy's <see cref="IceRate"/>.
        /// </summary>
        [UnityEngine.SerializeField]
        private float _iceResistance;
        public float IceResistance
        {
            get => _iceResistance;
            set => _iceResistance = value;
        }
        /// <summary>
        /// How much can resist from sleep status effect. Counterpart of enemy's <see cref="SleepRate"/>.
        /// </summary>
        [UnityEngine.SerializeField]
        private float _sleepResistance;
        public float SleepResistance
        {
            get => _sleepResistance;
            set => _sleepResistance = value;
        }

        public void AddDamage(int amount)
        {
            DamageMin += amount;
            DamageMax += amount;
        }

        public void AddDamage(float amount)
        {
            DamageMin += (int)amount;
            DamageMax += (int)amount;
        }

        public void MultipleDamage(int amount)
        {
            DamageMin *= amount;
            DamageMax *= amount;
        }
        public void MultipleDamage(float amount)
        {
            DamageMin = (int)(DamageMin * amount);
            DamageMax = (int)(DamageMax * amount);
        }
        public static Stat GetAnyDefaultStatForCharacter()
        {
            return new Stat
            {
                HitPoint = 200,
                DefenceMin = 1,
                DefenceMax = 5,
                DamageMin = 4,
                DamageMax = 8,
                AttackSeconds = 2,
                MovementSpeed = 8
            };
        }
        public static Stat operator +(Stat stat1, Stat stat2)
        {
            return new Stat
            {
                HitPoint = stat1.HitPoint + stat2.HitPoint,
                DefenceMin = stat1.DefenceMin + stat2.DefenceMin,
                DefenceMax = stat1.DefenceMax + stat2.DefenceMax,
                DamageMin = stat1.DamageMin + stat2.DamageMin,
                DamageMax = stat1.DamageMax + stat2.DamageMax,
                AttackSeconds = stat1.AttackSeconds + stat2.AttackSeconds,
                MovementSpeed = stat1.MovementSpeed + stat2.MovementSpeed,
                Critical = stat1.Critical + stat2.Critical,
                CriticalResistance = stat1.CriticalResistance + stat2.CriticalResistance,
                Knockback = stat1.Knockback + stat2.Knockback,
                KnockbackResistance = stat1.KnockbackResistance + stat2.KnockbackResistance,
                Stagger = stat1.Stagger + stat2.Stagger,
                StaggerResistance = stat1.StaggerResistance + stat2.StaggerResistance,
                FireRate = stat1.FireRate + stat2.FireRate,
                FireResistance = stat1.FireResistance + stat2.FireResistance,
                IceRate = stat1.IceRate + stat2.IceResistance,
                IceResistance = stat1.IceResistance + stat2.IceResistance,
                SleepRate = stat1.SleepRate + stat2.SleepRate,
                SleepResistance = stat1.SleepResistance + stat2.SleepResistance
            };
        }
        /// <summary>
        /// Adds stat. This CHANGES value.
        /// </summary>
        /// <param name="other">Other stat to add. Also this won't be changed.</param>
        /// <returns>Self, after operation.</returns>
        /// <note>+ operator won't change existing Stat class but will return new class.</note>
        public Stat Add(Stat other)
        {
            HitPoint += other.HitPoint;
            DefenceMin += other.DefenceMin;
            DefenceMax += other.DefenceMax;
            DamageMin += other.DamageMin;
            DamageMax += other.DamageMax;
            AttackSeconds += other.AttackSeconds;
            MovementSpeed += other.MovementSpeed;
            Critical += other.Critical;
            CriticalResistance += other.CriticalResistance;
            Knockback += other.Knockback;
            KnockbackResistance += other.KnockbackResistance;
            Stagger += other.Stagger;
            StaggerResistance += other.StaggerResistance;
            FireRate += other.FireRate;
            FireResistance += other.FireResistance;
            IceRate += other.IceResistance;
            IceResistance += other.IceResistance;
            SleepRate += other.SleepRate;
            SleepResistance += other.SleepResistance;
            return this;
        }
        /// <summary>
        /// Subtracts stat. This CHANGES value.
        /// </summary>
        /// <param name="other">Other stat to add. Also this won't be changed.</param>
        /// <returns>Self, after operation.</returns>
        public Stat Subtract(Stat other)
        {
            HitPoint -= other.HitPoint;
            DefenceMin -= other.DefenceMin;
            DefenceMax -= other.DefenceMax;
            DamageMin -= other.DamageMin;
            DamageMax -= other.DamageMax;
            AttackSeconds -= other.AttackSeconds;
            MovementSpeed -= other.MovementSpeed;
            Critical -= other.Critical;
            CriticalResistance -= other.CriticalResistance;
            Knockback -= other.Knockback;
            KnockbackResistance -= other.KnockbackResistance;
            Stagger -= other.Stagger;
            StaggerResistance -= other.StaggerResistance;
            FireRate -= other.FireRate;
            FireResistance -= other.FireResistance;
            IceRate -= other.IceResistance;
            IceResistance -= other.IceResistance;
            SleepRate -= other.SleepRate;
            SleepResistance -= other.SleepResistance;
            return this;
        }
        private Stat MidValueTo(Stat stat)
        {
            Add(stat);
            HitPoint /= 2;
            DefenceMin /= 2;
            DefenceMax /= 2;
            DamageMin /= 2;
            DamageMax /= 2;
            AttackSeconds /= 2;
            MovementSpeed /= 2;
            Critical /= 2;
            CriticalResistance /= 2;
            Knockback /= 2;
            KnockbackResistance /= 2;
            Stagger /= 2;
            StaggerResistance /= 2;
            FireRate /= 2;
            FireResistance /= 2;
            IceRate /= 2;
            IceResistance /= 2;
            SleepRate /= 2;
            SleepResistance /= 2;
            return this;
        }

        public static Stat GetMidValue(System.Collections.Generic.IEnumerable<Stat> stats)
        {
            return stats.Aggregate((s1, s2) => s1.MidValueTo(s2));
        }
        public Stat Copy() => (Stat)MemberwiseClone();
    }
}
