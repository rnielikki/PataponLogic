namespace Core.Character
{
    /// <summary>
    /// Attack move data, used for <see cref="AttackMoveModel"/> and <see cref="AttackMoveController"/>. Implements different logic to Patapon and Hazoron.
    /// </summary>
    public interface IAttackMoveData
    {
        public float MaxRushAttackPosition { get; }
        public float DefaultWorldPosition { get; }
        public float GetAttackPosition(float customDistance = -1);
        public float GetDefendingPosition(float customDistance = -1);
        public bool IsAttackableRange();
        public float GetRushPosition();
    }
}
