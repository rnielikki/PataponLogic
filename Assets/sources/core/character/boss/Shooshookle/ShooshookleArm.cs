using UnityEngine;

namespace PataRoad.Core.Character.Bosses
{
    public class ShooshookleArm : MonoBehaviour
    {
        private ParticleSystem _particle;
        [SerializeField]
        private GameObject _arm;
        [SerializeField]
        private int _fullHitPoint;
        private int _currentHitPoint;
        private ShooshookleAttack _parent;
        // Use this for initialization
        void Start()
        {
            _particle = GetComponentInChildren<ParticleSystem>();
            _currentHitPoint = _fullHitPoint;
            _parent = GetComponentInParent<ShooshookleAttack>();
        }
        void BreakArm()
        {
            if (!_arm.activeSelf) return;
            _arm.SetActive(false);
            _particle.Play();
            _parent.LoseArm();
        }
        internal void RestoreArm()
        {
            if (_arm.activeSelf) return;
            _currentHitPoint = _fullHitPoint;
            _arm.SetActive(true);
            _particle.Play();
        }

        internal void TakeDamage(int damage)
        {
            _currentHitPoint -= damage;
            if (_currentHitPoint <= 0)
            {
                _currentHitPoint = 0;
                BreakArm();
            }
        }
    }
}