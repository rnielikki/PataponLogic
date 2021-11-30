using System.Linq;
using UnityEngine;

namespace PataRoad.Core.Character.Hazorons
{
    /// <summary>
    /// Manages Hazoron positions. Also this displays closest Hazoron status.
    /// </summary>
    internal class HazoronPositionManager : MonoBehaviour
    {
        private readonly static System.Collections.Generic.List<Hazoron> _hazorons = new System.Collections.Generic.List<Hazoron>();
        [SerializeField]
        HazoronPositionDisplay _display;
        private static HazoronPositionDisplay _currentDisplay;
        // Start is called before the first frame update
        void Awake()
        {
#pragma warning disable S2696 // Instance members should not write to "static" fields
            _currentDisplay = Instantiate(_display);
#pragma warning restore S2696 // Instance members should not write to "static" fields
            _display.gameObject.SetActive(false);
        }
        public static float GetClosestHazoronPosition()
        {
            if (_hazorons.Count == 0) return Mathf.Infinity;
            return _hazorons.Min(h => h.DefaultWorldPosition);
        }
        private static Hazoron GetClosestHazoron()
        {
            return _hazorons.Aggregate((h1, h2) => h1.DefaultWorldPosition < h2.DefaultWorldPosition ? h1 : h2);
        }
        internal static void AddHazoron(Hazoron hazoron)
        {
            if (!_hazorons.Contains(hazoron))
            {
                _hazorons.Add(hazoron);
                if (!_currentDisplay.gameObject.activeSelf)
                {
                    _currentDisplay.gameObject.SetActive(true);
                }
                _currentDisplay.TrackHazoron(GetClosestHazoron());
            }
        }
        internal static void RemoveHazoron(Hazoron hazoron)
        {
            _currentDisplay.StopTracking(hazoron);
            _hazorons.Remove(hazoron);
            if (_hazorons.Count == 0)
            {
                _currentDisplay.gameObject.SetActive(false);
            }
            else
            {
                _currentDisplay.TrackHazoron(GetClosestHazoron());
            }
        }
    }
}
