﻿using UnityEngine;

namespace PataRoad.Core.Character.Equipments.Weapons
{
    class Horn : Weapon
    {
        private Transform _targetTransform; //transform of the bullet object when fired
        private ParticleDamaging _attackParticles;
        private GameObject _feverAttackObject;
        private GameObject _chargeDefenceObject;
        public override float MinAttackDistance { get; } = 10;
        public override float WindAttackDistanceOffset { get; } = 5;
        protected float _forceMultiplier = 1;
        protected float _feverPonponForceMultiplier = 1;

        private bool _ifFire;

        private void Start()
        {
            Init();
            _targetTransform = transform.Find("Attack");
            _attackParticles = _targetTransform.GetComponent<ParticleDamaging>();
            _feverAttackObject = GetWeaponInstance("Mega-FeverAttack");
            _chargeDefenceObject = GetWeaponInstance("Mega-ChargeDefence");

            if (Holder != null) _ifFire = Holder.GetAttackType() == 0;
        }

        public override void Attack(AttackCommandType attackCommandType)
        {
            float startSpeed = 0;
            int emitCount = 0;
            switch (attackCommandType)
            {
                case AttackCommandType.FeverAttack:
                    AttackFever();
                    return;
                case AttackCommandType.Attack:
                    //Attack is called in two times in animation, so doesn't need so many emit count.
                    startSpeed = 6 * _forceMultiplier;
                    emitCount = 4;
                    break;
                case AttackCommandType.Defend:
                    startSpeed = 3 * _forceMultiplier;
                    emitCount = 3;
                    break;
                case AttackCommandType.Charge:
                    startSpeed = 2 * _forceMultiplier;
                    emitCount = 2;
                    break;
                case AttackCommandType.ChargeDefend:
                    ChargeDefend();
                    break;
            }
            _attackParticles.Emit(emitCount, startSpeed);
        }
        private void AttackFever()
        {
            var newStat = Holder.Stat.Copy();
            if (_ifFire)
            {
                newStat.FireRate += 0.15f;
            }
            else
            {
                newStat.IceRate += 0.15f;
            }
            newStat.DamageMin *= 3;
            newStat.DamageMax *= 3;
            CreateBulletInstance(_feverAttackObject, MoveBulletOnGround, null, newStat, (_ifFire) ? Color.red : Color.blue)
                .AddForce(Holder.MovingDirection * 5 * _feverPonponForceMultiplier);
        }
        private void ChargeDefend()
        {
            var newStat = Holder.Stat.Copy();
            newStat.Knockback = 0; //Knockback independent.
            CreateBulletInstance(_chargeDefenceObject, StopBulletOnGround, PushBack, newStat, default)
                .AddForce(Holder.MovingDirection * 1000 * _forceMultiplier);
        }
        private Rigidbody2D CreateBulletInstance(GameObject targetObject,
            UnityEngine.Events.UnityAction<Collider2D> groundAction,
            UnityEngine.Events.UnityAction<Collider2D> collidingAction,
            Stat stat, Color color)
        {
            var instance = Instantiate(targetObject, transform.root.parent);
            instance.transform.position = _targetTransform.position;
            instance.layer = gameObject.layer;

            instance.transform.rotation = _targetTransform.rotation;
            if (color != default)
            {
                instance.GetComponent<SpriteRenderer>().color = color;
            }

            var bulletScript = instance.GetComponent<WeaponBullet>();
            bulletScript.Holder = Holder;
            bulletScript.Stat = stat;
            bulletScript.CollidingAction = collidingAction;
            bulletScript.GroundAction = groundAction;
            instance.SetActive(true);

            return instance.GetComponent<Rigidbody2D>();
        }
        //Fever Attack bullet
        private void MoveBulletOnGround(Collider2D self)
        {
            self.attachedRigidbody.AddForce(Holder.MovingDirection * 200);
            self.attachedRigidbody.constraints = RigidbodyConstraints2D.FreezePositionY;
            self.attachedRigidbody.gravityScale = 0;
            self.transform.rotation = Quaternion.identity;
            self.transform.Translate(transform.up * -0.5f);
        }
        //Charge Defence bullet
        private void StopBulletOnGround(Collider2D self)
        {
            self.attachedRigidbody.gravityScale = 0;
            self.attachedRigidbody.Sleep();
        }
        private void PushBack(Collider2D other)
        {
            other.GetComponentInParent<SmallCharacter>()?.StatusEffectManager?.SetKnockback();
        }
    }
}
