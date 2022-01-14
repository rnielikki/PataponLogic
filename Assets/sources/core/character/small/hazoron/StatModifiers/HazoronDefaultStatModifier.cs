namespace PataRoad.Core.Character.Hazorons.Levels
{
    class HazoronDefaultStatModifier : UnityEngine.MonoBehaviour, IHazoronStatModifier
    {
        protected int _level;
        public int Level => _level;
        protected Stat _stat;

        public void SetModifyTarget(Stat stat)
        {
            _stat = stat;
        }
        public virtual void SetLevel(int level)
        {
            _level = level;
            _stat.MultipleDamage(_level);
            _stat.HitPoint = (int)(_stat.HitPoint * UnityEngine.Mathf.Sqrt(_level));
        }
    }
}
