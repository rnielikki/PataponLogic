using PataRoad.Core.Character;
using PataRoad.Core.Character.Patapons;
using UnityEngine;

namespace PataRoad.Core.Map.Levels
{
    class GeneralOnStart : MonoBehaviour
    {
        [SerializeField]
        Character.Class.ClassType _classType;
        private void Start()
        {
            var pataponManager = FindObjectOfType<PataponsManager>();
            var inst = PataponGroupGenerator.GetGeneralOnlyPataponGroupInstance(_classType, pataponManager.transform, pataponManager);
            pataponManager.RegisterGroup(inst.GetComponent<PataponGroup>());
            MissionPoint.Current.AddMissionEndAction((success) =>
            {
                if (success)
                {
                    Global.GlobalData.CurrentSlot.Inventory.AddItem(
                        Items.ItemLoader.GetItem(Items.ItemType.Key, "Class",
                        Character.Class.ClassData.GetItemIndexFromClass(_classType))
                    );
                }
            });
        }
    }
}
