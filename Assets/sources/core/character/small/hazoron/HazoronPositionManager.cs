using System.Linq;
using UnityEngine;

namespace PataRoad.Core.Character.Hazorons
{
    /// <summary>
    /// Manages Hazoron positions. Also this displays closest Hazoron status.
    /// </summary>
    internal class HazoronPositionManager : MonoBehaviour
    {
        private readonly System.Collections.Generic.List<Hazoron> _hazorons = new System.Collections.Generic.List<Hazoron>();
        [SerializeField]
        HazoronPositionDisplay _display;
        internal static HazoronPositionManager Current { get; private set; }
        private Transform _pataponsTransform = null;
        private readonly System.Collections.Generic.List<Hazoron> _listToClean = new System.Collections.Generic.List<Hazoron>();
        // Start is called before the first frame update
        void Awake()
        {
            _display.gameObject.SetActive(false);
            Current = this;
        }
        internal void Init(Transform pataponsTransform) => _pataponsTransform = pataponsTransform;
        public static float GetClosestHazoronPosition()
        {
            if (Current == null || Current._hazorons.Count == 0) return Mathf.Infinity;
            return Current._hazorons.Min(h => h.DefaultWorldPosition);
        }
        private void TrackClosestHazoron()
        {
            if (_hazorons.Count == 0) return;
            Hazoron minHazoron = null;

            foreach (var hazoron in _hazorons)
            {
                if (hazoron.IsDead)
                {
                    _listToClean.Add(hazoron);
                }
                else if (minHazoron == null || hazoron.DefaultWorldPosition < minHazoron.DefaultWorldPosition)
                {
                    minHazoron = hazoron;
                }
            }
            if (_listToClean.Count > 0)
            {
                foreach (Hazoron hazoron in _listToClean)
                {
                    RemoveHazoron(hazoron, false);
                }
                _listToClean.Clear();
            }
            _display.TrackHazoron(minHazoron);
        }
        internal void AddHazoron(Hazoron hazoron)
        {
            if (!_hazorons.Contains(hazoron))
            {
                _hazorons.Add(hazoron);
                if (!_display.gameObject.activeSelf)
                {
                    _display.gameObject.SetActive(true);
                }
                TrackClosestHazoron();
            }
        }
        internal void RemoveHazoron(Hazoron hazoron) => RemoveHazoron(hazoron, true);
        private void RemoveHazoron(Hazoron hazoron, bool refresh)
        {
            _display.StopTracking(hazoron);
            _hazorons.Remove(hazoron);
            if (_hazorons.Count == 0)
            {
                _display.gameObject.SetActive(false);
            }
            else if (refresh)
            {
                TrackClosestHazoron();
            }
        }
        private void OnDestroy()
        {
            Current = null;
        }
    }
}
