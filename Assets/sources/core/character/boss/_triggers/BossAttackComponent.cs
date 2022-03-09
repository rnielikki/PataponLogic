using UnityEngine;

namespace PataRoad.Core.Character.Bosses
{
    public abstract class BossAttackComponent : MonoBehaviour
    {
        [SerializeField]
        protected bool _enabled;

        [SerializeField]
        private Stat _additionalStat;
        public Stat AdditionalStat => _additionalStat;

        [SerializeField]
        private float _damageOffsetMin;
        public float DamageOffsetMin => _damageOffsetMin;

        [SerializeField]
        private float _damageOffsetMax = 1;
        public float DamageOffsetMax => _damageOffsetMax;

        [SerializeField]
        protected Equipments.Weapons.AttackType _attackType;
        [SerializeField]
        protected Equipments.Weapons.ElementalAttackType _elementalAttackType;

        protected BossAttackData _boss;
        private void Awake()
        {
            Init();
        }
        protected void Init()
        {
            _boss = GetComponentInParent<BossAttackData>();
        }
        public void SetDisable() => _enabled = false;
        public virtual void StopAttacking() { }
    }
}
