using UnityEngine;

namespace PataRoad.Core.Character.Bosses
{
    public abstract class BossAttackData : MonoBehaviour
    {
        [SerializeReference]
        protected Stat _stat = Stat.GetAnyDefaultStatForCharacter();

        [SerializeReference]
        protected AttackTypeResistance _attackTypeResistance = new AttackTypeResistance();
        public virtual AttackTypeResistance AttackTypeResistance => _attackTypeResistance;
        public virtual Stat Stat => _stat;
        public float MinLastDamageOffset { get; protected set; } = 0;
        public float MaxLastDamageOffset { get; protected set; } = 0;
        public CharacterAnimator CharAnimator { get; set; }

        public bool UseCustomDataPosition { get; internal set; }

        public Boss Boss { get; protected set; }
        public float CharacterSize { get; protected set; }

        private void Awake()
        {
            Init();
        }
        protected virtual void Init()
        {
            Boss = GetComponent<Boss>();
        }
        internal abstract void UpdateStatForBoss(int level);

        /// <summary>
        /// Clear all collider or trigger of the body in here.
        /// </summary>
        public virtual void StopAllAttacking()
        {
            StopIgnoringStatusEffect();
        }
        public void Attack(BossAttackComponent component, GameObject target, Vector2 position,
            Equipments.Weapons.AttackType attackType,
            Equipments.Weapons.ElementalAttackType elementalAttackType = Equipments.Weapons.ElementalAttackType.Neutral,
            bool allowZero = false)
        {
            MinLastDamageOffset = component.DamageOffsetMin;
            MaxLastDamageOffset = component.DamageOffsetMax;
            Boss.AttackType = attackType;
            Boss.ElementalAttackType = elementalAttackType;
            Equipments.Logic.DamageCalculator.DealDamage(Boss, _stat + component.AdditionalStat, target, position, allowZero);
        }
        public virtual void SetCustomPosition() { }
        public void IgnoreStatusEffect()
        {
            Boss.StatusEffectManager.IgnoreStatusEffect = true;
        }
        public void StopIgnoringStatusEffect()
        {
            Boss.StatusEffectManager.IgnoreStatusEffect = false;
        }
        internal virtual void OnIdle()
        {
            CharAnimator.Animate("Idle");
        }
    }
}
