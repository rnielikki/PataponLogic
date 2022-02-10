using PataRoad.Core.Character.Patapons;
using UnityEngine;

namespace PataRoad.Core.Map.Levels
{
    class GeneralOnStart : MonoBehaviour
    {
        [SerializeField]
        Character.Class.ClassType _classType;
        [SerializeField]
        bool _doNotAddAfterClear;
        private void Start()
        {
            var item = Items.ItemLoader.GetItem(Items.ItemType.Key, "Class",
                        Character.Class.ClassData.GetItemIndexFromClass(_classType));
            if (Global.GlobalData.CurrentSlot.Inventory.HasItem(item))
            {
                return;
            }
            var pataponManager = FindObjectOfType<PataponsManager>();
            var inst = PataponGroupGenerator.GetGeneralOnlyPataponGroupInstance(_classType, pataponManager.transform, pataponManager);
            pataponManager.RegisterGroup(inst.GetComponent<PataponGroup>());
            if (!_doNotAddAfterClear)
            {
                MissionPoint.Current.AddMissionEndAction((success) =>
                {
                    if (success)
                    {
                        Global.GlobalData.CurrentSlot.Inventory.AddItem(item);
                    }
                });
            }
        }
    }
}
