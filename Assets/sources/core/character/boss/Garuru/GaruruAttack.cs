using UnityEngine;

namespace PataRoad.Core.Character.Bosses
{
    public class GaruruAttack : BossAttackData
    {
        private Animator _animator;
        private bool _isMonsterForm;
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
        [SerializeField]
        private Collider2D _headbutt;

        Vector2 _pushForce = new Vector2(500, 1500);

        //---Dragon
        GaruruBall _ball;
        AbsorbComponent _pickingHand;

        //--Monster
        float _rushTarget;
        CameraController.CameraMover _cameraMover;

        private Collider2D[] _allColliders;
        private float _altMoveSpeed;

        //--Common
        bool _moving;
        private Vector2 _targetPosition;

        private void Start()
        {
            CharacterSize = 6;
            _animator = CharAnimator.Animator;
            _dragonAnimator = _animator.runtimeAnimatorController;
            _ball = GetComponentInChildren<GaruruBall>();
            _pickingHand = GetComponentInChildren<AbsorbComponent>();
            _cameraMover = Camera.main.GetComponent<CameraController.CameraMover>();
            _movementSpeed = Boss.Stat.MovementSpeed;
            _allColliders = GetComponentsInChildren<Collider2D>(true);
            _altMoveSpeed = (_rushOffset + _moveOffset) / 2;


            _isMonsterForm = true;
        }
        //----------------- form change
        public void ChangeForm()
        {
            IgnoreStatusEffect();
            CharAnimator.Animate("change");
        }
        public void ChangeAnimator()
        {
            _isMonsterForm = !_isMonsterForm;
            _animator.runtimeAnimatorController =
                (_isMonsterForm) ? _monsterAniamtor : _dragonAnimator;
            StopIgnoringStatusEffect();
        }
        //----------------- form dragon
        public void ShowBall() => _ball.Show();
        public void HideBall() => _ball.StopAttacking();
        public void BallAttack() => _ball.Attack();
        public void IceAttack()
        {
            foreach (var target in Boss.DistanceCalculator.GetAllAbsoluteTargetsOnFront())
            {
                var character = target.GetComponent<ICharacter>();
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
        public void Burn() => _pickingHand.StartAbsorbing();

        //----------------- form beast
        public void PoisonAttack() => _poison.Attack();
        public void SetRushTarget()
        {
            UseCustomDataPosition = true;
            _rushTarget = Patapons.PataponsManager.Current.transform.position.x - _rushOffset;
            _cameraMover.SetTarget(transform, false);
            _movementSpeed = _altMoveSpeed;
            MovePosition(-999);
        }
        public void Rush()
        {
            MoveAbsolutePosition(_rushTarget);
            _movementSpeed = _altMoveSpeed + _rushOffset;
            foreach (var collider in _allColliders)
            {
                if (collider != _headbutt) collider.enabled = false;
            }
        }
        public void AfterRush()
        {
            _moving = false;
            _cameraMover.SetTarget(Patapons.PataponsManager.Current.transform, true);
        }
        private void MoveToDefault()
        {
            _movementSpeed = _rushOffset * 20;
            MoveAbsolutePosition(
                Patapons.PataponsManager.Current.transform.position.x + CharacterSize + 1);
        }
        public void BackToNormalPosition()
        {
            _movementSpeed = Boss.Stat.MovementSpeed;
            foreach (var collider in _allColliders)
            {
                if (collider != _headbutt) collider.enabled = true;
            }
            UseCustomDataPosition = false;
            _moving = false;
        }

        //----------------- form common
        public override void StopAllAttacking()
        {
            _ball.StopAttacking();
            _pickingHand.StopAttacking();
            _poison.StopAttacking();
            base.StopAllAttacking();
            _moving = false;
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
            var value = 0.8f + (level * 0.25f);
            _stat.MultipleDamage(value);
            _stat.DefenceMin += (level - 1) * 0.005f;
            _stat.DefenceMax += (level - 1) * 0.01f;
            Boss.SetMaximumHitPoint(Mathf.RoundToInt(_stat.HitPoint * value));
            _stat.CriticalResistance += level * 0.05f;
            _stat.StaggerResistance += level * 0.05f;
            _stat.FireResistance += level * 0.03f;
            _stat.IceResistance += level * 0.03f;
            _stat.SleepResistance += level * 0.03f;
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
                }
            }
        }
    }
}