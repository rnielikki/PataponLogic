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
        private LayerMask _layerMask;

        private Transform _defaultParent;
        private void Awake()
        {
            _bombImage = _bomb.sprite;
            _layerMask = GetComponentInParent<Boss>().DistanceCalculator.LayerMask;
            _defaultParent = transform.parent;
        }
        public void HideBomb()
        {
            gameObject.SetActive(false);
            _bomb.sprite = _bombImage;
            _bomb.color = Color.white;
            transform.parent = _defaultParent;
        }
        public void StartCounter()
        {
            _animator.SetBool("timer-on", true);
            int counter = 0;
            transform.parent = transform.root.parent;
            Rhythm.RhythmTimer.Current.OnTime.AddListener(ExplodeAfterTime);
            void ExplodeAfterTime()
            {
                counter++;
                if (counter == 5)
                {
                    Rhythm.RhythmTimer.Current.OnTime.RemoveListener(ExplodeAfterTime);
                    Explode();
                    _animator.SetBool("timer-on", false);
                }
            }
        }
        public void Explode()
        {
            foreach (var target in Physics2D.OverlapCircleAll(transform.position, 2f, _layerMask))
            {
                if (!target.CompareTag("SmallCharacter")) continue;
                _boss.Attack(this, target.gameObject, target.ClosestPoint(transform.position), _attackType, _elementalAttackType);
            }
            _animator.Play("boom");
        }
    }
}
