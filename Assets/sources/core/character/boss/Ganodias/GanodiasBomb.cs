using UnityEngine;

namespace PataRoad.Core.Character.Bosses
{
    class GanodiasBomb : BossAttackComponent
    {
        [SerializeField]
        Animator _animator;
        Sprite _bombImage;
        [SerializeField]
        SpriteRenderer _bomb;
        [SerializeField]
        ParticleSystem _particle;
        [SerializeField]
        Transform _startTransform;
        private LayerMask _layerMask;
        bool _loaded;

        private void Awake()
        {
            Init();
            _bombImage = _bomb.sprite;
        }
        private void Start()
        {
            HideBomb();
        }
        public void ShowBomb()
        {
            if (!_loaded)
            {
                _layerMask = GetComponentInParent<Boss>().DistanceCalculator.LayerMask;
                transform.parent = transform.root.parent;
                _loaded = true;
            }
            gameObject.SetActive(true);
            transform.position = _startTransform.position;
            _animator.Play("start");
            _particle.Play();
        }
        public void HideBomb()
        {
            gameObject.SetActive(false);
            _bomb.sprite = _bombImage;
            _bomb.color = Color.white;
        }
        public void StopParticle() => _particle.Stop();
        public void StartCounter()
        {
            _animator.SetBool("timer-on", true);
            int counter = 0;
            Rhythm.RhythmTimer.Current.OnTime.AddListener(ExplodeAfterTime);
            void ExplodeAfterTime()
            {
                counter++;
                if (counter == 5)
                {
                    StopParticle();
                    Rhythm.RhythmTimer.Current.OnTime.RemoveListener(ExplodeAfterTime);
                    Explode();
                    _animator.SetBool("timer-on", false);
                }
            }
        }
        public void Explode()
        {
            foreach (var target in Physics2D.OverlapCircleAll(_bomb.transform.position, 2f, _layerMask))
            {
                if (!target.CompareTag("SmallCharacter")) continue;
                _boss.Attack(this, target.gameObject, target.ClosestPoint(transform.position), _attackType, _elementalAttackType);
            }
            _animator.Play("boom");
        }
    }
}
