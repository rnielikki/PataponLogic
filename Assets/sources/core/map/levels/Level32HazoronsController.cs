using UnityEngine;

namespace PataRoad.Core.Map.Levels
{
    public class Level32HazoronsController : MonoBehaviour
    {
        private Character.Hazorons.Hazoron[] _hazorons;
        private Transform _pataponsManagerTransform;
        int _cnt;
        // Use this for initialization
        void Start()
        {
            _hazorons = FindObjectsOfType<Character.Hazorons.Hazoron>();
            _pataponsManagerTransform = Character.Patapons.PataponsManager.Current.transform;
        }

        // Update is called once per frame
        void Update()
        {
            if (_cnt < 1024) _cnt++;
            foreach (var hazoron in _hazorons)
            {
                if (hazoron.DefaultWorldPosition < _pataponsManagerTransform.position.x)
                {
                    if (_cnt == 999) hazoron.StatusEffectManager.SetIce(4);
                }
            }
        }
    }
}