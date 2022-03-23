using PataRoad.Core.Character.Bosses;
using UnityEngine;

namespace PataRoad.Core.Map.Levels.King
{
    public class KingBoss : EnemyBoss
    {
        [SerializeField]
        HazoronAttacker[] _attackers;
        void PlayBeforeDefend(int index)
        {
            _attackers[index].PlayBeforeDefend();
        }
        void PlayDefend(int index)
        {
            _attackers[index].PlayDefend();
        }

        //no moving back
        public override bool TakeDamage(int damage)
        {
            CurrentHitPoint -= damage;
            return true;
        }
    }
}