using UnityEngine;

namespace PataRoad.Core.Character.Bosses
{
    public class DodongaEnemy : EnemyBoss
    {
        private DodongaAttack _attack;
        DistanceCalculator _distanceCalculator;
        private void Awake()
        {
            Init(GetComponent<DodongaAttack>());
            _attack = BossAttackData as DodongaAttack;
            Rhythm.Command.TurnCounter.OnTurn.AddListener(Fire);
        }
        void Fire()
        {
            if (Rhythm.Command.TurnCounter.IsPlayerTurn)
            {
                _attack.AnimateFire();
            }
        }
        public override void Die()
        {
            Rhythm.Command.TurnCounter.OnTurn.RemoveListener(Fire);
            base.Die();
        }
    }
}
