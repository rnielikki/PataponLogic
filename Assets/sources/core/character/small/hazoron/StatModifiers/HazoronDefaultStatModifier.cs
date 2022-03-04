﻿namespace PataRoad.Core.Character.Hazorons.Levels
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
        public void SetLevel(int level, int absoluteMaxLevel)
        {
            if (_stat == null)
            {
                _stat = GetComponent<IAttackable>()?.Stat;
                if (_stat == null)
                {
                    throw new System.InvalidOperationException("Can't find stat to change!");
                }
            }
            _level = level;
            var value = 0.8f + 0.2f * level;
            _stat.MultipleDamage(value);
            GetComponent<Hazoron>().SetMaximumHitPoint(UnityEngine.Mathf.RoundToInt(value * _stat.HitPoint));
        }
    }
}
