using PataRoad.Core.Character;
using PataRoad.Core.Map;
using PataRoad.Core.Rhythm.Command;
using UnityEngine;

namespace Assets.sources.core.map.levels
{
    public class Level40HoshiCollision : MonoBehaviour
    {
        private int _index;
        private bool _absorbing;
        [SerializeField]
        private SpriteRenderer _hoshiponSprite;
        [SerializeField]
        private float _hitPoint;
        private float _maxHitPoint;
        private void Start()
        {
            TurnCounter.OnTurn.AddListener(SwitchActivationStatus);
            FindObjectOfType<RhythmCommand>().OnCommandCanceled.AddListener(Deactivate);
            _maxHitPoint = _hitPoint;
        }
        private void SwitchActivationStatus()
        {
            if (!TurnCounter.IsPlayerTurn)
            {
                _index = (_index + 1) % 3;
                _absorbing = _index == 0;
            }
        }
        private void Deactivate()
        {
            _index = 0;
            _absorbing = false;
        }
        private void OnTriggerEnter(Collider other)
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
                    float clr = _hitPoint / _maxHitPoint;
                    _hoshiponSprite.color = new Color(clr, clr, clr, 1);
                }
            }
        }
    }
}