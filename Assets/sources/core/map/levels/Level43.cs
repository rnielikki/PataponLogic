using UnityEngine;

namespace PataRoad.Core.Map.Levels
{
    public class Level43 : MonoBehaviour
    {
        void Start()
        {
            if (!Global.GlobalData.CurrentSlot.MapInfo.NextMap.Cleared)
            {
                MissionPoint.Current.AddMissionEndAction((success) =>
                {
                    if (success)
                    {
                        Global.GlobalData.CurrentSlot.Progress.IsMusicOpen = true;
                    }
                });
            }
        }
    }
}