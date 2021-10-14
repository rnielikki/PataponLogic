namespace Core.Character
{
    /// <summary>
    /// Reperesents stats for every characters and equipments.
    /// </summary>
    public class Stat
    {
        /// <summary>
        /// "HP", A.k.a stamina.
        /// </summary>
        public int HitPoint { get; set; }
        private float _defence;
        /// <summary>
        /// How much can resist attack. User gets 1/Defence of damage. Minimum is 0.01 (but nobody should have such bad defence). 1 gets exactly same as damage (without any bonus)
        /// </summary>
        public float Defence
        {
            get => _defence;
            set => _defence = (value < 0.01f) ? 0.01f : value;
        }
        /// <summary>
        /// Minimum damage value.
        /// </summary>
        public int DamageMin { get; set; }
        /// <summary>
        /// Maximum damage value.
        /// </summary>
        public int DamageMax { get; set; }
        /// <summary>
        /// Second(s) for one attack. Default for ordinary pons are 2 seconds (Mahopon 3s).
        /// </summary>
        public float AttackSeconds { get; set; }
        /// <summary>
        /// Chance to inflict critical damage. e.g. If critical chance is 100% it's 1 and if it's 150% this value is 1.5.
        /// </summary>
        public float Critical { get; set; }
        /// <summary>
        /// Chance to stagger others. e.g. If stagger chance is 100% it's 1 and if it's 150% this value is 1.5.
        /// </summary>
        public float Stagger { get; set; }
        /// <summary>
        /// Chance to knockback others. e.g. If knockback chance is 100% it's 1 and if it's 150% this value is 1.5.
        /// </summary>
        public float Knockback { get; set; }
        /// <summary>
        /// How much can resist from <see cref="Critical"/> damage.
        /// </summary>
        public float CriticalResistance { get; set; }
        /// <summary>
        /// How much can resist from <see cref="Stagger"/> status.
        /// </summary>
        public float StaggerResistance { get; set; }
        /// <summary>
        /// How much can resist from <see cref="Knockback"/> status.
        /// </summary>
        public float KnockbackResistance { get; set; }

        /// <summary>
        /// How much one can cause fire status effect. 1 means 100%.
        /// </summary>
        public float FireRate { get; set; }
        /// <summary>
        /// How much one can cause ice status effect. 1 means 100%.
        /// </summary>
        public float IceRate { get; set; }
        /// <summary>
        /// How much one can cause sleep status effect. 1 means 100%.
        /// </summary>
        public float SleepRate { get; set; }

        /// <summary>
        /// How much can resist from fire status effect. Counterpart of enemy's <see cref="FireRate"/>.
        /// </summary>
        public float FireResistance { get; set; }
        /// <summary>
        /// How much can resist from ice status effect. Counterpart of enemy's <see cref="IceRate"/>.
        /// </summary>
        public float IceResistance { get; set; }
        /// <summary>
        /// How much can resist from sleep status effect. Counterpart of enemy's <see cref="SleepRate"/>.
        /// </summary>
        public float SleepResistance { get; set; }

        public static Stat operator +(Stat stat1, Stat stat2)
        {
            return new Stat
            {
                HitPoint = stat1.HitPoint + stat2.HitPoint,
                Defence = stat1.Defence + stat2.Defence,
                DamageMin = stat1.DamageMin + stat2.DamageMin,
                DamageMax = stat1.DamageMax + stat2.DamageMax,
                AttackSeconds = stat1.AttackSeconds + stat2.AttackSeconds,
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
    }
}
