using UnityEngine;

namespace PataRoad.Core.Map.Weather
{
    /// <summary>
    /// Logics about wind direction.
    /// </summary>
    public class Wind : MonoBehaviour
    {
        private WindZone _zone;
        private const float _windRange = 3; //min is -_windRange and max is _windRange
        /// <summary>
        /// 0-1 range of wind magnitude. 0 is maximum headwind, 1 is maximum tailwind.
        /// </summary>
        public float Magnitude => _zone.windMain;
        /// <summary>
        /// Determines range unit attack distance on wind. Returns 0-1 value.
        /// </summary>
        public float AttackOffsetOnWind => Mathf.InverseLerp(-_windRange, _windRange, _zone.windMain);

        private int _miracleSeconds;
        /// <summary>
        /// In which interval wind direction is automatically changed.
        /// </summary>
        [SerializeField]
        [Tooltip("The time that wind stays at zero.")]
        private int _windStayTime;

        [SerializeField]
        [Tooltip("The bigger the value it is, the wind change occures more frequently, which is more fine and may drop performance.")]
        private int _windChangeInterval;
        private float _windChangeWaitTime;

        [SerializeField]
        [Tooltip("The time of wind change.")]
        private float _windChangeTime;
        private float _windChangeSize;
        private bool _onFixedWindDirection;

        [SerializeField]
        private int _maxMiracleTime;
        void Awake()
        {
            _zone = GetComponent<WindZone>();
            _windChangeWaitTime = _windChangeTime / _windChangeInterval;
            _windChangeSize = _windRange / _windChangeInterval;
            ChangeWindDirection();
        }
        public void StartChangingWind()
        {
            _onFixedWindDirection = false;
            StopAllCoroutines();
            ChangeWindDirection();
        }
        private void ChangeWindDirection()
        {
            if (_onFixedWindDirection) return;
            StartCoroutine(ChangeWindDirectionCoroutine());
        }
        /// <summary>
        /// Changes the wind direction in coroutine. This doesn't need to be very accurate.
        /// </summary>
        /// <returns>yield seconds, for waiting.</returns>
        System.Collections.IEnumerator ChangeWindDirectionCoroutine()
        {
            _zone.windMain = 0;
            while (true)
            {
                while (_zone.windMain > -_windRange)
                {
                    yield return new WaitForSeconds(_windChangeWaitTime);
                    _zone.windMain -= _windChangeSize;
                }
                _zone.windMain = -_windRange;
                yield return new WaitForSeconds(_windStayTime);

                while (_zone.windMain < 0)
                {
                    _zone.windMain += _windChangeSize;
                    yield return new WaitForSeconds(_windChangeWaitTime);
                }
                _zone.windMain = 0;
                yield return new WaitForSeconds(_windStayTime);

                while (_zone.windMain < _windRange)
                {
                    yield return new WaitForSeconds(_windChangeWaitTime);
                    _zone.windMain += _windChangeSize;
                }
                _zone.windMain = _windRange;
                yield return new WaitForSeconds(_windStayTime);

                while (_zone.windMain > 0)
                {
                    yield return new WaitForSeconds(_windChangeWaitTime);
                    _zone.windMain -= _windChangeSize;
                }
                _zone.windMain = 0;
                yield return new WaitForSeconds(_windStayTime);
            }
        }
        public void StartTailwind(int seconds)
        {
            _onFixedWindDirection = true;
            var miracleSeconds = _miracleSeconds;
            _miracleSeconds = Mathf.Clamp(0, miracleSeconds + seconds, _maxMiracleTime);
            if (miracleSeconds == 0)
            {
                StopAllCoroutines();
                StartCoroutine(StartTailwindCoroutine());
            }
        }
        public System.Collections.IEnumerator StartTailwindCoroutine()
        {
            _zone.windMain = _windRange;
            while (_miracleSeconds > 0)
            {
                yield return new WaitForSeconds(1);
                _miracleSeconds--;
            }
            _onFixedWindDirection = false;
            ChangeWindDirection();
        }
        public void StartHeadWind()
        {
            _zone.windMain = -_windRange;
            _onFixedWindDirection = true;
        }
    }
}
