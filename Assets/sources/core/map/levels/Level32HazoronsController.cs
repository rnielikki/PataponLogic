using UnityEngine;

namespace PataRoad.Core.Map.Levels
{
    public class Level32HazoronsController : MonoBehaviour
    {
        private Transform _pataponsManagerTransform;
        int _cnt;
        // Use this for initialization
        void Start()
        {
            var hazorons = FindObjectsOfType<Character.Hazorons.Hazoron>();
            MissionPoint.Current.AddMissionEndAction((success) =>
            {
                if (success)
                {
                    foreach (var hazoron in hazorons) hazoron.Die();
                }
            });
        }
    }
}