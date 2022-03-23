using PataRoad.Core.Character.Bosses;
using UnityEngine;

namespace PataRoad.Core.Map.Levels.King
{
    public class KingBossAttack : BossAttackData
    {
        [SerializeField]
        private BossParticleCollision _meteor;
        [SerializeField]
        private KingRushComponent _rush;
        [SerializeField]
        private KingShield _shield;
        public void MeteorAttack() => _meteor.Attack();
        public void EnableShield() => _shield.gameObject.SetActive(true);
        protected override void Init()
        {
            CharacterSize = 1;
            base.Init();
        }
        private void Start()
        {
            _rush.Init(Boss.DistanceCalculator);
        }
        internal override void UpdateStatForBoss(int level)
        {
            //special case. no stat update required.
        }
    }
}