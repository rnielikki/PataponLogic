namespace PataRoad.Core.Character
{
    /// <summary>
    /// Represents any basic ATTACKING objects. This is for e.g. cannon structures. Use <see cref="ICharacter"/> instead if character is moving or calculates distance.
    /// </summary>
    public interface IAttacker : IAttackable
    {
        /// <summary>
        /// This is for calculating damage/defence *between min-max value*. Returns [0-1]. The bigger the value is, the attack/defence is more. - for Patapon, "how perfect the drums are" affects.
        /// </summary>
        public float GetAttackValueOffset();
        public void OnAttackHit(UnityEngine.Vector2 point, int damage);
        public void OnAttackMiss(UnityEngine.Vector2 point);
        public Equipments.Weapons.AttackType AttackType { get; }
        public Equipments.Weapons.ElementalAttackType ElementalAttackType { get; }
    }
}
