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

        Vector2 _pushForce = new Vector2(500, 1500);

        //---
        GaruruBall _ball;
        AbsorbComponent _pickingHand;

        //--
        bool _moving;
        private Vector2 _targetPosition;

        private void Start()
        {
            _animator = CharAnimator.Animator;
            _dragonAnimator = _animator.runtimeAnimatorController;
            _ball = GetComponentInChildren<GaruruBall>();
            _pickingHand = GetComponentInChildren<AbsorbComponent>();
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
            _targetPosition = transform.position;
            _targetPosition.x += pos * Boss.MovingDirection.x;
            _moving = true;
        }
        public void StopMoving() => _moving = false;

        internal override void UpdateStatForBoss(int level)
        {
            throw new System.NotImplementedException();
        }
        private void Update()
        {
            if (_moving)
            {
                transform.position = Vector2.MoveTowards(
                    transform.position, _targetPosition, Boss.Stat.MovementSpeed * Time.deltaTime);
                if (transform.position.x == _targetPosition.x)
                {
                    _moving = false;
                }
            }
        }
    }
}