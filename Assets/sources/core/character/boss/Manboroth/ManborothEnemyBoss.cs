using UnityEngine;

namespace PataRoad.Core.Character.Bosses
{
    class ManborothEnemyBoss : EnemyBoss
    {
        [SerializeField]
        SpriteRenderer _ice;
        [SerializeField]
        Sprite _halfFrozenImage;
        [SerializeField]
        Sprite _notFrozenImage;
        [SerializeField]
        int _iceHitPointInZero;
        [SerializeField]
        int _iceHitPointInOne;

        int _currentIceHitPoint;
        bool _frozen;
        bool _changingPhase;

        private enum ManborothStatus
        {
            Frozen,
            HalfFrozen,
            NotFrozen
        }
        private ManborothStatus _status = ManborothStatus.Frozen;
        public override void Init()
        {
            base.Init();
            _sleeping = false;
            Freeze(true);
        }
        public void Freeze() => Freeze(false);
        private void Freeze(bool first)
        {
            _frozen = true;
            _changingPhase = false;
            CharAnimator.Animate("frozen");
            _status = ManborothStatus.Frozen;
            _currentIceHitPoint = _iceHitPointInZero + _iceHitPointInOne;
            if (!first)
            {
                foreach (var patapon in _pataponsManager.Patapons)
                {
                    patapon.StatusEffectManager.SetIce(2);
                }
            }
            BossAttackData.IgnoreStatusEffect();
            StatusEffectManager.IgnoreStatusEffect = true;
        }
        protected override bool CanContinue() => base.CanContinue() && !_frozen && !_changingPhase;
        protected override void StartMovingBack()
        {
            _changingPhase = true;
            BossTurnManager.DefineNextAction("freeze");
        }
        public override void TakeDamage(int damage)
        {
            if (_changingPhase) return;
            if (_status == ManborothStatus.NotFrozen) base.TakeDamage(damage);
            else
            {
                _currentIceHitPoint -= damage;
                if (_currentIceHitPoint <= _iceHitPointInOne)
                {
                    if (_frozen)
                    {
                        BossAttackData.StopIgnoringStatusEffect();
                        CharAnimator.Animate("Idle");
                        _frozen = false;
                    }
                    if (_currentIceHitPoint <= 0)
                    {
                        _status = ManborothStatus.NotFrozen;
                        _ice.sprite = _notFrozenImage;
                    }
                    else if (_status == ManborothStatus.Frozen)
                    {
                        _status = ManborothStatus.HalfFrozen;
                        _ice.sprite = _halfFrozenImage;
                    }
                }
            }
        }
    }
}
