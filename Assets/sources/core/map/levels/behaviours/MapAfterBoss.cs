using UnityEngine;

namespace PataRoad.Core.Map.Levels
{
    class MapAfterBoss : MonoBehaviour
    {
        [SerializeField]
        int _mapIndex;
        private void Start()
        {
            if (!Global.GlobalData.CurrentSlot.MapInfo.NextMap.Cleared)
            {
                MissionPoint.Current.AddMissionEndAction((success) =>
                {
                    if (success)
                    {
                        Global.GlobalData.CurrentSlot.MapInfo.OpenInIndex(_mapIndex);
                    }
                });
            }
        }
    }
}
