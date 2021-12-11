using UnityEngine;

namespace PataRoad.Core.Character.Bosses
{
    public abstract class BossAttackData : MonoBehaviour
    {
        [SerializeField]
        private string _name;
        public string Name => _name;
        [SerializeReference]
        private Stat _stat = new Stat();
        public Stat Stat => _stat;
        public float MinLastDamageOffset { get; protected set; } = 0;
        public float MaxLastDamageOffset { get; protected set; } = 0;
        public CharacterAnimator CharAnimator { get; set; }
        protected Boss _boss;
        private void Awake()
        {
            Init();
        }
        protected void Init()
        {
            _boss = GetComponent<Boss>();
        }

        /// <summary>
        /// Clear all collider or trigger of the body in here.
        /// </summary>
        public abstract void StopAllAttacking();
        public void Attack(BossAttackComponent component, GameObject target, Vector2 position)
        {
            MinLastDamageOffset = component.DamageOffsetMin;
            MaxLastDamageOffset = component.DamageOffsetMax;
            Equipments.Logic.DamageCalculator.DealDamage(_boss, _stat + component.AdditionalStat, target, position);
        }
    }
}
