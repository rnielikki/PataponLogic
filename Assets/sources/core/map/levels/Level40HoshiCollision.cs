using PataRoad.Core.Character;
using PataRoad.Core.Rhythm.Command;
using UnityEngine;

namespace PataRoad.Core.Map.Levels
{
    public class Level40HoshiCollision : MonoBehaviour, IHavingLevel
    {
        private int _index;
        private bool _absorbing;
        [SerializeField]
        private SpriteRenderer _hoshiponSprite;
        [SerializeField]
        private float _hitPoint;
        [SerializeField]
        private ParticleSystem _absorbingParticles;
        [SerializeField]
        Transform _movingTarget;
        [SerializeField]
        float _movingSpeed;
        private float _maxHitPoint;
        [SerializeField]
        MissionController _missionController;
        private void Start()
        {
            TurnCounter.OnTurn.AddListener(() =>
                PataRoad.Core.Rhythm.RhythmTimer.Current.OnNext.AddListener(SwitchActivationStatus));
            FindObjectOfType<RhythmCommand>().OnCommandCanceled.AddListener(Deactivate);
            _maxHitPoint = _hitPoint;
        }
        private void SwitchActivationStatus()
        {
            if (TurnCounter.IsPlayerTurn)
            {
                _index = (_index + 1) % 3;
                _absorbing = _index == 0;
                if (_absorbing)
                {
                    _absorbingParticles.Play();
                }
            }
        }
        private void Deactivate()
        {
            _index = 0;
            _absorbing = false;
            _absorbingParticles.Stop();
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!_absorbing) return;
            if (other.CompareTag("SmallCharacter"))
            {
                var patapon = other.GetComponentInParent<PataponData>();
                if (patapon == null) return;
                _hitPoint -= patapon.Rigidbody.mass;
                if (_hitPoint <= 0)
                {
                    _hoshiponSprite.color = Color.white;
                    MissionPoint.Current.FilledMissionCondition = true;
                    MissionPoint.Current.EndMission();
                    Deactivate();
                }
                else
                {
                    float clr = 1 - _hitPoint / _maxHitPoint;
                    _hoshiponSprite.color = new Color(clr, clr, clr, 1);
                }
            }
        }
        private void Update()
        {
            if (MissionPoint.Current.FilledMissionCondition) return;
            _movingTarget.Translate(Time.deltaTime * _movingSpeed * Vector2.right);
            if (_movingTarget.position.x >= MissionPoint.Current.MissionPointPosition.x)
            {
                _missionController.Fail(_movingTarget);
            }
        }
        public void SetLevel(int level, int absoluteMaxLevel)
        {
            _movingSpeed += level * 0.01f;
            _maxHitPoint = _hitPoint += level * 5;
        }
    }
}