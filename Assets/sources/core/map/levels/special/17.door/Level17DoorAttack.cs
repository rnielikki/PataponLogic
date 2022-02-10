using PataRoad.Core.Character;
using PataRoad.Core.Character.Equipments.Weapons;
using PataRoad.Core.Rhythm;
using UnityEngine;

namespace PataRoad.Core.Map.Levels.Level17Door
{
    class Level17DoorAttack : Structure, IAttacker
    {
        CannonStructure _cannon;
        ParticleDamaging[] _bullets;
        private int _layerMask;

        public AttackType AttackType { get; private set; }
        public ElementalAttackType ElementalAttackType { get; private set; }
        private bool _attacking;

        private void Start()
        {
            _cannon = GetComponentInChildren<CannonStructure>();
            _bullets = GetComponentsInChildren<ParticleDamaging>();

            RhythmTimer.Current.OnTime.AddListener(AutoAttack);
            Rhythm.Command.TurnCounter.OnTurn.AddListener(SetTurn);
            _layerMask = LayerMask.GetMask("patapons");
        }
        private void CalculateAttack()
        {
            _attacking = true;
            var raycast = Physics2D.BoxCast(transform.position, Vector2.one * 2, 0, Vector2.left);
            var collider = raycast.collider;
            float pikeChance = 0;
            float bulletChance = 0;
            if (collider == null)
            {
                pikeChance = 0;
                bulletChance = 0.1f;
            }
            else
            {
                var distance = collider.ClosestPoint(transform.position).x - transform.position.x;
                if (distance <= 5.5f)
                {
                    pikeChance = 0.6f;
                    bulletChance = 0.6f;
                }
                else
                {
                    pikeChance = 0.05f;
                    bulletChance = 0.6f;
                }
            }
            if (Common.Utils.RandomByProbability(pikeChance))
            {
                PikeAttack();
            }
            else if (Common.Utils.RandomByProbability(bulletChance))
            {
                BulletAttack();
            }
            else
            {
                CannonAttack();
            }
        }
        private void AutoAttack()
        {
            if (!Rhythm.Command.TurnCounter.IsOn && !_attacking)
            {
                CalculateAttack();
            }
        }
        private void SetTurn()
        {
            if (Rhythm.Command.TurnCounter.IsPlayerTurn && !_attacking)
            {
                CalculateAttack();
            }
        }
        public void StopAttacking() //Stops every attacking
        {
            _animator.Play("Idle");
            _attacking = false;
            _cannon.StopAttacking();
        }
        public void EndAttack() //Called in end of the attack animation
        {
            _attacking = false;
        }
        private void BulletAttack()
        {
            AttackType = AttackType.Magic;
            ElementalAttackType = ElementalAttackType.Ice;
            _animator.Play("bullet");
        }
        public void ShootBullets()
        {
            foreach (var bullet in _bullets)
            {
                bullet.Emit(10, 30);
            }
        }
        private void PikeAttack()
        {
            AttackType = AttackType.Stab;
            ElementalAttackType = ElementalAttackType.Neutral;
            _animator.Play("pike");
        }
        private void CannonAttack()
        {
            AttackType = AttackType.Crush;
            ElementalAttackType = ElementalAttackType.Fire;
            _cannon.IsAnimatorUpdatingAngle = true;
            _animator.Play("cannon");
        }
        private void ShootCannon()
        {
            _cannon.StartAttack();
        }

        public float GetAttackValueOffset() => 1;
        public override void Die()
        {
            StopAttacking();
            base.Die();
        }

        public void OnAttackHit(Vector2 point, int damage)
        {
            //
        }

        public void OnAttackMiss(Vector2 point)
        {
            //
        }
    }
}
