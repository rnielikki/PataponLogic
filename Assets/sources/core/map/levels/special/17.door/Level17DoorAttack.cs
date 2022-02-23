using PataRoad.Core.Character;
using PataRoad.Core.Character.Equipments.Weapons;
using PataRoad.Core.Rhythm;
using UnityEngine;

namespace PataRoad.Core.Map.Levels
{
    class Level17DoorAttack : Spawn, IAttacker
    {
        CannonStructure _cannon;
        ParticleDamaging[] _bullets;

        public AttackType AttackType { get; private set; }
        public ElementalAttackType ElementalAttackType { get; private set; }
        private bool _attacking;
        private int _attackIndex;
        private UnityEngine.Events.UnityAction[] _actions;

        [SerializeField]
        private int _cannonDamage;
        [SerializeField]
        private int _bulletDamage;
        [SerializeField]
        private int _pikeDamage;

        protected override void Start()
        {
            base.Start();
            _cannon = GetComponentInChildren<CannonStructure>();
            _bullets = GetComponentsInChildren<ParticleDamaging>();
            foreach (var bullet in _bullets) bullet.Init(this);
            _actions = new UnityEngine.Events.UnityAction[]
            {
                PikeAttack,
                BulletAttack,
                CannonAttack
            };

            RhythmTimer.Current.OnTime.AddListener(AutoAttack);
            Rhythm.Command.TurnCounter.OnTurn.AddListener(SetTurn);

            FindObjectOfType<Rhythm.Command.RhythmCommand>().OnCommandCanceled.AddListener(StopAttacking);
            OnDestroy.AddListener(() => MissionPoint.Current.EndMission());
            StartCoroutine(SpawnEnemy());
        }
        private void CalculateAttack()
        {
            _attacking = true;
            _actions[_attackIndex]();
            _attackIndex = (_attackIndex + 1) % _actions.Length;
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
            Stat.DamageMax = _bulletDamage;

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
            Stat.DamageMax = _pikeDamage;

            _animator.Play("pike");
        }
        private void CannonAttack()
        {
            AttackType = AttackType.Crush;
            ElementalAttackType = ElementalAttackType.Fire;
            Stat.DamageMax = _cannonDamage;

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
            foreach (var enemy in transform.parent.GetComponentsInChildren<Character.Hazorons.Hazoron>())
            {
                enemy.DieWithoutInvoking();
            }
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
