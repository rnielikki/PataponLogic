using UnityEngine;

namespace PataRoad.Core.Character.Equipments.Weapons
{
    class Horn : Weapon
    {
        private Transform _targetTransform; //transform of the bullet object when fired
        private ParticleDamaging _attackParticles;
        private GameObject _feverAttackObject;
        private GameObject _chargeDefenceObject;
        protected float _forceMultiplier = 1;
        protected float _feverPonponForceMultiplier = 1;
        private float _savedWindValue;

        private bool _ifFire;

        private void Start()
        {
            Init();
            _targetTransform = transform.Find("Attack");
            _attackParticles = _targetTransform.GetComponent<ParticleDamaging>();
            _feverAttackObject = GetWeaponInstance("Mega-FeverAttack");
            _chargeDefenceObject = GetWeaponInstance("Mega-ChargeDefence");

            if (Holder != null) _ifFire = Holder.AttackTypeIndex == 0;
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
                    emitCount = 5;
                    break;
                case AttackCommandType.Defend:
                    startSpeed = 3 * _forceMultiplier;
                    emitCount = 5;
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
                .AddForce(Holder.MovingDirection * 15 * _feverPonponForceMultiplier);
        }
        private void ChargeDefend()
        {
            var newStat = Holder.Stat.Copy();
            newStat.Knockback = 0; //Knockback independent.
            CreateBulletInstance(_chargeDefenceObject, StopBulletOnGround, PushBack, newStat, default)
                .AddForce(Holder.MovingDirection * 1000 * _forceMultiplier);
        }
        private Rigidbody2D CreateBulletInstance(GameObject targetObject,
            UnityEngine.Events.UnityAction<Collider2D, Vector2> groundAction,
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
        private static void MoveBulletOnGround(Collider2D self, Vector2 direction)
        {
            self.transform.rotation = Quaternion.identity;
            self.transform.position += Vector3.up * -0.5f;
            self.attachedRigidbody.AddForce(direction * 200);
            self.attachedRigidbody.constraints = RigidbodyConstraints2D.FreezePositionY;
            self.attachedRigidbody.gravityScale = 0;
        }
        //Charge Defence bullet
        private static void StopBulletOnGround(Collider2D self, Vector2 vector2)
        {
            self.attachedRigidbody.gravityScale = 0;
            self.attachedRigidbody.Sleep();
        }
        private static void PushBack(Collider2D other)
        {
            other.GetComponentInParent<SmallCharacter>()?.StatusEffectManager?.SetKnockback();
        }
        internal override void SetLastAttackCommandType(AttackCommandType attackCommandType)
        {
            base.SetLastAttackCommandType(attackCommandType);
            _savedWindValue = Map.Weather.WeatherInfo.Current.Wind.Magnitude;
        }
        public override float GetAttackDistance()
        {
            switch (LastAttackCommandType)
            {
                case AttackCommandType.Charge:
                case AttackCommandType.Attack:
                    return 17.5f + _savedWindValue * 0.5f;
                case AttackCommandType.FeverAttack:
                case AttackCommandType.Defend:
                    return 15 + _savedWindValue * 0.5f;
                default:
                    return 0;
            }
        }
    }
}
