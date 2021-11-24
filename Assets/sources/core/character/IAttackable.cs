namespace PataRoad.Core.Character
{
    /// <summary>
    /// Represents any attackable object, including struct and any character.
    /// </summary
    public interface IAttackable
    {
        /// <summary>
        /// Calculated *final* Stat data. If you use pipeline, hide real stat to private.
        /// </summary>
        /// <note>Even if the object is not attacking, this is necessary for defense.</note>
        public Stat Stat { get; }

        /// <summary>
        /// Current Hit point.
        /// <remarks>It shouldn't be bigger than <see cref="Stat.HitPoint"/> or smaller than 0. If this value is 0, it causes death.</remarks>
        /// </summary>
        public int CurrentHitPoint { get; }

        /// <summary>
        /// Invoked right after <see cref="TakeDamage(int)"/> through <see cref="Equipment.Logic.DamageCalculator"/>. If not wanted to use, set as <c><null/c>.
        /// </summary>
        /// <remarks>This is useful for health status update - it sends 0-1 value of "how much health currently is"</remarks>
        public UnityEngine.Events.UnityEvent<float> OnDamageTaken { get; }
        /// <summary>
        /// The DIRECT damage the object will get. Expected to be calculated through <see cref="Equipment.Logic.DamageCalculator"/>.
        /// </summary>
        public void TakeDamage(int damage);

        /// <summary>
        /// Calls when health is 0. This should contain destroying action; like destroying animation -> destroy
        /// </summary>
        public void Die();
        public float GetDefenceValueOffset();
        public StatusEffectManager StatusEffectManager { get; }
        public bool IsDead { get; }
    }
}
