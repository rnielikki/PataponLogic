using UnityEngine;

namespace PataRoad.Core.Character.Bosses
{
    class ZaknelAttack : BossAttackData
    {
        [SerializeField]
        private BossAttackParticle Fire;

        protected override void Init()
        {
            CharacterSize = 5;
            base.Init();
        }
        public void FireAttack()
        {
            Fire.Attack();
        }
        public void EarthquakeAttack()
        {
            Boss.AttackType = Equipments.Weapons.AttackType.Crush;
            Boss.ElementalAttackType = Equipments.Weapons.ElementalAttackType.Neutral;
            Boss.StatusEffectManager.TumbleAttack(true);
        }
        public void GrowlAttack()
        {
            StopIgnoringStatusEffect();
            Boss.AttackType = Equipments.Weapons.AttackType.Magic;
            Boss.ElementalAttackType = Equipments.Weapons.ElementalAttackType.Neutral;
            foreach (var target in Boss.DistanceCalculator.GetAllAbsoluteTargetsOnFront())
            {
                Equipments.Logic.DamageCalculator.DealDamage(Boss, _stat, target.gameObject, target.transform.position);
            }
        }
        internal override void UpdateStatForBoss(int level)
        {
            var value = 0.8f + (level * 0.2f);
            _stat.MultipleDamage(value);
            _stat.DefenceMin += (level - 1) * 0.005f;
            _stat.DefenceMax += (level - 1) * 0.01f;
            Boss.SetMaximumHitPoint(Mathf.RoundToInt(_stat.HitPoint * value));
            _stat.CriticalResistance += level * 0.05f;
            _stat.StaggerResistance += level * 0.05f;
            _stat.KnockbackResistance += level * 0.05f;
            _stat.FireResistance += level * 0.03f;
            _stat.IceResistance += level * 0.03f;
            _stat.SleepResistance += level * 0.03f;
        }
    }
}
