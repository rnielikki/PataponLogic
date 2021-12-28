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
        //the order "which is superior"
        /// <summary>
        /// Internal conversion of <see cref="WindType"/>.
        /// </summary>
        [System.Flags]
        private enum PrioritizedWindType
        {
            None = 0,
            Changing = 1,
            HeadWind = 2,
            TailWind = 4
        }

        [SerializeField]
        private WindZone _zone;
        private const float _windRange = 3; //min is -_windRange and max is _windRange
        private const float _changingWindRange = _windRange * 0.75f; //min is -_windRange and max is _windRange
        /// <summary>
        /// 0-1 range of wind magnitude. 0 is maximum headwind, 1 is maximum tailwind.
        /// </summary>
        public float Magnitude => _zone.windMain;

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

        private WindType _defaultStatus;

        private PrioritizedWindType _windFlags;
        private static int _maxFlag { get; set; }
        private Dictionary<WindType, UnityAction> _windActions;
        private bool _isActive = true;

        public void Init(WindType windType)
        {
            _maxFlag = (int)GetFlag((WindType)(System.Enum.GetValues(typeof(WindType)).Length - 1));

            _windActions = new Dictionary<WindType, UnityAction>()
            {
                { WindType.None, StartNoWind },
                { WindType.Changing, StartChangingWind },
                { WindType.HeadWind, StartHeadWind },
                { WindType.TailWind, StartTailwind }
            };

            _windChangeWaitTime = _windChangeTime / _windChangeInterval;
            _windChangeSize = _windRange / _windChangeInterval;
            _image.Init();
            _defaultStatus = windType;
            if (_defaultStatus != 0) StartWind(_defaultStatus);
            else StartNoWind();
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
            var prioritizedType = GetFlag(type);
            if (!_isActive && type != 0) ActivateWind(true);
            if (prioritizedType > _windFlags)
            {
                if (type == WindType.None) StopAllCoroutines();
                _windActions[type]();
            }
            _windFlags |= prioritizedType;

        }
        public void StopWind(WindType type)
        {
            var prioritizedType = GetFlag(type);
            _windFlags &= ~prioritizedType;
            if (prioritizedType > _windFlags)
            {
                if (type == WindType.Changing) StopAllCoroutines();
                _windActions[GetMaxFlag(_windFlags)]();
            }
        }
        private static PrioritizedWindType GetFlag(WindType type)
        {
            if (type == 0) return 0;
            else return (PrioritizedWindType)(1 << ((int)type - 1));
        }
        private static WindType GetMaxFlag(PrioritizedWindType type)
        {
            var flag = _maxFlag;
            while (!type.HasFlag((PrioritizedWindType)flag))
            {
                flag >>= 1;
            }
            //Square root
            return (flag == 0) ? 0 : (WindType)((flag >> 2) + 1);
        }
        /// <summary>
        /// Activate or disactivate wind.
        /// </summary>
        /// <param name="on"><c>true</c> if turning on, otherwise <c>false</c>.</param>
        private void ActivateWind(bool on)
        {
            foreach (Transform children in transform)
            {
                if (children != null) children.gameObject.SetActive(on);
            }
            _isActive = on;
        }
        public void StartNoWind()
        {
            StopAllCoroutines();
            _onFixedWindDirection = true;
            UpdateWind(0);
            ActivateWind(false);
        }
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
