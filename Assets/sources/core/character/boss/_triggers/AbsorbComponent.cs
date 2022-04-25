using PataRoad.Core.Character.Patapons;
using UnityEngine;

namespace PataRoad.Core.Character.Bosses
{
    class AbsorbComponent : BossAttackComponent
    {
        private Collider2D _collider;
        private IAbsorbableBossBehaviour _absorber;
        [SerializeField]
        private AudioClip _onDeadSound;
        [SerializeField]
        Vector3 _pataponOffset = new Vector3(3, -1, 0);
        [SerializeField]
        bool _playDyingSoundBeforeDeath;
        private Patapon _pataponToEat;

        private void Awake()
        {
            Init();
            _collider = GetComponent<Collider2D>();
            _absorber = GetComponentInParent<IAbsorbableBossBehaviour>();
            if (!(_absorber is EnemyBossBehaviour)) _absorber = null;
        }
        private void OnDisable()
        {
            StopAttacking();
        }
        internal void Attack()
        {
            if (_absorber == null) return; //won't activate if it isn't enemy boss
            _enabled = true;
            _collider.isTrigger = true;
        }
        public override void StopAttacking()
        {
            DisableAttacker();
            CancelAbsorbing();
        }
        public void DisableAttacker()
        {
            _enabled = false;
            _collider.isTrigger = false;
        }
        private void OnTriggerStay2D(Collider2D collision)
        {
            if (_enabled && _pataponToEat == null)
            {
                var patapon = collision.gameObject.GetComponentInParent<Patapon>();
                if (patapon != null && !patapon.IsDead)
                {
                    _pataponToEat = patapon;
                    patapon.BeTaken();
                    patapon.transform.SetParent(transform);
                    patapon.transform.localPosition = _pataponOffset;

                    if (_playDyingSoundBeforeDeath) PlayDyingSound(patapon);

                    _boss.AttackPaused = true;
                    (_boss.Boss as EnemyBoss).BossTurnManager.EndWithoutStopAttacking();
                    _absorber.SetAbsorbHit();
                }
            }
        }
        internal void StartAbsorbing(bool releasePauseAttack = true)
        {
            if (_absorber == null || _pataponToEat == null) return;
            if (releasePauseAttack) _boss.AttackPaused = false;
            if (!_playDyingSoundBeforeDeath) PlayDyingSound(_pataponToEat);
            _pataponToEat.EnsureDeath();
            _absorber.Heal(_pataponToEat.CurrentHitPoint);
            _pataponToEat = null;
        }
        private void CancelAbsorbing()
        {
            if (_absorber == null || _pataponToEat == null) return;
            _absorber.StopAbsorbing();
            _pataponToEat.CancelDeath();
            _pataponToEat = null;
            _boss.AttackPaused = false;
        }
        private void PlayDyingSound(Patapon patapon) =>
            GameSound.SpeakManager.Current.Play(
                        _onDeadSound == null ? patapon.Sounds.OnDead : _onDeadSound);

    }
}
