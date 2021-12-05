using PataRoad.Core.Character.Patapons.Data;
using PataRoad.Core.Items;

namespace PataRoad.Core.Character
{
    public class PataponData : SmallCharacterData
    {
        //private PataponClassEquipmentInfo _equipmentInfo;
        public int IndexInGroup { get; internal set; }
        public bool IsGeneral { get; private set; }
        public string GeneralName { get; private set; }
        public override void Init()
        {
            base.Init();
            var general = GetComponent<Patapons.General.PataponGeneral>();
            IsGeneral = general != null;
            if (IsGeneral)
            {
                GeneralName = general.GeneralName;
            }

            //Equip(_equipmentInfo.RareponData);
            //if (_equipmentInfo.RareponData.Index == 0) Equip(_equipmentInfo.HelmData);
        }
        protected override void SetEquipments()
        {
            //_equipmentInfo = GlobalData.PataponInfo.GetEquipmentInfo(Type, IndexInGroup);
        }
        public void DisableAllEquipments()
        {
            foreach (var eq in GetComponentsInChildren<Equipments.Equipment>()) eq.enabled = false;
        }
    }
}
