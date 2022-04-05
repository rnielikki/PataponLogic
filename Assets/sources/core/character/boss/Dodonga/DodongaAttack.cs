using UnityEngine;

namespace PataRoad.Core.Character.Bosses
{
    public class DodongaAttack : BossAttackData
    {
        [SerializeField]
        private BossAttackParticle Fire;
        [SerializeField]
        private BossAttackCollision Headbutt;
        [SerializeField]
        private DodongaEatingComponent EatingMouth;
        [SerializeField]
        private Equipments.Weapons.ElementalAttackType _growlType;
        [SerializeField]
        private int _growlMinDamage;
        [SerializeField]
        private int _growlMaxDamage;
        private Stat _growlStat;

        protected override void Init()
        {
            CharacterSize = 7;
            base.Init();
        }
        public void FireAttack()
        {
            Fire.Attack();
        }
        public void EatingAttack()
        {
            EatingMouth.Attack();
        }
        public void StopEatingAttack()
        {
            EatingMouth.StopAttacking();
        }
        public void GrowlAttack()
        {
            if (_growlStat == null)
            {
                _growlStat = new Stat();
                _growlStat.DamageMin = _growlMinDamage;
                _growlStat.DamageMax = _growlMaxDamage;
            }
            Boss.AttackType = Equipments.Weapons.AttackType.Magic;
            Boss.ElementalAttackType = _growlType;
            foreach (var target in Boss.DistanceCalculator.GetAllAbsoluteTargetsOnFront())
            {
                Equipments.Logic.DamageCalculator.DealDamage(Boss, _growlStat, target.gameObject, target.transform.position);
            }
        }
        public override void StopAllAttacking()
        {
            base.StopAllAttacking();
            Headbutt.StopAttacking();
            EatingMouth.StopAttacking();
        }
        internal override void UpdateStatForBoss(int level)
        {
            var value = 0.8f + (level * 0.2f);
            _stat.MultipleDamage(value);
            _stat.DefenceMin += (level - 1) * 0.005f;
            _stat.DefenceMax += (level - 1) * 0.01f;
            Boss.SetMaximumHitPoint(Mathf.RoundToInt(_stat.HitPoint * value));
            _stat.AddCriticalResistance(level * 0.05f);
            _stat.AddStaggerResistance(level * 0.05f);
            _stat.AddKnockbackResistance(level * 0.05f);
            _stat.AddFireResistance(level * 0.03f);
            _stat.AddIceResistance(level * 0.03f);
            _stat.AddSleepResistance(level * 0.03f);
        }
    }
}
