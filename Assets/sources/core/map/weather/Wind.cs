using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PataRoad.Core.Map.Weather
{
    /// <summary>
    /// Logics about wind direction.
    /// </summary>
    public class Wind : MonoBehaviour
    {
        private WindZone _zone;
        private const float _windRange = 3; //min is -_windRange and max is _windRange
        private const float _changingWindRange = _windRange * 0.75f; //min is -_windRange and max is _windRange
        /// <summary>
        /// 0-1 range of wind magnitude. 0 is maximum headwind, 1 is maximum tailwind.
        /// </summary>
        public float Magnitude => _zone.windMain;
        /// <summary>
        /// Determines range unit attack distance on wind. Returns 0-1 value.
        /// </summary>
        public float AttackOffsetOnWind => Mathf.InverseLerp(-_windRange, _windRange, _zone.windMain);

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
        private WindImage _image;

        [SerializeField]
        private WindType _defaultStatus;

        private WindType _windFlags;
        private int _maxFlag;
        private Dictionary<WindType, UnityAction> _windActions;
        private bool _isActive = true;

        void Awake()
        {
            foreach (int flag in System.Enum.GetValues(typeof(WindType)))
            {
                if (_maxFlag < flag) _maxFlag = flag;
            }
            _windActions = new Dictionary<WindType, UnityAction>()
            {
                { WindType.None, StartNoWind },
                { WindType.Changing, StartChangingWind },
                { WindType.HeadWind, StartHeadWind },
                { WindType.TailWind, StartTailwind }
            };

            _zone = GetComponentInChildren<WindZone>();
            _windChangeWaitTime = _windChangeTime / _windChangeInterval;
            _windChangeSize = _windRange / _windChangeInterval;
            _image.Init();
            StartWind(_defaultStatus);
        }


        /// <summary>
        /// Changes the wind direction in coroutine. This doesn't need to be very accurate.
        /// </summary>
        /// <returns>yield seconds, for waiting.</returns>
        private System.Collections.IEnumerator ChangeWindDirectionCoroutine()
        {
            UpdateWind(0);
            while (true)
            {
                while (_zone.windMain > -_changingWindRange)
                {
                    yield return new WaitForSeconds(_windChangeWaitTime);
                    UpdateWind(_zone.windMain - _windChangeSize);
                }
                UpdateWind(-_changingWindRange);
                yield return new WaitForSeconds(_windStayTime);

                while (_zone.windMain < 0)
                {
                    yield return new WaitForSeconds(_windChangeWaitTime);
                    UpdateWind(_zone.windMain + _windChangeSize);
                }
                UpdateWind(0);
                yield return new WaitForSeconds(_windStayTime);

                while (_zone.windMain < _changingWindRange)
                {
                    yield return new WaitForSeconds(_windChangeWaitTime);
                    UpdateWind(_zone.windMain + _windChangeSize);
                }
                UpdateWind(_changingWindRange);
                yield return new WaitForSeconds(_windStayTime);

                while (_zone.windMain > 0)
                {
                    yield return new WaitForSeconds(_windChangeWaitTime);
                    UpdateWind(_zone.windMain - _windChangeSize);
                }
                UpdateWind(0);
                yield return new WaitForSeconds(_windStayTime);
            }
        }
        public void StartWind(WindType type)
        {
            if (!_isActive && type != 0) ActivateWind(true);
            type = GetMaxFlag(type);
            if (type > _windFlags)
            {
                _windActions[type]();
            }
            _windFlags |= type;
        }
        public void StopWind(WindType type)
        {
            type = GetMaxFlag(type);
            _windFlags &= ~type;
            if (type > _windFlags)
            {
                _windActions[GetMaxFlag(_windFlags)]();
            }
        }
        private WindType GetMaxFlag(WindType type)
        {
            var flag = _maxFlag;
            while (!type.HasFlag((WindType)flag))
            {
                flag >>= 1;
            }
            return (WindType)flag;
        }
        /// <summary>
        /// Activate or disactivate wind.
        /// </summary>
        /// <param name="on"><c>true</c> if turning on, otherwise <c>false</c>.</param>
        private void ActivateWind(bool on)
        {
            foreach (Transform children in transform)
            {
                children.gameObject.SetActive(on);
            }
            _isActive = on;
        }
        public void StartNoWind() => ActivateWind(false);
        private void StartChangingWind()
        {
            if (_onFixedWindDirection) return;
            StartCoroutine(ChangeWindDirectionCoroutine());
        }
        private void StartHeadWind()
        {
            _onFixedWindDirection = true;
            UpdateWind(-_windRange);
        }
        public void StartTailwind()
        {
            _onFixedWindDirection = true;
            UpdateWind(_windRange);
        }
        private void UpdateWind(float value)
        {
            value = Mathf.Clamp(-_windRange, value, _windRange);
            _zone.windMain = value;
            _image.Visualise(value, value / _windRange);
        }
    }
}
