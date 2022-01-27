using UnityEngine;

namespace PataRoad.Core.Character.Bosses
{
    public abstract class BossAttackData : MonoBehaviour
    {
        [SerializeReference]
        protected Stat _stat = Stat.GetAnyDefaultStatForCharacter();
        public Stat Stat => _stat;
        public float MinLastDamageOffset { get; protected set; } = 0;
        public float MaxLastDamageOffset { get; protected set; } = 0;
        public CharacterAnimator CharAnimator { get; set; }

        public bool UseCustomDataPosition { get; protected set; }

        protected Boss _boss;
        private void Awake()
        {
            Init();
        }
        protected void Init()
        {
            _boss = GetComponent<Boss>();
        }
        internal abstract void UpdateStatForBoss(int level);

        /// <summary>
        /// Clear all collider or trigger of the body in here.
        /// </summary>
        public abstract void StopAllAttacking();
        public void Attack(BossAttackComponent component, GameObject target, Vector2 position, bool allowZero = false)
        {
            MinLastDamageOffset = component.DamageOffsetMin;
            MaxLastDamageOffset = component.DamageOffsetMax;
            Equipments.Logic.DamageCalculator.DealDamage(_boss, _stat + component.AdditionalStat, target, position, allowZero);
        }
        public virtual void SetCustomPosition() { }
    }
}
