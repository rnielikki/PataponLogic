using UnityEngine;

namespace PataRoad.Core.Character.Bosses
{
    public class DodongaEatingComponent : BossAttackComponent
    {
        private bool _enabled;
        private bool _pataponEaten;
        private Collider2D _collider;
        [SerializeField]
        private AudioClip OnPataponEaten;
        private void Awake()
        {
            Init();
            _collider = GetComponent<Collider2D>();
        }
        internal void Attack()
        {
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
                    patapon.BeEaten();
                    GameSound.SpeakManager.Current.Play(OnPataponEaten);
                    _pataponEaten = true;
                }
            }
        }
    }
}
