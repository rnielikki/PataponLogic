using PataRoad.Core.Character.Patapons.Data;
using PataRoad.Core.Items;

namespace PataRoad.Core.Character
{
    class PataponData : SmallCharacterData
    {
        //private PataponClassEquipmentInfo _equipmentInfo;
        public int IndexInGroup { get; internal set; }
        public override void Init()
        {
            base.Init();
            //Equip(_equipmentInfo.RareponData);
            //if (_equipmentInfo.RareponData.Index == 0) Equip(_equipmentInfo.HelmData);
        }
        protected override void SetEquipments()
        {
            //_equipmentInfo = GlobalData.PataponInfo.GetEquipmentInfo(Type, IndexInGroup);
        }
    }
}
