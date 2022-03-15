using UnityEngine;

namespace PataRoad.Core.Character.Bosses
{
    public class ShooshookleArm : MonoBehaviour, IBossPart
    {
        private ParticleSystem _particle;
        [SerializeField]
        private GameObject _arm;
        [SerializeField]
        private int _fullHitPoint;
        private int _currentHitPoint;
        // Use this for initialization
        void Start()
        {
            _particle = GetComponentInChildren<ParticleSystem>();
            _currentHitPoint = _fullHitPoint;
        }
        void BreakArm()
        {
            if (!_arm.activeSelf) return;
            _arm.SetActive(false);
            _particle.Play();
        }
        void RestoreArm()
        {
            if (_arm.activeSelf) return;
            _currentHitPoint = _fullHitPoint;
            _arm.SetActive(true);
            _particle.Play();
        }

        public float TakeDamage(int damage)
        {
            _currentHitPoint -= damage;
            if (_currentHitPoint <= 0)
            {
                _currentHitPoint = 0;
                BreakArm();
            }
            return 0.8f;
        }
    }
}