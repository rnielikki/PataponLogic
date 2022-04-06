using UnityEngine;

namespace PataRoad.Core.Character.Bosses
{
    class ZaknelWheelAttack : BossAttackComponent
    {
        Transform _pataponsTransform;
        Vector3 _wheelTargetPosition => _pataponsTransform.position +
            (_boss.CharacterSize * (Vector3)_boss.Boss.MovingDirection);

        private void Start()
        {
            _pataponsTransform = Patapons.PataponsManager.Current.transform;
        }
        public void Attack()
        {
            _enabled = true;
        }
        public override void StopAttacking()
        {
            base.StopAttacking();
            _enabled = false;
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            var character = collision.GetComponentInParent<IAttackable>();
            if (character != null && !character.StatusEffectManager.IsOnStatusEffect)
            {
                if (character is SmallCharacter small) small.StatusEffectManager.SetKnockback();
                var mono = character as MonoBehaviour;
                _boss.Attack(this, mono.gameObject,
                    collision.ClosestPoint(mono.transform.position), _attackType, _elementalAttackType, false);
            }
        }
        private void Update()
        {
            if (_enabled)
            {
                _boss.transform.position = Vector3.MoveTowards(_boss.transform.position, _wheelTargetPosition, Time.deltaTime * 15);
            }
        }
    }
}
