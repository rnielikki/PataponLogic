namespace PataRoad.Core.Character
{
    public interface IDistanceCalculatable
    {
        public float DefaultWorldPosition { get; }
        public UnityEngine.Vector2 MovingDirection { get; }
        public float AttackDistance { get; }
        public float Sight { get; }
    }
}
