using PataRoad.Core.Items;
using System.Collections.Generic;

namespace PataRoad.Core.Character
{
    public class PataponData : SmallCharacterData
    {
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
        }
        protected override IEnumerable<EquipmentData> GetEquipmentData()
            => Global.GlobalData.CurrentSlot.PataponInfo.GetCurrentEquipments(this);

        public void DisableAllEquipments()
        {
            foreach (var eq in GetComponentsInChildren<Equipments.Equipment>()) eq.enabled = false;
        }
    }
}
