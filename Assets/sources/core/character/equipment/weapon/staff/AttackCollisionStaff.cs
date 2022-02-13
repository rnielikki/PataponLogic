using UnityEngine;

namespace PataRoad.Core.Character.Equipments.Weapons
{
    public class AttackCollisionStaff : MonoBehaviour, IStaffActions
    {
        private SmallCharacter _holder;
        private Collider2D _collider;
        protected SpriteRenderer _image;
        private Stat _stat;
        [SerializeField]
        float _minRandomOffset;
        [SerializeField]
        float _maxRandomOffset;

        protected float _attackSeconds => Mathf.Min(_stat.AttackSeconds, Rhythm.RhythmEnvironment.TurnSeconds);

        public virtual void Initialize(SmallCharacter holder)
        {
            _holder = holder;
            _stat = holder.Stat;
            transform.parent = transform.root;

            _collider = GetComponent<Collider2D>();
            _image = GetComponent<SpriteRenderer>();
            _holder = holder;
        }
        public virtual void ChargeAttack()
        {
            NormalAttack();
        }

        public virtual void Defend()
        {
            //no def.
        }

        public virtual void NormalAttack()
        {
            PerformAttack();
        }
        public virtual void SetElementalColor(Color color)
        {
            var c = color;
            c.a = _image.color.a;
            _image.color = c;
        }
        protected virtual void PerformAttack()
        {
            var pos = _holder?.DistanceCalculator?.GetClosestForAttack();
            if (pos == null) return;
            var position = pos.Value;

            position.x += Random.Range(_minRandomOffset, _maxRandomOffset);
            transform.position = position;
            gameObject.SetActive(true);

            StartCoroutine(DisappearAfterTime());

            System.Collections.IEnumerator DisappearAfterTime()
            {
                yield return new WaitForSeconds(_attackSeconds);
                gameObject.SetActive(false);
            }
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            AttackOnTrigger(collision);
        }
        protected bool AttackOnTrigger(Collider2D collision)
        {
            if (_collider == null || !_collider.enabled) return false;
            var point = collision.ClosestPoint(transform.position);
            Logic.DamageCalculator.DealDamage(_holder, _stat, collision.gameObject, point);
            return true;
        }
    }
}
