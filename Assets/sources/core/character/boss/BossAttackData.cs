using UnityEngine;

namespace PataRoad.Core.Character.Bosses
{
    public class BossAttackData : MonoBehaviour
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
        private Boss _boss;
        private void Awake()
        {
            Init();
        }
        protected void Init()
        {
            _boss = GetComponent<Boss>();
        }

        public void Attack(BossAttackComponent component, GameObject target, Vector2 position)
        {
            MinLastDamageOffset = component.DamageOffsetMin;
            MaxLastDamageOffset = component.DamageOffsetMax;
            Equipments.Logic.DamageCalculator.DealDamage(_boss, _stat + component.AdditionalStat, target, position);
        }
    }
}
