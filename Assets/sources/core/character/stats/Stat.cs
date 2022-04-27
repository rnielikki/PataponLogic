using System.Linq;

namespace PataRoad.Core.Character
{
    /// <summary>
    /// Reperesents stats for every characters and equipments.
    /// </summary>
    /// <note>FOR BOTH SERIALIZATION (in Unity Editor) AND MANUAL MODIFICATIONN, don't make them private or property.</note>
    [System.Serializable]
    public class Stat
    {
        [UnityEngine.SerializeField]
        private bool _isCharacterStat;
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
        [UnityEngine.SerializeField]
        private float _defenceMin;
        /// <summary>
        /// Minimum of how much one can resist attack. It DIVIDES the original damage. Use <see cref="AttackTypeResistance"/> for individual damage taking defence.
        /// </summary>
        /// <remarks>This value is very sensitive so making 2 will simply reduces damage to half. Be careful when use this,</remarks>
        public float DefenceMin
        {
            get => _defenceMin;
            set => _defenceMin = value;
        }
        [UnityEngine.SerializeField]
        private float _defenceMax;
        /// <summary>
        /// Maximum of how much one can resist attack. It DIVIDES the original damage. Use <see cref="AttackTypeResistance"/> for individual damage taking defence.
        /// </summary>
        /// <remarks>This value is very sensitive so making 2 will simply reduces damage to half. Be careful when use this,</remarks>
        public float DefenceMax
        {
            get => _defenceMax;
            set => _defenceMax = value;
        }

        [UnityEngine.SerializeField]
        private int _damageMin;
        /// <summary>
        /// Minimum damage value.
        /// </summary>
        public int DamageMin
        {
            get => _damageMin;
            set => _damageMin = value;
        }
        [UnityEngine.SerializeField]
        private int _damageMax;
        /// <summary>
        /// Maximum damage value.
        /// </summary>
        public int DamageMax
        {
            get => _damageMax;
            set => _damageMax = value;
        }
        [UnityEngine.SerializeField]
        private float _attackSeconds;
        /// <summary>
        /// Second(s) for one attack. Default for ordinary pons are 2 seconds (Mahopon 3s).
        /// </summary>
        public float AttackSeconds
        {
            get => _attackSeconds;
            set => _attackSeconds = GetSafeValue(value, 0.01f);
        }
        [UnityEngine.SerializeField]
        private float _movementSpeed;
        /// <summary>
        /// Movement speed (for attack, dodge etc) of a Patapon. Default for a normal patapon is usually expected as 8.
        /// </summary>
        public float MovementSpeed
        {
            get => _movementSpeed;
            set => _movementSpeed = GetSafeValue(value, 0.1f);
        }
        [UnityEngine.SerializeField]
        private float _critical;
        /// <summary>
        /// Chance to inflict critical damage. e.g. If critical chance is 100% it's 1 and if it's 150% this value is 1.5.
        /// </summary>
        public float Critical
        {
            get => _critical;
            set => _critical = value;
        }
        [UnityEngine.SerializeField]
        private float _stagger;
        /// <summary>
        /// Chance to stagger others. e.g. If stagger chance is 100% it's 1 and if it's 150% this value is 1.5.
        /// </summary>
        public float Stagger
        {
            get => _stagger;
            set => _stagger = value;
        }
        [UnityEngine.SerializeField]
        private float _knockback;
        /// <summary>
        /// Chance to knockback others. e.g. If knockback chance is 100% it's 1 and if it's 150% this value is 1.5.
        /// </summary>
        public float Knockback
        {
            get => _knockback;
            set => _knockback = value;
        }
        [UnityEngine.SerializeField]
        private bool _infiniteCriticalResistance;
        private int _infiniteCriticalResistanceCount;
        [UnityEngine.SerializeField]
        private float _criticalResistance;
        /// <summary>
        /// How much can resist from <see cref="Critical"/> damage.
        /// </summary>
        public float CriticalResistance
        {
            get => _infiniteCriticalResistance ? UnityEngine.Mathf.Infinity : _criticalResistance;
            private set => _criticalResistance = value;
        }
        [UnityEngine.SerializeField]
        private bool _infiniteStaggerResistance;
        private int _infiniteStaggerResistanceCount;
        [UnityEngine.SerializeField]
        private float _staggerResistance;
        /// <summary>
        /// How much can resist from <see cref="Stagger"/> status.
        /// </summary>
        public float StaggerResistance
        {
            get => _infiniteStaggerResistance ? UnityEngine.Mathf.Infinity : _staggerResistance;
            private set => _staggerResistance = value;
        }
        [UnityEngine.SerializeField]
        private bool _infiniteKnockbackResistance;
        private int _infiniteKnockbackResistanceCount;
        [UnityEngine.SerializeField]
        private float _knockbackResistance;
        /// <summary>
        /// How much can resist from <see cref="Knockback"/> status.
        /// </summary>
        public float KnockbackResistance
        {
            get => _infiniteKnockbackResistance ? UnityEngine.Mathf.Infinity : _knockbackResistance;
            private set => _knockbackResistance = value;
        }
        [UnityEngine.SerializeField]
        private float _fireRate;
        /// <summary>
        /// How much one can cause fire status effect. 1 means 100%.
        /// </summary>
        public float FireRate
        {
            get => _fireRate;
            set => _fireRate = value;
        }
        [UnityEngine.SerializeField]
        private float _iceRate;
        /// <summary>
        /// How much one can cause ice status effect. 1 means 100%.
        /// </summary>
        public float IceRate
        {
            get => _iceRate;
            set => _iceRate = value;
        }
        [UnityEngine.SerializeField]
        private float _sleepRate;
        /// <summary>
        /// How much one can cause sleep status effect. 1 means 100%.
        /// </summary>
        public float SleepRate
        {
            get => _sleepRate;
            set => _sleepRate = value;
        }

        [UnityEngine.SerializeField]
        private bool _infiniteFireResistance;
        private int _infiniteFireResistanceCount;
        [UnityEngine.SerializeField]
        private float _fireResistance;
        /// <summary>
        /// How much can resist from fire status effect. Counterpart of enemy's <see cref="FireRate"/>.
        /// </summary>
        public float FireResistance
        {
            get => _infiniteFireResistance ? UnityEngine.Mathf.Infinity : _fireResistance;
            private set => _fireResistance = value;
        }
        [UnityEngine.SerializeField]
        private bool _infiniteIceResistance;
        private int _infiniteIceResistanceCount;
        [UnityEngine.SerializeField]
        private float _iceResistance;
        /// <summary>
        /// How much can resist from ice status effect. Counterpart of enemy's <see cref="IceRate"/>.
        /// </summary>
        public float IceResistance
        {
            get => _infiniteIceResistance ? UnityEngine.Mathf.Infinity : _iceResistance;
            private set => _iceResistance = value;
        }
        [UnityEngine.SerializeField]
        private bool _infiniteSleepResistance;
        private int _infiniteSleepResistanceCount;
        [UnityEngine.SerializeField]
        private float _sleepResistance;
        /// <summary>
        /// How much can resist from sleep status effect. Counterpart of enemy's <see cref="SleepRate"/>.
        /// </summary>
        public float SleepResistance
        {
            get => _infiniteSleepResistance ? UnityEngine.Mathf.Infinity : _sleepResistance;
            private set => _sleepResistance = value;
        }
        public Stat()
        {
            _infiniteCriticalResistanceCount = _infiniteCriticalResistance ? 1 : 0;
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
            var stat = new Stat
            {
                HitPoint = 200,
                DefenceMin = 0.9f,
                DefenceMax = 1.1f,
                DamageMin = 4,
                DamageMax = 8,
                AttackSeconds = 2,
                MovementSpeed = 8
            };
            stat._isCharacterStat = true;
            return stat;
        }
        public static Stat operator +(Stat stat1, Stat stat2)
        {
            var newStat = new Stat
            {
                HitPoint = stat1.HitPoint + stat2.HitPoint,
                DefenceMin = stat1.DefenceMin + stat2.DefenceMin,
                DefenceMax = stat1.DefenceMax + stat2.DefenceMax,
                DamageMin = stat1.DamageMin + stat2.DamageMin,
                DamageMax = stat1.DamageMax + stat2.DamageMax,
                AttackSeconds = stat1.AttackSeconds + stat2.AttackSeconds,
                MovementSpeed = stat1.MovementSpeed + stat2.MovementSpeed,
                Critical = stat1.Critical + stat2.Critical,
                Knockback = stat1.Knockback + stat2.Knockback,
                Stagger = stat1.Stagger + stat2.Stagger,
                FireRate = stat1.FireRate + stat2.FireRate,
                IceRate = stat1.IceRate + stat2.IceResistance,
                SleepRate = stat1.SleepRate + stat2.SleepRate,

                CriticalResistance = stat1.CriticalResistance,
                KnockbackResistance = stat1.KnockbackResistance,
                StaggerResistance = stat1.StaggerResistance,
                FireResistance = stat1.FireResistance,
                IceResistance = stat1.IceResistance,
                SleepResistance = stat1.SleepResistance
            };

            newStat.AddResistancesFromStat(stat2);
            return newStat;
        }
        public Stat Add(Stat other) => Add(other, 1);
        /// <summary>
        /// Adds stat. This CHANGES value.
        /// </summary>
        /// <param name="other">Other stat to add. Also this won't be changed.</param>
        /// <returns>Self, after operation.</returns>
        /// <note>+ operator won't change existing Stat class but will return new class.</note>
        public Stat Add(Stat other, float damageMultiplier)
        {
            HitPoint += other.HitPoint;
            DefenceMin += other.DefenceMin;
            DefenceMax += other.DefenceMax;
            DamageMin += (int)(other.DamageMin * damageMultiplier);
            DamageMax += (int)(other.DamageMax * damageMultiplier);
            AttackSeconds += other.AttackSeconds;
            MovementSpeed += other.MovementSpeed;
            Critical += other.Critical;
            Knockback += other.Knockback;
            Stagger += other.Stagger;
            FireRate += other.FireRate;
            IceRate += other.IceRate;
            SleepRate += other.SleepRate;

            AddResistancesFromStat(other);
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
            Knockback -= other.Knockback;
            Stagger -= other.Stagger;
            FireRate -= other.FireRate;
            IceRate -= other.IceRate;
            SleepRate -= other.SleepRate;

            AddResistancesFromStat(other, true);

            return this;
        }

        public static Stat GetMidValue(System.Collections.Generic.IEnumerable<Stat> stats)
        {
            return new Stat
            {
                HitPoint = (int)stats.Average(s => s.HitPoint),
                DefenceMin = stats.Average(s => s.DefenceMin),
                DefenceMax = stats.Average(s => s.DefenceMax),
                DamageMin = (int)stats.Average(s => s.DamageMin),
                DamageMax = (int)stats.Average(s => s.DamageMax),
                AttackSeconds = stats.Average(s => s.AttackSeconds),
                MovementSpeed = stats.Average(s => s.MovementSpeed),
                Critical = stats.Average(s => s.Critical),
                CriticalResistance = stats.Average(s => s.CriticalResistance),
                Knockback = stats.Average(s => s.Knockback),
                KnockbackResistance = stats.Average(s => s.KnockbackResistance),
                Stagger = stats.Average(s => s.Stagger),
                StaggerResistance = stats.Average(s => s.StaggerResistance),
                FireRate = stats.Average(s => s.FireRate),
                FireResistance = stats.Average(s => s.FireResistance),
                IceRate = stats.Average(s => s.IceRate),
                IceResistance = stats.Average(s => s.IceResistance),
                SleepRate = stats.Average(s => s.SleepRate),
                SleepResistance = stats.Average(s => s.SleepResistance)
            };
        }
        public Stat Copy() => (Stat)MemberwiseClone();
        /// <summary>
        /// Sets stat to specific stat value. Useful for resetting value without instantiating.
        /// </summary>
        /// <param name="stat">The reference stat to make values identical.</param>
        /// <returns>The same instance, having identical value to input stat.</returns>
        public Stat SetValuesTo(Stat stat)
        {
            HitPoint = stat.HitPoint;
            DefenceMin = stat.DefenceMin;
            DefenceMax = stat.DefenceMax;
            DamageMin = stat.DamageMin;
            DamageMax = stat.DamageMax;
            AttackSeconds = stat.AttackSeconds;
            MovementSpeed = stat.MovementSpeed;
            Critical = stat.Critical;
            Knockback = stat.Knockback;
            Stagger = stat.Stagger;
            FireRate = stat.FireRate;
            IceRate = stat.IceRate;
            SleepRate = stat.SleepRate;

            _criticalResistance = stat._criticalResistance;
            _infiniteCriticalResistance = stat._infiniteCriticalResistance;
            _infiniteCriticalResistanceCount = stat._infiniteCriticalResistanceCount;

            _staggerResistance = stat._staggerResistance;
            _infiniteStaggerResistance = stat._infiniteStaggerResistance;
            _infiniteStaggerResistanceCount = stat._infiniteStaggerResistanceCount;

            _knockbackResistance = stat._knockbackResistance;
            _infiniteKnockbackResistance = stat._infiniteKnockbackResistance;
            _infiniteKnockbackResistanceCount = stat._infiniteKnockbackResistanceCount;

            _fireResistance = stat._fireResistance;
            _infiniteFireResistance = stat._infiniteFireResistance;
            _infiniteFireResistanceCount = stat._infiniteFireResistanceCount;

            _iceResistance = stat._iceResistance;
            _infiniteIceResistance = stat._infiniteIceResistance;
            _infiniteIceResistanceCount = stat._infiniteIceResistanceCount;

            _sleepResistance = stat._sleepResistance;
            _infiniteSleepResistance = stat._infiniteSleepResistance;
            _infiniteSleepResistanceCount = stat._infiniteSleepResistanceCount;

            return this;
        }
        public Stat BoostResistance(float amount)
        {
            AddCriticalResistance(amount, false);
            AddStaggerResistance(amount, false);
            AddKnockbackResistance(amount, false);
            AddFireResistance(amount, false);
            AddIceResistance(amount, false);
            AddSleepResistance(amount, false);

            return this;
        }
        //I don't want to do this but existing data will be removed if I move to other file.
        private float GetSafeValue(float value, float min) => _isCharacterStat ? UnityEngine.Mathf.Max(min, value) : value;
        public void AddCriticalResistance(float value, bool negative = false) =>
            InfinitySafeResistanceAdd(value,
                ref _infiniteCriticalResistanceCount, ref _infiniteCriticalResistance, ref _criticalResistance, negative);
        public void AddStaggerResistance(float value, bool negative = false) =>
            InfinitySafeResistanceAdd(value,
                ref _infiniteStaggerResistanceCount, ref _infiniteStaggerResistance, ref _staggerResistance, negative);
        public void AddKnockbackResistance(float value, bool negative = false) =>
            InfinitySafeResistanceAdd(value,
                ref _infiniteKnockbackResistanceCount, ref _infiniteKnockbackResistance, ref _knockbackResistance, negative);
        public void AddFireResistance(float value, bool negative = false) =>
            InfinitySafeResistanceAdd(value,
                ref _infiniteFireResistanceCount, ref _infiniteFireResistance, ref _fireResistance, negative);
        public void AddIceResistance(float value, bool negative = false) =>
            InfinitySafeResistanceAdd(value,
                ref _infiniteIceResistanceCount, ref _infiniteIceResistance, ref _iceResistance, negative);
        public void AddSleepResistance(float value, bool negative = false) =>
            InfinitySafeResistanceAdd(value,
                ref _infiniteSleepResistanceCount, ref _infiniteSleepResistance, ref _sleepResistance, negative);

        public void SetStaggerResistance(float value)
        {
            InfinitySafeResistanceAdd(value, ref _infiniteStaggerResistanceCount, ref _infiniteStaggerResistance,
                ref _staggerResistance, false, true);
        }

        private void AddResistancesFromStat(Stat otherStat, bool negative = false)
        {
            AddCriticalResistance(otherStat.CriticalResistance, negative);
            AddStaggerResistance(otherStat.StaggerResistance, negative);
            AddKnockbackResistance(otherStat.KnockbackResistance, negative);
            AddFireResistance(otherStat.FireResistance, negative);
            AddIceResistance(otherStat.IceResistance, negative);
            AddSleepResistance(otherStat.SleepResistance, negative);
        }
        private void InfinitySafeResistanceAdd(float valueToAdd, ref int resistCount,
            ref bool infinityResist, ref float realValue, bool negative, bool directSet = false)
        {
            if (negative) valueToAdd = -valueToAdd;
            if (valueToAdd == UnityEngine.Mathf.Infinity)
            {
                infinityResist = true;
                resistCount++;
            }
            else if (valueToAdd == UnityEngine.Mathf.NegativeInfinity)
            {
                if (resistCount > 0) resistCount--;
                infinityResist = resistCount > 0;
            }
            else
            {
                if (!directSet) realValue += valueToAdd;
                else realValue = valueToAdd;
            }
        }
    }
}
