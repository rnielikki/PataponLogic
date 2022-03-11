using PataRoad.Common;
using UnityEngine;
using UnityEngine.Pool;

namespace PataRoad.Core.Character.Equipments.Weapons
{
    class Horn : Weapon
    {
        private IObjectPool<GameObject> _feverAttackPool;
        private IObjectPool<GameObject> _chargeDefencePool;
        private Transform _targetTransform; //transform of the bullet object when fired
        private ParticleDamaging _attackParticles;
        protected float _forceMultiplier = 1;
        protected float _feverPonponForceMultiplier = 1;
        private float _savedWindValue;

        private bool _ifFire;

        private void Start()
        {
            Init();
            _targetTransform = transform.Find("Attack");
            _attackParticles = _targetTransform.GetComponent<ParticleDamaging>();
            _attackParticles.Init(Holder);

            var poolObject = GameObject.Find(nameof(GameObjectPool));
            if (poolObject != null)
            {
                _feverAttackPool = poolObject
                .GetComponent<GameObjectPool>()
                .GetPool("Characters/Equipments/PrefabBase/Mega-FeverAttack", poolInitialSize, poolMaxSize);

                _chargeDefencePool = poolObject
                .GetComponent<GameObjectPool>()
                .GetPool("Characters/Equipments/PrefabBase/Mega-ChargeDefence", poolInitialSize, poolMaxSize);
            }

            if (Holder != null)
            {
                _ifFire = Holder.AttackTypeIndex == 0;
            }
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
                    startSpeed = 11 * _forceMultiplier;
                    emitCount = 5;
                    break;
                case AttackCommandType.Defend:
                    startSpeed = 10 * _forceMultiplier;
                    emitCount = 4;
                    break;
                case AttackCommandType.Charge:
                    startSpeed = 3 * _forceMultiplier;
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
            CreateBulletInstance(_feverAttackPool, MoveBulletOnGround, null, newStat, (_ifFire) ? Color.red : Color.blue)
                .AddForce(_feverPonponForceMultiplier * 50 * Holder.MovingDirection);
        }
        private void ChargeDefend()
        {
            var newStat = Holder.Stat.Copy();
            newStat.Knockback = 0; //Knockback independent.
            CreateBulletInstance(_chargeDefencePool, StopBulletOnGround, PushBack, newStat, default)
                .AddForce(_forceMultiplier * 1000 * Holder.MovingDirection);
        }
        private Rigidbody2D CreateBulletInstance(IObjectPool<GameObject> targetObjectPool,
            UnityEngine.Events.UnityAction<Collider2D, Vector2> groundAction,
            UnityEngine.Events.UnityAction<Collider2D> collidingAction,
            Stat stat, Color color)
        {
            var instance = targetObjectPool.Get();
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
            self.attachedRigidbody.constraints = RigidbodyConstraints2D.FreezePositionY;
            self.attachedRigidbody.gravityScale = 0;
            self.attachedRigidbody.AddForce(direction * 1000);

            self.transform.SetPositionAndRotation(Vector3.up * -0.5f + self.transform.position, Quaternion.identity);
        }
        //Charge Defence bullet
        private static void StopBulletOnGround(Collider2D self, Vector2 direction)
        {
            self.attachedRigidbody.gravityScale = 0;
            self.attachedRigidbody.AddForce(direction * 100);
            self.attachedRigidbody.Sleep();
        }
        private static void PushBack(Collider2D other)
        {
            var character = other.GetComponentInParent<SmallCharacter>();
            if (character != null && character.StatusEffectManager != null)
            {
                character.StatusEffectManager.SetKnockback();
            }
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
                    return 15 + (_savedWindValue * 0.5f);
                case AttackCommandType.FeverAttack:
                case AttackCommandType.Defend:
                    return 10 + (_savedWindValue * 0.5f);
                default:
                    return 0;
            }
        }
    }
}
