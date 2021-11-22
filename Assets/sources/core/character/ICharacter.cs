
namespace PataRoad.Core.Character
{
    /// <summary>
    /// Represents any basic charcter logic. It can be Patapon, enemy or boss. DOESN'T represent structures, though.
    /// </summary>
    public interface ICharacter : IAttackable
    {
        /// <summary>
        /// This is for calculating damage/defence *between min-max value*. Returns [0-1]. The bigger the value is, the attack/defence is more. - for Patapon, "how perfect the drums are" affects.
        /// </summary>
        public float GetAttackValueOffset();
        public UnityEngine.Vector2 MovingDirection { get; }
        public void OnAttackHit(UnityEngine.Vector2 point, int damage);
        public CharacterAnimator CharAnimator { get; }
        public void OnAttackMiss(UnityEngine.Vector2 point);
        public DistanceCalculator DistanceCalculator { get; }
        public float AttackDistance { get; }
        public void StopAttacking();
    }
}
