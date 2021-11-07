using UnityEngine;

namespace PataRoad.Core.Character.Equipment.Weapon
{
    class Horn : WeaponObject
    {
        private Transform _targetTransform; //transform of the bullet object when fired
        private ParticleSystem _attackParticles;
        private GameObject _feverAttackObject;
        private GameObject _chargeDefenceObject;
        public override float MinAttackDistance { get; } = 10;
        public override float WindAttackDistanceOffset { get; } = 5;
        private Vector2 _direction;

        private void Start()
        {
            Init();
            _direction = (Holder is Patapons.Patapon) ? Vector2.right : Vector2.left;
            _targetTransform = transform.Find("Attack");
            _attackParticles = _targetTransform.GetComponent<ParticleSystem>();
            _feverAttackObject = GetWeaponInstance("Mega-FeverAttack");
            _chargeDefenceObject = GetWeaponInstance("Mega-ChargeDefence");
        }

        public override void Attack(AttackCommandType attackCommandType)
        {
            var main = _attackParticles.main;
            int startSpeed = 0;
            int emitCount = 0;
            switch (attackCommandType)
            {
                case AttackCommandType.FeverAttack:
                    AttackFever();
                    return;
                case AttackCommandType.Attack:
                    //Attack is called in two times in animation, so doesn't need so many emit count.
                    startSpeed = 6;
                    emitCount = 4;
                    break;
                case AttackCommandType.Defend:
                    startSpeed = 3;
                    emitCount = 3;
                    break;
                case AttackCommandType.Charge:
                    startSpeed = 2;
                    emitCount = 2;
                    break;
                case AttackCommandType.ChargeDefend:
                    ChargeDefend();
                    break;
            }
            main.startSpeed = startSpeed;
            _attackParticles.Emit(emitCount);
        }
        private void AttackFever()
        {
            CreateBulletInstance(_feverAttackObject, MoveBulletOnGround).AddForce(Holder.MovingDirection * 5);
        }
        private void ChargeDefend()
        {
            CreateBulletInstance(_chargeDefenceObject, StopBulletOnGround, true).AddForce(Holder.MovingDirection * 1000);
        }
        private Rigidbody2D CreateBulletInstance(GameObject targetObject, UnityEngine.Events.UnityAction<Collider2D> groundAction, bool fixedRotation = false)
        {
            var instance = Instantiate(targetObject, transform.root.parent);
            instance.transform.position = _targetTransform.position;
            instance.layer = gameObject.layer;

            if (!fixedRotation) instance.transform.rotation = _targetTransform.rotation;
            var bulletScript = instance.GetComponent<WeaponBullet>();
            bulletScript.Holder = Holder;
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
    }
}
