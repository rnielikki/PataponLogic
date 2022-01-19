using UnityEngine;

namespace PataRoad.Core.Map.Levels
{
    class Level3 : MonoBehaviour
    {
        [SerializeField]
        GameObject _gongObject;
        private void Start()
        {
            //Ending this mission (no matter success.fail.) instantly will open Dodonga.
            MissionPoint.Current.AddMissionEndAction((_) => Global.GlobalData.CurrentSlot.MapInfo.OpenInIndex(4));

            //opens Gong if has chakachaka song.
            if (Global.GlobalData.CurrentSlot.Inventory.HasItem(
                Items.ItemLoader.GetItem(Items.ItemType.Key, "Song", 2)))
            {
                Instantiate(_gongObject);
            }

            MissionPoint.Current.AddMissionEndAction((success) =>
            {
                if (success)
                {
                    Global.GlobalData.CurrentSlot.Progress.IsRareponOpen = true;
                    Global.GlobalData.CurrentSlot.Progress.IsGongOpen = true;
                }
            });
        }
    }
}
