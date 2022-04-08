using UnityEngine;

namespace PataRoad.Core.Map.Levels
{
    class Level4 : MonoBehaviour
    {
        [SerializeField]
        Level2 _nobodyDieScript;
        [SerializeField]
        int _altTipIndex;
        void Start()
        {
            var cleared = Global.GlobalData.CurrentSlot.MapInfo.NextMap.Cleared;
            _nobodyDieScript.enabled = !cleared;
            if (cleared)
            {
                MissionPoint.Current.AddMissionEndAction(success =>
                {
                    if (success && !Global.GlobalData.CurrentSlot.Tips.HasTipIndex(_altTipIndex))
                    {
                        Global.GlobalData.CurrentSlot.Tips.SaveTipIndex(_altTipIndex);
                    }
                });
            }
        }
    }
}
