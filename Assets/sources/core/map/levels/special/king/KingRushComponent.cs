using UnityEngine;

namespace PataRoad.Core.Map.Levels.King
{
    public class KingRushComponent : MonoBehaviour
    {
        [SerializeField]
        Transform _parent;
        [SerializeField]
        bool _enabled;
        [SerializeField]
        private float _rushSpeed;
        [SerializeField]
        private float _rushScale;
        private Vector3 _rushPosition;
        private Character.DistanceCalculator _calc;

        internal void Init(Character.DistanceCalculator calc)
        {
            _calc = calc;
            _rushPosition = _parent.transform.position;
            _rushPosition.x -= _rushScale;
        }
        private void Update()
        {
            if (_enabled)
            {
                var targetPos = Vector3.MoveTowards(_parent.transform.position, _rushPosition, _rushSpeed * Time.deltaTime);
                targetPos.x = _calc.GetSafeForwardPosition(targetPos.x);
                _parent.transform.position = targetPos;
            }
        }
    }
}