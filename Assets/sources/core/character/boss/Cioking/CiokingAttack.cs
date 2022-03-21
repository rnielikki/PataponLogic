using UnityEngine;

namespace PataRoad.Core.Character.Bosses
{
    public class CiokingAttack : BossAttackData
    {
        private BossParticleCollision _sleepBubble;
        private AbsorbComponent _thrower;
        private CiokingDeathBubble[] _deathBubbles;

        protected override void Init()
        {
            CharacterSize = 10;
            _thrower = GetComponentInChildren<AbsorbComponent>();
            _sleepBubble = GetComponentInChildren<BossParticleCollision>();
            _deathBubbles = GetComponentsInChildren<CiokingDeathBubble>(true);
            base.Init();
            foreach (var deathBubble in _deathBubbles)
            {
                deathBubble.Init(Boss.MovingDirection);
            }
        }
        public void SleepAttack()
        {
            _sleepBubble.Attack();
        }
        public void ThrowingAttack()
        {
            _thrower.Attack();
        }
        public void StartThrowing()
        {
            _thrower.StartAbsorbing();
        }
        public void StopThrowingAttack()
        {
            _thrower.StopAttacking();
        }
        public void DeathBubble()
        {
            int i = 500;
            foreach (var bubble in _deathBubbles)
            {
                bubble.gameObject.SetActive(true);
                bubble.Throw(i);
                i += 100;
            }
        }

        public void StartEating()
        {
            _thrower.StartAbsorbing();
        }
        public override void StopAllAttacking()
        {
            _thrower.StopAttacking();
            base.StopAllAttacking();
        }
        internal override void UpdateStatForBoss(int level)
        {
            var value = 0.8f + (level * 0.25f);
            _stat.MultipleDamage(value);
            _stat.DefenceMin += (level - 1) * 0.005f;
            _stat.DefenceMax += (level - 1) * 0.01f;
            Boss.SetMaximumHitPoint(Mathf.RoundToInt(_stat.HitPoint * value));
            _stat.CriticalResistance += level * 0.06f;
            _stat.StaggerResistance += level * 0.08f;
            _stat.FireResistance += level * 0.05f;
            _stat.IceResistance += level * 0.05f;
        }

    }
}