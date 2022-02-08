namespace PataRoad.Core.Character
{
    /// <summary>
    /// Represents any basic charcter logic that MOVING OR DISTANCE MATTERS. It can be Patapon, enemy or boss. DOESN'T represent structures, though.
    /// </summary>
    public interface ICharacter : IAttacker, IDistanceCalculatable
    {
        public CharacterAnimator CharAnimator { get; }
        public DistanceCalculator DistanceCalculator { get; }
        public float CharacterSize { get; }
        public void StopAttacking(bool pause);
    }
}
