using UnityEngine;

namespace PataRoad.Core.Character.Bosses
{
    class ZaknelWheelAttack : BossAttackComponent
    {
        Transform _pataponsTransform;
        Vector3 _wheelTargetPosition => _pataponsTransform.position + (_boss.CharacterSize * Vector3.right);

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
            if (!collision.CompareTag("SmallCharacter")) return;
            var character = collision.GetComponentInParent<SmallCharacter>();
            if (!character.StatusEffectManager.IsOnStatusEffect)
            {
                character.StatusEffectManager.SetKnockback();
                _boss.Attack(this, character.gameObject,
                    collision.ClosestPoint(character.transform.position), _attackType, _elementalAttackType, false);
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
