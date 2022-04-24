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
            _movingBackQueued = false;
            CharAnimator.Animate("frozen");
            _status = ManborothStatus.Frozen;
            _currentIceHitPoint = _iceHitPointInZero;
            if (!first)
            {
                foreach (var patapon in _pataponsManager.Patapons)
                {
                    patapon.StatusEffectManager.SetIce(2);
                }
            }
        }
        protected override bool CanContinue() => base.CanContinue() && !_frozen && !_changingPhase;
        protected override void StartMovingBack()
        {
            _changingPhase = true;
            BossAttackData.IgnoreStatusEffect();
            BossTurnManager.DefineNextAction("freeze");
        }
        public override bool TakeDamage(int damage)
        {
            if (_changingPhase) return true;
            if (_status == ManborothStatus.NotFrozen) base.TakeDamage(damage);
            else
            {
                _currentIceHitPoint -= damage;
                if (_currentIceHitPoint <= _iceHitPointInOne)
                {
                    if (_currentIceHitPoint <= 0)
                    {
                        _status = ManborothStatus.NotFrozen;
                        _ice.sprite = _notFrozenImage;
                        _frozen = false;
                        BossAttackData.StopIgnoringStatusEffect();
                    }
                    else if (_status == ManborothStatus.Frozen)
                    {
                        _status = ManborothStatus.HalfFrozen;
                        _ice.sprite = _halfFrozenImage;
                    }
                }
            }
            return true; //always displays damage.
        }
    }
}
