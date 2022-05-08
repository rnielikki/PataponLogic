using UnityEngine;

namespace PataRoad.AppDebug
{
    public class OpenMapData : MonoBehaviour
    {
        [SerializeField]
        int _mapIndexToOpen;
        // Use this for initialization
        void Start()
        {
            /*
            var mapInfo = GlobalData.CurrentSlot.MapInfo;
            var closedList = typeof(MapInfo).GetField("_closedMaps",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .GetValue(mapInfo) as List<int>;
            closedList.Remove(_mapIndexToOpen);
            mapInfo.OpenInIndex(_mapIndexToOpen);

            //--------
            for (int i = 0; i < 9; i++)
            {
                AddKeyItemBy("Class", i);
            }

            foreach (Core.Character.Class.ClassType classType in System.Enum.GetValues(typeof(Core.Character.Class.ClassType)))
            {
                var weaponAndProtector = Core.Character.Class.ClassAttackEquipmentData.GetWeaponAndProtectorName(classType);
                AddEquipment(weaponAndProtector.weapon);
                if (weaponAndProtector.protector != null) AddEquipment(weaponAndProtector.protector);
            }
            AddEquipment("Helm");

            void AddEquipment(string name)
            {
                for (int i = 1; i < 16; i++)
                {
                    AddItemBy(Core.Items.ItemType.Equipment, name, i, 50);
                }
            }
            void AddKeyItemBy(string group, int index) => AddItemBy(Core.Items.ItemType.Key, group, index, 1);
            */
        }
    }
}