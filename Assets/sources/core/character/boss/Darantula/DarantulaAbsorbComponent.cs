using UnityEngine;

namespace PataRoad.Core.Character.Bosses
{
    class DarantulaAbsorbComponent : BossAttackComponent
    {
        private bool _enabled;
        private bool _pataponEaten;
        private Collider2D _collider;
        private DarantulaEnemy _darantula;

        private void Awake()
        {
            Init();
            _collider = GetComponent<Collider2D>();
            _darantula = GetComponentInParent<DarantulaEnemy>();
        }
        internal void Attack()
        {
            if (_darantula == null) return; //won't activate if it isn't enemy boss
            _enabled = true;
            _collider.isTrigger = true;
            _pataponEaten = false;
        }
        public override void StopAttacking()
        {
            _enabled = false;
            _collider.isTrigger = false;
            _pataponEaten = false;
        }
        private void OnTriggerStay2D(Collider2D collision)
        {
            if (_enabled && !_pataponEaten)
            {
                var patapon = collision.gameObject.GetComponentInParent<Patapons.Patapon>();
                if (patapon != null)
                {
                    patapon.BeTaken();
                    GameSound.SpeakManager.Current.Play(patapon.Sounds.OnDead);
                    patapon.transform.parent = transform;
                    _pataponEaten = true;

                    _darantula.SetAbsorbHit();
                }
            }
        }
    }
}
