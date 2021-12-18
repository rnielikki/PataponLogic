using UnityEngine;

namespace PataRoad.Core.Character
{
    /// <summary>
    /// Attack move data, used for <see cref="AttackMoveModel"/> and <see cref="AttackMoveController"/>. Implements different logic to Patapon and Hazoron.
    /// </summary>
    public interface IAttackMoveData
    {
        public float MaxRushAttackPosition { get; }
        public float GetAttackPosition();
        public float GetDefendingPosition();
        public bool IsAttackableRange();
        public float GetRushPosition();
        public bool WasHitLastTime { get; set; } //should remember last hit even the turn endps, so can keep attacking without moving too much.
        public Vector2 LastHit { get; set; } //When the weapon hit last time.
    }
}
