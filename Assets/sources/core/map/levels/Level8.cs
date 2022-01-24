using UnityEngine;

namespace PataRoad.Core.Map.Levels
{
    class Level8 : MonoBehaviour
    {
        private void Start()
        {
            MissionPoint.Current.AddMissionEndAction((success) =>
            {
                if (success)
                {
                    var progress = Global.GlobalData.CurrentSlot.Progress;
                    progress.IsSummonOpen = true;
                }
            });
        }
    }
}
