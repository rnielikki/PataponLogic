using UnityEngine;

namespace PataRoad.Core.Character.Equipments.Weapons
{
    class ThunderStaff : CastingStaff
    {
        private Stat _stat;
        private ParticleSystem _particleSystem;
        [Header("Particle Range")]
        [SerializeField]
        float _normalRange;
        [SerializeField]
        float _chargedRange;
        public override void Initialize(SmallCharacter holder)
        {
            _particleSystem = GetComponent<ParticleSystem>();
            base.Initialize(holder);
        }
        public override void NormalAttack()
        {
            var shape = _particleSystem.shape;
            shape.radius = _normalRange;
            _stat = _holder.Stat;
            SetPosition();
            gameObject.SetActive(true);
            base.NormalAttack();
            AfterAttack();
        }
        public override void ChargeAttack()
        {
            var shape = _particleSystem.shape;
            shape.radius = _chargedRange;
            _stat = _holder.Stat;
            gameObject.SetActive(true);
            SetPosition();
            base.ChargeAttack();
            AfterAttack();
        }
        private void SetPosition()
        {
            var pos = _holder?.DistanceCalculator?.GetClosestForAttack();
            if (pos == null) return;
            transform.position = pos.Value;
        }
        private void AfterAttack()
        {
            StartCoroutine(DisappearAfterTime());

            System.Collections.IEnumerator DisappearAfterTime()
            {
                yield return new WaitForSeconds(Mathf.Min(_stat.AttackSeconds, Rhythm.RhythmEnvironment.TurnSeconds));
                gameObject.SetActive(false);
            }
        }
    }
}
