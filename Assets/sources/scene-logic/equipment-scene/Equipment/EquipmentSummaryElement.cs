using PataRoad.Core.Character.Equipments;
using PataRoad.SceneLogic.CommonSceneLogic;
using UnityEngine;

namespace PataRoad.SceneLogic.EquipmentScene
{
    public class EquipmentSummaryElement : SummaryElement
    {
        [SerializeField]
        private bool _isGeneralMode;
        public bool IsGeneralMode => _isGeneralMode;
        [SerializeField]
        private EquipmentType _equipmentType;
        public EquipmentType EquipmentType => _equipmentType;
        private Core.Items.IItem _current;
        public Core.Items.IItem Item => _current;

        void Awake()
        {
            if (_equipmentType == EquipmentType.Rarepon && !Core.Global.GlobalData.CurrentSlot.Progress.IsRareponOpen)
            {
                gameObject.SetActive(false);
                return;
            }
            Init();
        }
        public void SetItem(Core.Items.IItem item)
        {
            _current = item;
            if (item != null)
            {
                _text.text = item.Name;
            }
            else
            {
                _text.text = "None";
            }
        }
    }
}
