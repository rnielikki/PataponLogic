using UnityEngine;

namespace Core.Character.Equipment.Weapon
{
    class Horn : WeaponObject
    {
        private Transform _targetTransform; //transform of the bullet object when fired
        private ParticleSystem _attackParticles;
        private GameObject _feverAttackObject;
        private GameObject _chargeDefenceObject;
        private void Awake()
        {
            Init();
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
                    emitCount = 5;
                    break;
                case AttackCommandType.Defend:
                    startSpeed = 3;
                    emitCount = 5;
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
            CreateBulletInstance(_feverAttackObject, MoveBulletOnGround).AddForce(Vector2.right * 0.5f);
        }
        private void ChargeDefend()
        {
            CreateBulletInstance(_chargeDefenceObject, StopBulletOnGround, true).AddForce(Vector2.right * 0.75f);
        }
        private Rigidbody2D CreateBulletInstance(GameObject targetObject, UnityEngine.Events.UnityAction<Collider2D> groundAction, bool fixedRotation = false)
        {
            var instance = Instantiate(targetObject, transform.root.parent);
            instance.transform.position = _targetTransform.position;
            if (!fixedRotation) instance.transform.rotation = _targetTransform.rotation;
            instance.GetComponent<WeaponBullet>().GroundAction = groundAction;
            instance.SetActive(true);
            var rb = instance.GetComponent<Rigidbody2D>();
            rb.mass = 0.001f;
            return rb;
        }
        //Fever Attack bullet
        private void MoveBulletOnGround(Collider2D self)
        {
            self.attachedRigidbody.gravityScale = 0;
            self.attachedRigidbody.velocity = self.attachedRigidbody.velocity.x * Vector2.right;
            self.transform.rotation = Quaternion.identity;
            self.transform.Translate(transform.up * -0.5f);
        }
        //Charge Defence bullet
        private void StopBulletOnGround(Collider2D self)
        {
            self.attachedRigidbody.gravityScale = 0;
            self.attachedRigidbody.velocity = Vector3.zero;
        }
    }
}
