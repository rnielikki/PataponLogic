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
        private void Start()
        {
            _animator = CharAnimator.Animator;
            _dragonAnimator = _animator.runtimeAnimatorController;
        }
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
        internal override void UpdateStatForBoss(int level)
        {
            throw new System.NotImplementedException();
        }
    }
}