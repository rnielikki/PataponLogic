using UnityEngine;

namespace PataRoad.Core.Character.Hazorons
{
    internal class HazoronPositionDisplay : MonoBehaviour
    {
        [SerializeField]
        private LineRenderer _line;
        [SerializeField]
        private Transform _end;
        // Start is called before the first frame update
        private Hazoron _target;
        private float _yPos;
        void Awake()
        {
            _yPos = transform.position.y;
        }

        // Update is called once per frame
        void Update()
        {
            if (_target != null)
            {
                Vector2 targetPos = _target.transform.position;
                targetPos.y = _yPos;
                _line.SetPosition(1, (targetPos.x - _target.DefaultWorldPosition) * Vector2.right);
                _end.position = targetPos;
            }
        }

        internal void StopTracking(Hazoron hazoron)
        {
            if (_target == hazoron) _target = null;
        }

        internal void TrackHazoron(Hazoron hazoron)
        {
            if (hazoron != null && _target != hazoron)
            {
                _target = hazoron;
                transform.position = new Vector2(hazoron.DefaultWorldPosition, _yPos);
            }
        }
    }
}
