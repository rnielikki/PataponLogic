using System.Linq;
using UnityEngine;

namespace PataRoad.Core.Character.Bosses
{
    public class GaruruAttack : BossAttackData
    {
        private Animator _animator;
        [SerializeField]
        private bool _isMonsterForm;
        public bool IsMonsterForm => _isMonsterForm;
        [Header("Monster form")]
        [SerializeField]
        private RuntimeAnimatorController _monsterAniamtor;
        private RuntimeAnimatorController _dragonAnimator;
        [SerializeReference]
        private Stat _monsterStat = Stat.GetAnyDefaultStatForCharacter();
        [SerializeField]
        private AttackTypeResistance _monsterAttackTypeResistance;
        public override Stat Stat => _isMonsterForm ? _monsterStat : _stat;
        public override AttackTypeResistance AttackTypeResistance =>
            _isMonsterForm ? _monsterAttackTypeResistance : _attackTypeResistance;
        [SerializeField]
        private BossParticleCollision _poison;

        [SerializeField]
        private float _rushOffset;
        [SerializeField]
        private float _moveOffset;
        private float _movementSpeed;

        Vector2 _pushForce = new Vector2(500, 1500);

        //---Dragon
        GaruruBall _ball;
        AbsorbComponent _pickingHand;

        //--Monster
        float _rushTarget;
        CameraController.CameraMover _cameraMover;

        private Collider2D[] _raycastColliders;
        private Collider2D[] _nonTriggerColliders;
        [SerializeField]
        Collider2D _rushPushbackCollider;

        private bool _rushAttacking;
        private bool _movingToNormal;
        private float _restorePosition =>
            Patapons.PataponsManager.Current.transform.position.x + CharacterSize + 1;

        private int _nonRaycastLayer;
        private int _raycastLayer;

        //--Common
        bool _moving;
        private Vector2 _targetPosition;
        bool _isEnemyBoss;
        GaruruEnemyBoss _enemy;

        private void Start()
        {
            CharacterSize = 6;
            _animator = CharAnimator.Animator;
            _dragonAnimator = _animator.runtimeAnimatorController;
            _ball = GetComponentInChildren<GaruruBall>(true);
            _pickingHand = GetComponentInChildren<AbsorbComponent>(true);
            _cameraMover = Camera.main.GetComponent<CameraController.CameraMover>();
            _movementSpeed = Boss.Stat.MovementSpeed;

            if (Boss is GaruruEnemyBoss enemy) //rushing is annoying
            {
                _isEnemyBoss = true;
                _enemy = enemy;

                //only works with boss. not used for summons
                var layers = CharacterTypeDataCollection.GetCharacterData(CharacterType.Others);

                var allColliders = GetComponentsInChildren<Collider2D>(true);

                _raycastLayer = layers.SelfLayerMaskRayCast;
                _nonRaycastLayer = layers.SelfLayerMaskNoRayCast;

                _raycastColliders = allColliders.Where(col =>
                col.gameObject.layer == _raycastLayer && col != _rushPushbackCollider).ToArray();

                _nonTriggerColliders = allColliders
                    .Where(col => !col.isTrigger && col != _rushPushbackCollider).ToArray();
            }
        }
        //----------------- form change
        public void ChangeForm()
        {
            IgnoreStatusEffect();
            AttackPaused = true;
            CharAnimator.Animate("change");
        }
        public void ChangeAnimator()
        {
            AttackPaused = false;
            _isMonsterForm = !_isMonsterForm;
            _animator.runtimeAnimatorController =
                (_isMonsterForm) ? _monsterAniamtor : _dragonAnimator;
            StopIgnoringStatusEffect();

            if (_isEnemyBoss)
            {
                _enemy.ChangedForm();
            }
            CharAnimator.Animate("Idle");
        }
        //----------------- form dragon
        public void ShowBall() => _ball.Show();
        public void HideBall() => _ball.StopAttacking();
        public void BallAttack() => _ball.Attack();
        public void IceAttack()
        {
            foreach (var target in Boss.DistanceCalculator.GetAllAbsoluteTargetsOnFront())
            {
                var character = target.GetComponentInParent<ICharacter>();
                if (character != null)
                {
                    Equipments.Logic.DamageCalculator.CalculateAndSetStatusEffect(
                        character, StatusEffectType.Ice, 1, character.Stat.IceResistance);
                    if (character.StatusEffectManager is SmallCharacterStatusEffectManager st)
                    {
                        st.AddForce(_pushForce);
                    }
                }
            }
        }
        public void Pick() => _pickingHand.Attack();
        public void StopPicking() => _pickingHand.StopAttacking();
        public void Burn() => _pickingHand.StartAbsorbing(false);
        public void ReleasePausingAttack() => AttackPaused = false;

        //----------------- form beast
        public void PoisonAttack() => _poison.Attack();
        public void SetRushTarget()
        {
            AttackPaused = true;
            _rushTarget = Patapons.PataponsManager.Current.transform.position.x - _rushOffset;
            _cameraMover.SetTarget(transform, false);
            _cameraMover.SetCameraOffset(0);
            _movementSpeed = _moveOffset / 2;
            MovePosition(-999);
        }
        public void Rush()
        {
            _rushAttacking = true;
            MoveAbsolutePosition(_rushTarget);
            _movementSpeed = (_rushOffset + _moveOffset) / 2.25f;
            _rushPushbackCollider.enabled = true;
            foreach (var collider in _raycastColliders)
            {
                collider.gameObject.layer = _nonRaycastLayer;
            }
            foreach (var collider in _nonTriggerColliders)
            {
                collider.isTrigger = true;
            }
        }
        public void AfterRush()
        {
            _moving = false;
            _cameraMover.SetTarget(Patapons.PataponsManager.Current.transform, true, 3);
            _cameraMover.SetCameraOffset(Patapons.PataponsManager.CameraOffsetOnNonZoom);
        }
        private void MoveToDefault()
        {
            _movingToNormal = true;
            _movementSpeed = Boss.Stat.MovementSpeed;
            MoveAbsolutePosition(_restorePosition);
        }
        public void BackToNormalPosition()
        {
            _movementSpeed = Boss.Stat.MovementSpeed;
            foreach (var collider in _raycastColliders)
            {
                collider.gameObject.layer = _raycastLayer;
            }
            foreach (var collider in _nonTriggerColliders)
            {
                collider.isTrigger = false;
            }
            _rushPushbackCollider.enabled = false;
            _enemy.BossTurnManager.EndAttack();
            _movingToNormal = false;
            AttackPaused = false;
            _moving = false;
            _rushAttacking = false;
        }
        private void RestorePosition() //patapon position system is somewhat annoying w this...
        {
            transform.position = new Vector3(_restorePosition,
                transform.position.y, transform.position.z);
            BackToNormalPosition();
        }

        //----------------- form common
        public override void StopAllAttacking()
        {
            if (_rushAttacking)
            {
                RestorePosition();
            }
            _ball.StopAttacking();
            _pickingHand.StopAttacking();
            _poison.StopAttacking();
            base.StopAllAttacking();
            _moving = false;
            _movingToNormal = false;
        }
        public void MovePosition(float pos)
        {
            if (_moving) return;
            MoveAbsolutePosition(pos);
            _targetPosition.x = transform.position.x
                + _targetPosition.x * Boss.MovingDirection.x;
        }
        private void MoveAbsolutePosition(float pos)
        {
            if (_moving) return;
            _targetPosition = transform.position;
            _targetPosition.x = pos;
            _moving = true;
        }
        public void StopMoving() => _moving = false;

        internal override void UpdateStatForBoss(int level)
        {
            UpdateStatForBoss(level, _stat);
            UpdateStatForBoss(level, _monsterStat);
            Boss.SetMaximumHitPoint(_stat.HitPoint);
        }
        private void UpdateStatForBoss(int level, Stat stat)
        {
            var value = 0.8f + (level * 0.25f);
            stat.MultipleDamage(value);
            stat.DefenceMin += (level - 1) * 0.005f;
            stat.DefenceMax += (level - 1) * 0.01f;
            stat.HitPoint = Mathf.RoundToInt(stat.HitPoint * value);
            stat.AddCriticalResistance(level * 0.05f);
            stat.AddStaggerResistance(level * 0.05f);
            stat.AddFireResistance(level * 0.03f);
            stat.AddIceResistance(level * 0.03f);
            stat.AddSleepResistance(level * 0.03f);
        }
        private void Update()
        {
            if (_moving)
            {
                transform.position = Vector2.MoveTowards(
                    transform.position, _targetPosition, _movementSpeed * Time.deltaTime);
                if (transform.position.x == _targetPosition.x)
                {
                    _moving = false;
                    if (_movingToNormal) BackToNormalPosition();
                }
            }
        }
    }
}