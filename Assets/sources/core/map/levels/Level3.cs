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
            /*
            MissionPoint.Current.AddMissionEndAction((_) => Global.GlobalData.CurrentSlot.MapInfo.OpenInIndex(4));
            if (Global.GlobalData.CurrentSlot.Inventory.HasItem(
                Items.ItemLoader.GetItem(Items.ItemType.Key, "Song", 2)))
            {
            */
            //opens Gong
            //var gongGroup = new GameObject("GongGroup");
            //gongGroup.transform.SetParent(FindObjectOfType<Character.Patapons.PataponsManager>().transform);
            //gongGroup.AddComponent<Character.Patapons.PataponGroup>();
            //gongGroup.transform.Translate(-Character.Patapons.PataponEnvironment.GroupDistance * 4, 0, 0);
            //var gong = Instantiate(_gongObject, gongGroup.transform);
            //gong.GetComponent<Character.Patapons.Patapon>().ClassData.Attack();
            var gong = Instantiate(_gongObject, transform);
            //}
        }
    }
}
