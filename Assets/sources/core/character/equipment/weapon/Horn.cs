using UnityEngine;

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

        private void Start()
        {
            Init();
            _targetTransform = transform.Find("Attack");
            _attackParticles = _targetTransform.GetComponent<ParticleDamaging>();
            _feverAttackObject = GetWeaponInstance("Mega-FeverAttack");
            _chargeDefenceObject = GetWeaponInstance("Mega-ChargeDefence");
        }

        public override void Attack(AttackCommandType attackCommandType)
        {
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
            _attackParticles.Emit(emitCount, startSpeed);
        }
        private void AttackFever()
        {
            var newStat = Holder.Stat.Copy();
            newStat.FireRate += 0.5f;
            CreateBulletInstance(_feverAttackObject, MoveBulletOnGround, newStat).AddForce(Holder.MovingDirection * 5);
        }
        private void ChargeDefend()
        {
            var newStat = Holder.Stat.Copy();
            newStat.Knockback += 2;
            CreateBulletInstance(_chargeDefenceObject, StopBulletOnGround, newStat).AddForce(Holder.MovingDirection * 1000);
        }
        private Rigidbody2D CreateBulletInstance(GameObject targetObject, UnityEngine.Events.UnityAction<Collider2D> groundAction, Stat stat, bool fixedRotation = false)
        {
            var instance = Instantiate(targetObject, transform.root.parent);
            instance.transform.position = _targetTransform.position;
            instance.layer = gameObject.layer;

            if (!fixedRotation) instance.transform.rotation = _targetTransform.rotation;
            var bulletScript = instance.GetComponent<WeaponBullet>();
            bulletScript.Holder = Holder;
            bulletScript.Stat = stat;
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
