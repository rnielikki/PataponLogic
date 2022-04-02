using UnityEngine;

namespace PataRoad.Core.Character.Bosses
{
    public class ShooshookleAttack : BossAttackData
    {
        [SerializeField]
        private BossAttackParticle _spore;
        [SerializeField]
        private AbsorbComponent[] _absorbers;
        private ShooshookleArm[] _arms;
        private int _lostArms;
        protected override void Init()
        {
            CharacterSize = 5;
            _absorbers = GetComponentsInChildren<AbsorbComponent>();
            _arms = GetComponentsInChildren<ShooshookleArm>();
            base.Init();
        }
        public void SporeAttack()
        {
            _spore.Attack();
        }
        public void StartEating()
        {
            foreach (var absorber in _absorbers)
            {
                absorber.StartAbsorbing();
            }
        }
        internal void AbsorberAttack(int index) => _absorbers[index].Attack();
        internal void StopAbsorberAttack(int index) => _absorbers[index].DisableAttacker();
        public void StopEating()
        {
            foreach (var absorber in _absorbers)
            {
                absorber.StopAttacking();
            }
        }
        public override void StopAllAttacking()
        {
            StopEating();
            base.StopAllAttacking();
        }
        internal override void OnIdle()
        {
            base.OnIdle();
            if (_lostArms > 0
                && Boss.StatusEffectManager.CurrentStatusEffect != StatusEffectType.Ice
                && Common.Utils.RandomByProbability(Mathf.Sqrt(Boss.GetLevel()) * _lostArms / 5))
            {
                foreach (var arm in _arms)
                {
                    arm.RestoreArm();
                }
                _lostArms = 0;
            }
        }
        internal void LoseArm() => _lostArms++;
        internal override void UpdateStatForBoss(int level)
        {
            var value = 0.8f + (level * 0.2f);
            _stat.MultipleDamage(value);
            _stat.DefenceMin += (level - 1) * 0.005f;
            _stat.DefenceMax += (level - 1) * 0.01f;
            Boss.SetMaximumHitPoint(Mathf.RoundToInt(_stat.HitPoint * value));
            _stat.AddCriticalResistance(level * 0.05f);
            _stat.AddStaggerResistance(level * 0.05f);
            _stat.AddFireResistance(level * 0.05f);
            _stat.AddIceResistance(level * 0.05f);

        }
    }
}