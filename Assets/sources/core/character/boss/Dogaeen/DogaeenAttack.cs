using UnityEngine;

namespace PataRoad.Core.Character.Bosses
{
    public class DogaeenAttack : BossAttackData
    {
        [SerializeField]
        private BossAttackParticle Fire;
        [SerializeField]
        private BossAttackCollision Headbutt;
        [SerializeField]
        private DodongaEatingComponent EatingMouth;

        public void FireAttack()
        {
            Fire.Attack();
        }
        public void HeadbuttAttack()
        {
            Headbutt.Attack();
        }
        public void StopHeadbuttAttack()
        {
            Headbutt.StopAttacking();
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
            _boss.AttackType = Equipments.Weapons.AttackType.Magic;
            _boss.ElementalAttackType = Equipments.Weapons.ElementalAttackType.Neutral;
            foreach (var target in _boss.DistanceCalculator.GetAllAbsoluteTargetsOnFront())
            {
                Equipments.Logic.DamageCalculator.DealDamage(_boss, _stat, target.gameObject, target.transform.position);
            }
        }
        public override void StopAllAttacking()
        {
            Headbutt.StopAttacking();
            EatingMouth.StopAttacking();
        }
        internal override void UpdateStatForBoss(int level)
        {
            var value = 0.8f + level * 0.2f;
            _stat.MultipleDamage(value);
            _stat.DefenceMin += (level - 1) * 0.005f;
            _stat.DefenceMax += (level - 1) * 0.01f;
            _boss.SetMaximumHitPoint(Mathf.RoundToInt(_stat.HitPoint * value));
            _stat.CriticalResistance += level * 0.05f;
            _stat.StaggerResistance += level * 0.05f;
            _stat.KnockbackResistance += level * 0.05f;
            _stat.FireResistance += level * 0.03f;
            _stat.IceResistance += level * 0.03f;
            _stat.SleepResistance += level * 0.03f;
        }
    }
}
