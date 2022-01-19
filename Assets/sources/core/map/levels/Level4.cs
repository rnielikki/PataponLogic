using UnityEngine;

namespace PataRoad.Core.Map.Levels
{
    class Level4 : MonoBehaviour
    {
        [SerializeField]
        Level2 _nobodyDieScript;
        void Start()
        {
            _nobodyDieScript.enabled = !Global.GlobalData.CurrentSlot.MapInfo.NextMap.Cleared;
        }
    }
}
