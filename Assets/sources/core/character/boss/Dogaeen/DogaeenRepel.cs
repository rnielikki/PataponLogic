using UnityEngine;

namespace PataRoad.Core.Character.Bosses
{
    class DogaeenRepel : BossAttackComponent
    {
        private Vector2 _center;
        private Vector2 _size;
        private LayerMask _layerMask;
        private void Start()
        {
            Init();
            var collider = GetComponent<Collider2D>();
            _center = collider.bounds.center;
            _size = collider.bounds.size + Vector3.one * 0.5f;
            _layerMask = GetComponentInParent<Boss>().DistanceCalculator.LayerMask;
        }
        public void Repel()
        {
            foreach (Collider2D target in Physics2D.OverlapBoxAll(_center, _size, transform.eulerAngles.z, _layerMask))
            {
                if (target.tag != "SmallCharacter") continue;
                _boss.Attack(this, target.gameObject, target.ClosestPoint(transform.position), _attackType, _elementalAttackType);
                target.GetComponentInParent<ICharacter>()?.StatusEffectManager?.SetKnockback();
            }
        }
    }
}
