using UnityEngine;

namespace PataRoad.Core.Character.Bosses
{
    public abstract class BossAttackComponent : MonoBehaviour
    {
        [SerializeField]
        private Stat _additionalStat;
        public Stat AdditionalStat => _additionalStat;

        [SerializeField]
        private float _damageOffsetMin;
        public float DamageOffsetMin => _damageOffsetMin;

        [SerializeField]
        private float _damageOffsetMax = 1;
        public float DamageOffsetMax => _damageOffsetMax;

        protected BossAttackData _boss;
        private void Awake()
        {
            Init();
        }
        protected void Init()
        {
            _boss = GetComponentInParent<BossAttackData>();
        }
        public virtual void StopAttacking() { }
    }
}
